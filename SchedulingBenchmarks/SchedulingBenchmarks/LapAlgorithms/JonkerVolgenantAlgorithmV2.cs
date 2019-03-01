using System.Runtime.CompilerServices;

namespace SchedulingBenchmarks.LapAlgorithms
{
    static class JonkerVolgenantAlgorithmV2
    {
        public static (int[] copulationVerticesX, int[] copulationVerticesY) RunAlgorithm(double[][] costGraph)
        {
            var copulationVerticesX = new int[costGraph.Length];
            var copulationVerticesY = new int[costGraph.Length];

            // O(n^2)
            var (matches, v) = ColumnReduction(costGraph, copulationVerticesX, copulationVerticesY);
            // O(n^2)
            var (free, freeCount) = ReductionTransfer(costGraph, matches, copulationVerticesX, v);
            // O(n^2)
            freeCount = RowReductionAugmentation(costGraph, free, freeCount, v, copulationVerticesX, copulationVerticesY);
            // O(n^2)
            freeCount = RowReductionAugmentation(costGraph, free, freeCount, v, copulationVerticesX, copulationVerticesY);
            // O(n^3) ??
            SolutionAugmentationForFreeRows(costGraph, free, freeCount, v, copulationVerticesX, copulationVerticesY);

            return (copulationVerticesX, copulationVerticesY);
        }

        /// <summary>
        /// Marks the found minimum pairs in the copulationVertices* arrays, and 
        /// returns the found minimum value for each column, and how many minimum values are found in each row
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static (int[] matches, double[] v) ColumnReduction(double[][] costGraph, int[] copulationVerticesX, int[] copulationVerticesY)
        {
            var length = costGraph.Length;
            var matches = new int[length];
            var v = new double[length];

            for (var j = length - 1; j >= 0; j--)
            {
                // Find minimum value in column, and its corresponding index
                var min = double.MaxValue;
                var minIndex = int.MaxValue;

                for (var i = 0; i < length; i++)
                {
                    if (costGraph[i][j] < min)
                    {
                        min = costGraph[i][j];
                        minIndex = i;
                    }
                }

                // Save the minimum value for each column
                v[j] = min;
                if (++matches[minIndex] == 1)
                {
                    // When this is the first found minimum value in its row, save it as a found minimum cost pair
                    copulationVerticesX[minIndex] = j;
                    copulationVerticesY[j] = minIndex;
                }
                else if (v[j] < v[copulationVerticesX[minIndex]])
                {
                    /* Otherwise, when this is not the first found minimum value in its row, but it is the smallest in the row so far 
                     * (compared to the columns right to the current - the outer for loop goes backwards)
                     * THEN save it as a found minimum cost pair, and unset the previously found pair */
                    var j1 = copulationVerticesX[minIndex];
                    copulationVerticesX[minIndex] = j;
                    copulationVerticesY[j] = minIndex;
                    copulationVerticesY[j1] = -1;
                }
                else
                {
                    // Otherwise, mark with -1 that no minimum value found for the column
                    copulationVerticesY[j] = -1;
                }
            }

            /* matches: how many minimum values are found in each row (a value is minimum in its column)
             * v: what is the minimum value in the column */
            return (matches, v);
        }

        /// <summary>
        /// Decreases the found smallest value for each column by the minimum amount of how much remained for each column to be the minimum in its row (clarification needed)
        /// Returns those rows indices who doesn't have a minimum value.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static (int[] free, int freeCount) ReductionTransfer(double[][] costGraph, int[] matches, int[] copulationVerticesX, double[] v)
        {
            var length = costGraph.Length;
            var free = new int[length];
            var freeCount = 0;

            for (var i = 0; i < length; i++)
            {
                if (matches[i] == 0)
                {
                    // When the row doesn't contain minimum value, save the row index
                    free[freeCount++] = i;
                }
                else if (matches[i] == 1)
                {
                    // When the row contains exactly one minimum value

                    /* Except the current minimum value in the row, subtract from each element the smallest value in its column (the result is always positive) and 
                     * save the smallest among these */
                    var min = double.MaxValue;

                    for (var j = 0; j < length; j++)
                    {
                        if (j != copulationVerticesX[i])
                        {
                            if (costGraph[i][j] - v[j] < min)
                            {
                                min = costGraph[i][j] - v[j];
                            }
                        }
                    }

                    // Subtract the found value from the minimum value (may be a negative number)
                    v[copulationVerticesX[i]] -= min;
                }
            }

            return (free, freeCount);
        }

        /// <summary>
        /// Maintains the v array, tries to find additional minimum cost pairs, and collects the still free rows
        /// </summary>
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int RowReductionAugmentation(double[][] costGraph, int[] free, int freeCount, double[] v, int[] copulationVerticesX, int[] copulationVerticesY)
        {
            var length = costGraph.Length;
            var newFreeCount = 0;
            var i = 0;
            var f = 0;

            // For each free row
            while (f < freeCount)
            {
                i = free[f];
                f++;

                // Find the minimum, and second minimum value (difference between the cost and the minimum cost in its column)
                var uMin = double.MaxValue;
                var uSubMin = double.MaxValue;
                var uMinColumnIndex = 0;
                var uSubMinColumnIndex = 0;

                for (var j = 0; j < length; j++)
                {
                    var h = costGraph[i][j] - v[j];
                    if (h < uSubMin)
                    {
                        if (h < uMin)
                        {
                            uSubMin = uMin;
                            uMin = h;
                            uSubMinColumnIndex = uMinColumnIndex;
                            uMinColumnIndex = j;
                        }
                        else
                        {
                            uSubMin = h;
                            uSubMinColumnIndex = j;
                        }
                    }
                }

                var uMinRowIndex = copulationVerticesY[uMinColumnIndex];
                if (uMin < uSubMin)
                {
                    // Shouldn't always be this the case? First minimum is always smaller than second minimum?
                    // Clarification needed for the value of v
                    v[uMinColumnIndex] -= (uSubMin - uMin);
                }
                else if (uMinRowIndex > -1)
                {
                    // Minimum and secondary minimum supposed to be equal in this branch

                    // Let the second minimum be the first minimum
                    uMinColumnIndex = uSubMinColumnIndex;
                    uMinRowIndex = copulationVerticesY[uSubMinColumnIndex];
                }

                // Save the minimum as a found minimum cost pair
                copulationVerticesX[i] = uMinColumnIndex;
                copulationVerticesY[uMinColumnIndex] = i;

                // If the minimum value's column had a paired row previously, now that row is free, lets save it
                if (uMinRowIndex > -1)
                {
                    if (uMin < uSubMin)
                    {
                        // Save the row index to the current index' place, so it will be the examined row in the next cycle
                        free[--f] = uMinRowIndex;
                    }
                    else
                    {
                        // Minimum and secondary minimum supposed to be equal in this branch

                        /* Reuse the past part of the array, save the row index to the beginning, so it will be examined during the next invocation of this method, or 
                         * in case of this being the second invocation, the still free rows are going to be used in the next method */
                        free[newFreeCount++] = uMinRowIndex;
                    }
                }
            }

            return newFreeCount;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SolutionAugmentationForFreeRows(double[][] costGraph, int[] free, int freeCount, double[] v, int[] copulationVerticesX, int[] copulationVerticesY)
        {
            var length = costGraph.Length;
            var endOfPath = 0;
            var min = 0.0;

            var colList = new int[length];
            var d = new double[length];
            var pred = new int[length];

            for (var f = 0; f < freeCount; f++)
            {
                var freeRow = free[f];

                // Dijkstra shortest path algorithm
                for (var j = length - 1; j >= 0; j--)
                {
                    d[j] = costGraph[freeRow][j] - v[j];
                    pred[j] = freeRow;
                    colList[j] = j;
                }

                var low = 0;
                var up = 0;
                var last = 0;
                var unassignedFound = false;

                do
                {
                    if (up == low)
                    {
                        last = low - 1;

                        min = d[colList[up++]];
                        for (var k = up; k < length; k++)
                        {
                            var j = colList[k];
                            var h = d[j];

                            if (h <= min)
                            {
                                if (h < min)
                                {
                                    up = low;
                                    min = h;
                                }

                                colList[k] = colList[up];
                                colList[up++] = j;
                            }
                        }

                        for (var k = low; k < up; k++)
                        {
                            if (copulationVerticesY[colList[k]] < 0)
                            {
                                endOfPath = colList[k];
                                unassignedFound = true;
                                break;
                            }
                        }
                    }

                    if (!unassignedFound)
                    {
                        var j1 = colList[low];
                        low++;
                        var ii = copulationVerticesY[j1];
                        var h = costGraph[ii][j1] - v[j1] - min;

                        for (var k = up; k < length; k++)
                        {
                            var j = colList[k];
                            var v2 = costGraph[ii][j] - v[j] - h;
                            if (v2 < d[j])
                            {
                                pred[j] = ii;
                                if (v2 == min)
                                {
                                    if (copulationVerticesY[j] < 0)
                                    {
                                        endOfPath = j;
                                        unassignedFound = true;
                                        break;
                                    }
                                    else
                                    {
                                        colList[k] = colList[up];
                                        colList[up++] = j;
                                    }
                                }

                                d[j] = v2;
                            }
                        }
                    }
                } while (!unassignedFound);

                for (var k = last + 1; k >= 0; k--)
                {
                    var j1 = colList[k];
                    v[j1] = v[j1] + d[j1] - min;
                }

                var i = 0;

                do
                {
                    i = pred[endOfPath];
                    copulationVerticesY[endOfPath] = i;
                    var j1 = endOfPath;
                    endOfPath = copulationVerticesX[i];
                    copulationVerticesX[i] = j1;
                } while (i != freeRow);
            }
        }
    }
}

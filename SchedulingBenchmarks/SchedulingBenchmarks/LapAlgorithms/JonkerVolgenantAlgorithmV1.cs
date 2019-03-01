using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulingBenchmarks.LapAlgorithms
{
    static class JonkerVolgenantAlgorithmV1
    {
        private const double BIG = double.MaxValue;

        public static (int[] copulationVerticesX, int[] copulationVerticesY) RunAlgorithm(double[][] costGraph)
        {
            var dim = costGraph.Length;
            var copulationVerticesX = new int[dim];
            var copulationVerticesY = new int[dim];

            var rowSol = new int[dim];
            var colSol = new int[dim];
            var u = new double[dim];
            var v = new double[dim];

            bool unassignedFound;
            int i, imin, numFree = 0;
            int prvNumFree, f, i0, k, freeRow;
            int j, j1, j2, endOfPath, last, low, up;
            double min, h, umin, uSubMin, v2;

            var free = new int[dim];
            var colList = new int[dim];
            var matches = new int[dim];
            var d = new double[dim];
            var pred = new int[dim];

            // ??
            j2 = 0;
            endOfPath = 0;
            min = 0;
            last = 0;

            // COLUMN REDUCTION
            for (j = dim - 1; j >= 0; j--)
            {
                min = costGraph[0][j];
                imin = 0;

                for (i = 1; i < dim; i++)
                {
                    if (costGraph[i][j] < min)
                    {
                        min = costGraph[i][j];
                        imin = i;
                    }
                }

                v[j] = min;
                if (++matches[imin] == 1)
                {
                    rowSol[imin] = j;
                    colSol[j] = imin;
                }
                else if (v[j] < v[rowSol[imin]])
                {
                    j1 = rowSol[imin];
                    rowSol[imin] = j;
                    colSol[j] = imin;
                    colSol[j1] = -1;
                }
                else
                {
                    colSol[j] = -1;
                }
            }

            // REDUCTION TRANSFER
            for (i = 0; i < dim; i++)
            {
                if (matches[i] == 0)
                {
                    free[numFree++] = i;
                }
                else
                {
                    if (matches[i] == 1)
                    {
                        j1 = rowSol[i];
                        min = BIG;
                        for (j = 0; j < dim; j++)
                        {
                            if (j != j1)
                            {
                                if (costGraph[i][j] - v[j] < min)
                                {
                                    min = costGraph[i][j] - v[j];
                                }
                            }
                        }

                        v[j1] = v[j1] - min;
                    }
                }
            }

            // AUGMENTING ROW REDUCTION
            int loopcnt = 0;
            do
            {
                loopcnt++;

                k = 0;
                prvNumFree = numFree;
                numFree = 0;

                while (k < prvNumFree)
                {
                    i = free[k];
                    k++;

                    umin = costGraph[i][0] - v[0];
                    j1 = 0;
                    uSubMin = BIG;

                    for (j = 1; j < dim; j++)
                    {
                        h = costGraph[i][j] - v[j];
                        if (h < uSubMin)
                        {
                            if (h >= umin)
                            {
                                uSubMin = h;
                                j2 = j;
                            }
                            else
                            {
                                uSubMin = umin;
                                umin = h;
                                j2 = j1;
                                j1 = j;
                            }
                        }
                    }

                    i0 = colSol[j1];
                    if (umin < uSubMin)
                    {
                        v[j1] = v[j1] - (uSubMin - umin);
                    }
                    else
                    {
                        if (i0 > -1)
                        {
                            j1 = j2;
                            i0 = colSol[j2];
                        }
                    }

                    rowSol[i] = j1;
                    colSol[j1] = i;

                    if (i0 > -1)
                    {
                        if (umin < uSubMin)
                        {
                            free[--k] = i0;
                        }
                        else
                        {
                            free[numFree++] = i0;
                        }
                    }
                }
            } while (loopcnt < 2);

            // AUGMENT SOLUTION for each free row
            for (f = 0; f < numFree; f++)
            {
                freeRow = free[f];

                for (j = dim - 1; j >= 0; j--)
                {
                    d[j] = costGraph[freeRow][j] - v[j];
                    pred[j] = freeRow;
                    colList[j] = j;
                }

                low = 0;
                up = 0;

                unassignedFound = false;

                do
                {
                    if (up == low)
                    {
                        last = low - 1;

                        min = d[colList[up++]];
                        for (k = up; k < dim; k++)
                        {
                            j = colList[k];
                            h = d[j];

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

                        for (k = low; k < up; k++)
                        {
                            if (colSol[colList[k]] < 0)
                            {
                                endOfPath = colList[k];
                                unassignedFound = true;
                                break;
                            }
                        }
                    }

                    if (!unassignedFound)
                    {
                        j1 = colList[low];
                        low++;
                        i = colSol[j1];
                        h = costGraph[i][j1] - v[j1] - min;

                        for (k = up; k < dim; k++)
                        {
                            j = colList[k];
                            v2 = costGraph[i][j] - v[j] - h;
                            if (v2 < d[j])
                            {
                                pred[j] = i;
                                if (v2 == min)
                                {
                                    if (colSol[j] < 0)
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

                for (k = last + 1; k >= 0; k--)
                {
                    j1 = colList[k];
                    v[j1] = v[j1] + d[j1] - min;
                }

                do
                {
                    i = pred[endOfPath];
                    colSol[endOfPath] = i;
                    j1 = endOfPath;
                    endOfPath = rowSol[i];
                    rowSol[i] = j1;
                } while (i != freeRow);
            }

            double lapcost = 0;
            for (i = dim - 1; i >= 0; i--)
            {
                j = rowSol[i];
                u[i] = costGraph[i][j] - v[j];
                lapcost = lapcost + costGraph[i][j];
            }

            for (i = 0; i < dim; i++)
            {
                copulationVerticesX[i] = rowSol[i];
                copulationVerticesY[i] = colSol[i];
            }

            return (copulationVerticesX, copulationVerticesY);
        }
    }
}

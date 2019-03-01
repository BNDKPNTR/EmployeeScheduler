using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace SchedulingBenchmarks.LapAlgorithms
{
    /// <summary>
    /// Az Egerváry algoritmus(Hungarian algorithm) logikáját tartalmazó osztály. Maximális súlyú párosítást keres 
    /// egy egyenlő pontú (négyzetes) páros gráfban.
    /// </summary>
    static class EgervaryAlgorithmV2
    {
        private const double EPSILON = 0.0000000001; 

        /// <summary>
        /// A párosítás megkezdése előtt a cimkézés incializálása
        /// </summary>
        private static double Initialize(double[][] graph, double[] verticesXLabel, double[] verticesYLabel)
        {
            double maximumEdge = 0;
            for (int i = 0; i < verticesXLabel.Length; i++)
            {
                double max = 0;
                for (int j = 0; j < verticesYLabel.Length; j++)
                {
                    if (graph[i][j] > max) max = graph[i][j];
                }
                verticesXLabel[i] = max;
                if (max > maximumEdge) maximumEdge = max;

                verticesYLabel[i] = 0;
            }

            return maximumEdge;
        }

        /// <summary>
        /// A gráf előkészítése melyen a magyar módszer futhat (tehát ahol a két érintett pont cimkézésének összege megegyezik az él súlyával)
        /// </summary>
        private static void FillEqualityGraph(double[][] graph, double[] verticesXLabel, double[] verticesYLabel, ref bool[][] equalityGraph)
        {
            for (int i = 0; i < verticesXLabel.Length; i++)
            {
                for (int j = 0; j < verticesYLabel.Length; j++)
                {
                    equalityGraph[i][j] = Math.Abs(graph[i][j] - (verticesXLabel[i] + verticesYLabel[j])) < EPSILON;
                }
            }
        }

        private static bool[][] CreateEqualityGraph(int length)
        {
            var equalityGraph = new bool[length][];

            for (int i = 0; i < length; i++)
            {
                equalityGraph[i] = new bool[length];
            }
            
            return equalityGraph;
        }

        private static int[][] CreateVertices(int length)
        {
            var vertices = new int[2][];
            vertices[0] = new int[length];
            vertices[1] = new int[length];

            return vertices;
        }

        /// <summary>
        /// Az algoritmus lefuttatása (magyar módszer futtatása és a cimkézés frissítése ciklikusan).
        /// A mátrixnak, mely leírja a páros gráfot négyzetesnek kell lennie, tehát a páros gráf két
        /// ponthalmazásank számossága szükségszerűen egyenlő.
        /// </summary>
        public static (int[] copulationVerticesX, int[] copulationVerticesY) RunAlgorithm(double[][] costMatrix, double maxCost)
        {
            var copulationVerticesX = new int[costMatrix.Length];
            var copulationVerticesY = new int[costMatrix.Length];
            var graph = new double[costMatrix.Length][];

            for (int i = 0; i < costMatrix.Length; i++)
            {
                graph[i] = new double[costMatrix.Length];

                for (int j = 0; j < costMatrix.Length; j++)
                {
                    graph[i][j] = maxCost - costMatrix[i][j];
                }
            }

            RunAlgorithmInternal(graph, copulationVerticesX, copulationVerticesY);

            return (copulationVerticesX, copulationVerticesY);
        }

        private static void RunAlgorithmInternal(double[][] graph, int[] copulationVerticesX, int[] copulationVerticesY)
        {
            var verticesXLabel = new double[copulationVerticesX.Length];
            var verticesYLabel = new double[copulationVerticesY.Length];

            var maximumEdge = Initialize(graph, verticesXLabel, verticesYLabel);
            var equalityGraph = CreateEqualityGraph(verticesXLabel.Length);
            var vertices = CreateVertices(copulationVerticesX.Length);
            
            var verticesXLabelHunAlg = new int[copulationVerticesX.Length];
            var verticesYLabelHunAlg = new int[copulationVerticesY.Length];
            var notCopulatedXVertices = new int[verticesXLabelHunAlg.Length];

            var F1UF2 = new int[copulationVerticesX.Length];
            var L2 = new int[copulationVerticesX.Length];
            var L1UL3 = new int[copulationVerticesX.Length];

            var F1UF2Count = 0;
            var L2Count = 0;
            var L1UL3Count = 0;

            // végtelen ciklus amíg az algoritmus ki nem lép a kész eredménnyel
            while (true)
            {
                for (int i = 0; i < copulationVerticesX.Length; i++)
                {
                    copulationVerticesX[i] = -1;
                    copulationVerticesY[i] = -1;
                }
                
                // Gráf előállítása a magyar módszer számára (tehát ahol a címkézés egyenlő az élsúllyal)
                FillEqualityGraph(graph, verticesXLabel, verticesYLabel, ref equalityGraph);

                // A magyar módszer futtatása a párosításhoz
                var copulationDone = HungarianAlgorithmV2.RunAlgorithm(equalityGraph, copulationVerticesX, copulationVerticesY, verticesXLabelHunAlg, verticesYLabelHunAlg, notCopulatedXVertices, vertices);


                // Ha van teljes párosítás, kilépés
                if (copulationDone)
                {
                    return;
                }

                // Ha nincs teljes párosítás akkor a cimkézés változtatása

                for (int i = 0; i < copulationVerticesX.Length; i++)
                {
                    // a cimkézésben változtatandó "alsó" (X ponthalmaz) pontok meghatározása
                    if (vertices[0][i] == 0)
                    {
                        F1UF2[F1UF2Count++] = i;
                    }

                    // a cimkézésben változtatandó "felső" (Y ponthalmaz) pontok meghatározása    
                    if (vertices[1][i] == 0)
                    {
                        L2[L2Count++] = i;
                    }
                    else
                    {
                        L1UL3[L1UL3Count++] = i;
                    }
                }

                // a legkisebb címkézés-élsúly érték meghatározása
                double minDelta = maximumEdge + 1;

                for (int t = 0; t < F1UF2Count; ++t)
                {
                    var i = F1UF2[t];
                    for (int u = 0; u < L1UL3Count; ++u)
                    {
                        var j = L1UL3[u];
                        double delta = verticesXLabel[i] + verticesYLabel[j] - graph[i][j];
                        if (delta < minDelta) minDelta = delta;
                    }
                }

                // a cimkézésben változtatandó "alsó" (X ponthalmaz) pontok megváltoztatása (csökkentés a minimum értékkel)
                for (int i = 0; i < F1UF2Count; i++)
                {
                    verticesXLabel[F1UF2[i]] -= minDelta;
                }

                // a cimkézésben változtatandó "felső" (Y ponthalmaz) pontok megváltoztatása (növelés a minimum értékkel)
                for (int i = 0; i < L2Count; i++)
                {
                    verticesYLabel[L2[i]] += minDelta;
                }

                F1UF2Count = L2Count = L1UL3Count = 0;
            }
        }
    }
}

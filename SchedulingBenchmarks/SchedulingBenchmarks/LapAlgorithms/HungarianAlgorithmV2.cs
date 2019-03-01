using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SchedulingBenchmarks.LapAlgorithms
{
    /// <summary>
    /// A magyar módszer logikáját tartalmazó osztály. Maximális élszámú párosítást keres egy páros gráfban.
    /// </summary>
    static class HungarianAlgorithmV2
    {
        /// <summary>
        /// Javítóút keresése és felhasználása a páros gráfban
        /// </summary>
        private static bool AmendCopulation(bool[][] graph, int[] copulationVerticesX, int[] copulationVerticesY, int[] verticesXLabel, int[] verticesYLabel, int[] notCopulatedXVertices, int lengthX, int lengthY, int[][] vertices)
        {
            int notCopulatedXVerticesCount = 0;

            // Párosítási tömbök inicializálása
            for (int i = 0; i < lengthX; i++)
            {
                if (copulationVerticesX[i] == -1)
                {
                    verticesXLabel[i] = 0;
                    notCopulatedXVertices[notCopulatedXVerticesCount++] = i;
                }
                else
                {
                    verticesXLabel[i] = -1;
                }

                verticesYLabel[i] = -1;
            }

            // Javítóút keresése
            int foundedIndex = -1;
            int foundedDepth = -1;
            bool found = false;
            bool succes = true;
            // addig megyünk míg nem találtunk javítóutat, de lehet lépni
            for (int depth = 0; !found && succes; depth++)
            {
                // minden páros lépésben párt keresünk
                if (depth % 2 == 0)
                {
                    succes = false;
                    
                    // végigmegyünk azokon az "alsó" (X halmaz) pontokon ahova az előző körben léptünk
                    for (int s = 0; s < notCopulatedXVerticesCount; s++)
                    {
                        var i = notCopulatedXVertices[s];
                        // végigmegyünk a "felső" (Y halmaz) pontokon
                        for (int j = 0; j < lengthY; j++)
                        {
                            // ha van él a pontok között és a "felső" (y pont) pontban még nem jártunk
                            if (graph[i][j] && verticesYLabel[j] == -1)
                            {
                                // megjelöljük a pontot jártnak
                                verticesYLabel[j] = depth + 1;
                                succes = true;
                                // ha a "felső" pont még nem volt párosítva, akkor találtunk javítóutat
                                if (copulationVerticesY[j] == -1)
                                {
                                    found = true;
                                    foundedIndex = j;
                                    foundedDepth = depth + 1;
                                    break;
                                }
                            }
                        }
                        // ha találtunk javítóutat, kilépünk
                        if (found) break;
                    }
                }
                else
                {
                    notCopulatedXVerticesCount = 0;

                    // minden páratlanadik lépésben végigmegyünk a "felső" pontokon ahova az előző lépésben léptünk,
                    // és az ő párját megjelöljük a következő mélységgel.
                    for (int i = 0; i < lengthY; i++)
                    {
                        if (verticesYLabel[i] == depth)
                        {
                            verticesXLabel[copulationVerticesY[i]] = depth + 1;
                            notCopulatedXVertices[notCopulatedXVerticesCount++] = copulationVerticesY[i];
                        }
                    }
                }
            }

            // Ha van javítóút, akkor a párosítás megváltoztatása
            if (found)
            {
                while (foundedDepth > -1)
                {
                    // megkeresem az előző elemet a javítóútban 
                    int i = 0;
                    while (verticesXLabel[i] != foundedDepth - 1 || !graph[i][foundedIndex]) i++;

                    int temp = foundedIndex;
                    foundedIndex = copulationVerticesX[i];
                    copulationVerticesX[i] = temp;
                    copulationVerticesY[temp] = i;
                    if (foundedIndex != -1) copulationVerticesY[foundedIndex] = -1;
                    foundedDepth -= 2;
                }
                
                return true;
            }

            // Ha már nincs javítóút a nem párosított pontok megjelőlése
            for (int i = 0; i < lengthX; i++)
            {
                vertices[0][i] = verticesXLabel[i] == -1 ? -1 : 0;
                vertices[1][i] = verticesYLabel[i] == -1 ? -1 : 0;
            }

            return false;
        }

        /// <summary>
        /// Párosított pontok számolása
        /// </summary>
        private static bool VerticesCopulated(int[] copulation)
        {
            for (int i = 0; i < copulation.Length; i++)
            {
                if (copulation[i] == -1)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Az algoritmus lefuttatása (addig dolgozik amíg nincs minden pont párosítva vagy nincs javítóút már)
        /// </summary>
        public static bool RunAlgorithm(bool[][] graph, int[] copulationVerticesX, int[] copulationVerticesY, int[] verticesXLabel, int[] verticesYLabel, int[] notCopulatedXVertices, int[][] vertices)
        {
            // Ciklus amíg nincs minden pont párosítva
            while (!VerticesCopulated(copulationVerticesX))
            {
                bool success = AmendCopulation(graph, copulationVerticesX, copulationVerticesY, verticesXLabel, verticesYLabel, notCopulatedXVertices, copulationVerticesX.Length, copulationVerticesY.Length, vertices);
                if (!success)
                {
                    // Kilépés ha nincs több javítóut
                    return false;
                }
            }
            
            return true;
        }
    }
}

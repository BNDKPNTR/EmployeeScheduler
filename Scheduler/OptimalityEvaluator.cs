using Scheduler.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheduler
{
    static class OptimalityEvaluator
    {
        public static int CalculateCost(InputModel result)
        {
            var totalCost = 0;

            foreach (var person in result.People)
            {
                if (person.Assignments.Count > 2)
                {
                    var mostCommonWorkStartCount = person.Assignments
                        .GroupBy(a => a.Start.TimeOfDay, (key, elements) => elements.Count())
                        .Max();

                    const double twoThird = 2.0 / 3.0;
                    if (mostCommonWorkStartCount / (double)person.Assignments.Count <= twoThird)
                    {
                        totalCost += person.Assignments.Count;
                    }
                }
            }

            return totalCost;
        }
    }
}

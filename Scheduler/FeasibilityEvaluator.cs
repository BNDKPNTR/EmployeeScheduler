using Scheduler.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheduler
{
    static class FeasibilityEvaluator
    {
        public static bool Feasible(InputModel result)
        {
            if (!AllDemandHasExactlyOneAssignment(result)) return false;

            foreach (var person in result.People)
            {
                if (HasAssignmentOutsideOfAvailability(person)) return false;
                if (HasTwoAssignmentAtTheSameTime(person)) return false;
            }

            return true;
        }

        private static bool HasAssignmentOutsideOfAvailability(Person person)
        {
            var assignments = new Queue<Assignment>(person.Assignments);

            while (assignments.Count > 0)
            {
                var assignment = assignments.Dequeue();

                if (person.Availabilities.Any(a => !(a.Start <= assignment.Start && assignment.End <= a.End)))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool HasTwoAssignmentAtTheSameTime(Person person)
        {
            var comparer = EqualityComparer.Create<Assignment>((a, b) => a.Start == b.Start && a.End == b.End);

            return person.Assignments.Distinct(comparer).Count() != person.Assignments.Count;
        }

        private static bool AllDemandHasExactlyOneAssignment(InputModel result)
        {
            var allAssignments = result.People.SelectMany(p => p.Assignments).ToList();

            foreach (var demand in result.Demands)
            {
                for (int i = 0; i < demand.RequiredPersonCount; i++)
                {
                    Assignment assignment = null;

                    for (int j = 0; j < allAssignments.Count; j++)
                    {
                        assignment = allAssignments[j];

                        if (Matches(assignment, demand))
                        {
                            allAssignments.RemoveAt(j);
                            break;
                        }
                    }

                    if (assignment is null) return false;
                }
            }

            return allAssignments.Count == 0;

            bool Matches(Assignment assignment, Demand demand) => 
                assignment.Start == demand.Start 
                && assignment.End == demand.End 
                && assignment.Activity.Id == demand.Activity.Id;
        }
    }
}

using Scheduler.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheduler
{
    static class ResultEvaluator
    {
        public static bool Evaluate(InputModel result)
        {
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
    }
}

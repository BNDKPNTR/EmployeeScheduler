using SchedulingBenchmarks.CostFunctions;
using SchedulingBenchmarks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulingBenchmarks.Schedulers
{
    internal class RemoveUnderMinConsecutiveShiftsScheduler : SchedulerBase
    {
        public RemoveUnderMinConsecutiveShiftsScheduler(SchedulerModel model, CostFunctionBase costFunction, WorkEligibilityChecker workEligibilityChecker) 
            : base(model, costFunction, workEligibilityChecker)
        {
        }

        public override void Run()
        {
            Parallel.ForEach(_model.People, person =>
            {
                foreach (var assignmentGroup in GetAssignmentGroups(person).Where(r => r.Start != 0 && r.End != _model.SchedulePeriod.End))
                {
                    if (assignmentGroup.Length < person.WorkSchedule.MinConsecutiveWorkDays)
                    {
                        foreach (var day in assignmentGroup)
                        {
                            person.Assignments.Remove(day);
                        }
                    }
                }
            });
        }

        private List<Range> GetAssignmentGroups(Person p)
        {
            var assignmentGroups = new List<Range>();

            if (p.Assignments.AllRounds.Count == 0) return assignmentGroups;

            var firstAssignmentInRow = p.Assignments.AllRounds.Values.First();
            var lastAssignmentInRow = firstAssignmentInRow;

            foreach (var assignment in p.Assignments.AllRounds.Values.Skip(1))
            {
                if (assignment.Day != lastAssignmentInRow.Day + 1)
                {
                    assignmentGroups.Add(Range.Of(firstAssignmentInRow.Day, lastAssignmentInRow.Day));

                    firstAssignmentInRow = assignment;
                    lastAssignmentInRow = firstAssignmentInRow;
                }
                else
                {
                    lastAssignmentInRow = assignment;
                }
            }

            assignmentGroups.Add(Range.Of(firstAssignmentInRow.Day, lastAssignmentInRow.Day));

            return assignmentGroups;
        }
    }
}

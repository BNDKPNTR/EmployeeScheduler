using Google.OrTools.LinearSolver;
using System.Linq;

namespace IPScheduler.Common.Mapper
{
    public static class ObjectiveMapper
    {
        public static void MapObjectives(SchedulingIpContext context)
        {
            var objective = context.Solver.Objective();

            SetObjectiveForShiftOnRequests(objective, context);
            SetObjectiveForShiftOffRequests(objective, context);
            SetObjectiveForCoverRequirements(objective, context);
            objective.SetMinimization();
        }

        private static void SetObjectiveForCoverRequirements(Objective objective, SchedulingIpContext context)
        {
            foreach (var assignment in context.Assignments)
            {
                objective.SetCoefficient(assignment.Shift.OverMax, assignment.Shift.MaxWeight);
                objective.SetCoefficient(assignment.Shift.UnderMin, assignment.Shift.MinWeight);
            }
        }

        private static void SetObjectiveForShiftOffRequests(Objective objective, SchedulingIpContext context)
        {
            foreach (var shiftOffRequest in context.Persons.Values.SelectMany(person =>
                person.ShiftOffRequests.Values))
            {
                objective.SetCoefficient(shiftOffRequest.ShiftOffRrequestVariable, shiftOffRequest.Weight);
            }
        }

        private static void SetObjectiveForShiftOnRequests(Objective objective, SchedulingIpContext context)
        {
            foreach (var shiftOnRequest in context.Persons.Values.SelectMany(person =>
                person.ShiftOnRequests.Values))
            {
                objective.SetCoefficient(shiftOnRequest.ShiftOnRrequestVariable, shiftOnRequest.Weight);
            }
        }
    }
}

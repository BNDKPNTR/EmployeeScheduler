using SchedulingBenchmarks.SchedulingBenchmarksModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchedulingBenchmarks.Evaluators
{
    public class OptimalityEvaluator
    {
        private readonly SchedulingBenchmarkModel _schedulingBenchmarkModel;

        public OptimalityEvaluator(SchedulingBenchmarkModel schedulingBenchmarkModel)
        {
            _schedulingBenchmarkModel = schedulingBenchmarkModel ?? throw new ArgumentNullException(nameof(schedulingBenchmarkModel));
        }

        public static int CalculatePenalty(SchedulingBenchmarkModel schedulingBenchmarkModel) => new OptimalityEvaluator(schedulingBenchmarkModel).CalculatePenalty();

        private int CalculatePenalty()
        {
            var totalPenalty = _schedulingBenchmarkModel.Employees
                .AsParallel()
                .Sum(e => CalculateShiftOnRequestPenalty(e) + CalculateShiftOffRequestPenalty(e));

            return totalPenalty + CalculateCoverPenalty();
        }

        private int CalculateShiftOnRequestPenalty(Employee employee)
        {
            var penalty = 0;

            foreach (var request in employee.ShiftOnRequests)
            {
                if (!employee.Assignments.TryGetValue(request.Day, out var assignment) || assignment.ShiftId != request.ShiftId)
                {
                    penalty += request.Penalty;
                }
            }

            return penalty;
        }

        private int CalculateShiftOffRequestPenalty(Employee employee)
        {
            var penalty = 0;

            foreach (var request in employee.ShiftOffRequests)
            {
                if (employee.Assignments.TryGetValue(request.Day, out var assignment) && assignment.ShiftId == request.ShiftId)
                {
                    penalty += request.Penalty;
                }
            }

            return penalty;
        }

        private int CalculateCoverPenalty()
        {
            var shiftCountsPerDay = _schedulingBenchmarkModel.Employees
                .SelectMany(e => e.Assignments.Values)
                .GroupBy(a => a.Day)
                .ToDictionary(g => g.Key, g => g.GroupBy(a => a.ShiftId).ToDictionary(x => x.Key, x => x.Count()));

            var penalty = 0;
            var emptyDictionary = new Dictionary<string, int>(0);

            foreach (var kvp in _schedulingBenchmarkModel.Demands)
            {
                var day = kvp.Key;
                var demands = kvp.Value;

                if (!shiftCountsPerDay.TryGetValue(day, out var shiftCountsForDay))
                {
                    shiftCountsForDay = emptyDictionary;
                }

                foreach (var demand in demands)
                {
                    shiftCountsForDay.TryGetValue(demand.ShiftId, out var shiftCountForDemand);

                    if (shiftCountForDemand < demand.MinEmployeeCount)
                    {
                        penalty += (demand.MinEmployeeCount - shiftCountForDemand) * demand.UnderMinEmployeeCountPenalty;
                    }

                    if (shiftCountForDemand > demand.MaxEmployeeCount)
                    {
                        penalty += (shiftCountForDemand - demand.MaxEmployeeCount) * demand.OverMaxEmployeeCountPenalty;
                    }
                }
            }

            return penalty;
        }
    }
}

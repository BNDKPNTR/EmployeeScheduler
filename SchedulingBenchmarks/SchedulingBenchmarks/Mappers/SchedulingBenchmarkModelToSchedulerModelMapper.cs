using SchedulingBenchmarks.Models;
using SchedulingBenchmarks.SchedulingBenchmarksModel;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

using SchedulerDemand = SchedulingBenchmarks.Models.Demand;
using Demand = SchedulingBenchmarks.SchedulingBenchmarksModel.Demand;

namespace SchedulingBenchmarks.Mappers
{
    class SchedulingBenchmarkModelToSchedulerModelMapper
    {
        private readonly SchedulingBenchmarkModel _schedulingBenchmarkModel;

        private SchedulingBenchmarkModelToSchedulerModelMapper(SchedulingBenchmarkModel schedulingBenchmarkModel)
        {
            _schedulingBenchmarkModel = schedulingBenchmarkModel ?? throw new ArgumentNullException(nameof(schedulingBenchmarkModel));
        }

        public static SchedulerModel MapToScheduleModel(SchedulingBenchmarkModel schedulingBenchmarkModel) 
            => new SchedulingBenchmarkModelToSchedulerModelMapper(schedulingBenchmarkModel).MapToScheduleModel();

        private SchedulerModel MapToScheduleModel()
        {
            var model = new SchedulerModel();

            model.SchedulePeriod = Range.Of(start: 0, length: _schedulingBenchmarkModel.Duration);
            model.People = MapPeople(model.SchedulePeriod);
            model.Demands = MapDemands(model.SchedulePeriod);

            return model;
        }

        private List<Person> MapPeople(Range schedulePeriod)
        {
            var people = new List<Person>();

            foreach (var employee in _schedulingBenchmarkModel.Employees)
            {
                var person = new Person(
                    employee.Id,
                    CreateState(),
                    MapWorkSchedule(employee.Contract),
                    MapAvailabilities(employee, schedulePeriod),
                    MapShiftOffRequests(employee, schedulePeriod),
                    MapShiftOnRequests(employee, schedulePeriod));

                people.Add(person);
            }

            return people;
        }

        private State CreateState()
        {
            return new State
            {
                TotalWorkTime = 0,
                ConsecutiveShiftCount = 0,
                DayOffCount = int.MaxValue / 2,
                WorkedOnWeeked = false,
                WorkedWeekendCount = 0,
                ShiftWorkedCount = new Dictionary<string, int>()
            };
        }

        private bool[] MapAvailabilities(Employee employee, Range schedulePeriod)
        {
            var availabilities = new bool[schedulePeriod.Length];

            for (int i = 0; i < availabilities.Length; i++)
            {
                if (!employee.DayOffs.Contains(i))
                {
                    availabilities[i] = true;
                }
            }

            return availabilities;
        }

        private bool[] MapShiftOffRequests(Employee employee, Range schedulePeriod)
        {
            var shiftOffRequests = new bool[schedulePeriod.Length];
            
            // TODO: include shift types
            foreach (var request in employee.ShiftOffRequests)
            {
                shiftOffRequests[request.Day] = true;
            }

            return shiftOffRequests;
        }

        private WorkSchedule MapWorkSchedule(Contract contract)
        {
            return new WorkSchedule(
                contract.MinRestTime,
                contract.MinTotalWorkTime,
                contract.MaxTotalWorkTime,
                contract.MinConsecutiveShifts,
                contract.MaxConsecutiveShifts,
                contract.MinConsecutiveDayOffs,
                contract.MaxWorkingWeekendCount,
                contract.ValidShiftIds,
                contract.MaxShifts.ToDictionary(x => x.Key, x => x.Value));
        }

        private bool[] MapShiftOnRequests(Employee employee, Range schedulePeriod)
        {
            var shiftOnRequests = new bool[schedulePeriod.Length];

            // TODO: include shift types
            foreach (var request in employee.ShiftOnRequests)
            {
                shiftOnRequests[request.Day] = true;
            }

            return shiftOnRequests;
        }

        private Dictionary<int, SchedulerDemand[]> MapDemands(Range schedulePeriod)
        {
            return _schedulingBenchmarkModel.Demands.ToDictionary(x => x.Key, x => x.Value.Select(d => MapDemand(d)).ToArray());

            SchedulerDemand MapDemand(Demand demand) => new SchedulerDemand(demand.Day, demand.ShiftId, demand.MinEmployeeCount, demand.MaxEmployeeCount);
        }
    }
}

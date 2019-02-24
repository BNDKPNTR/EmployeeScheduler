using SchedulingBenchmarks.Dto;
using SchedulingBenchmarks.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace SchedulingBenchmarks.Mappers
{
    class SchedulingPeriodMapper
    {
        private readonly SchedulingPeriod _schedulingBenchmarkModel;

        private SchedulingPeriodMapper(SchedulingPeriod inputModel)
        {
            _schedulingBenchmarkModel = inputModel ?? throw new ArgumentNullException(nameof(inputModel));
        }

        public static SchedulerModel MapToScheduleModel(SchedulingPeriod inputModel) 
            => new SchedulingPeriodMapper(inputModel).MapToScheduleModel();

        private SchedulerModel MapToScheduleModel()
        {
            var model = new SchedulerModel();

            model.Calendar = new Calendar(_schedulingBenchmarkModel.StartDate, 24 * 60);
            model.SchedulePeriod = Range.Of(
                start: model.Calendar.DateTimeToTimeSlotIndex(_schedulingBenchmarkModel.StartDate),
                end: model.Calendar.DateTimeToTimeSlotIndex(_schedulingBenchmarkModel.EndDate));

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
                    MapWorkSchedule(employee.ContractIds),
                    MapAvailabilities(employee.Id, schedulePeriod),
                    MapShiftOffRequests(employee.Id, schedulePeriod),
                    MapShiftOnRequests(employee.Id, schedulePeriod));

                people.Add(person);
            }

            return people;
        }

        private State CreateState()
        {
            return new State
            {
                TimeSlotsWorked = 0,
                TimeSlotsWorkedToday = 0,
                WorkedDaysInMonthCount = 0,
                DailyWorkStartCounts = ImmutableDictionary<int, int>.Empty
            };
        }

        private bool[] MapAvailabilities(string employeeId, Range schedulePeriod)
        {
            var availabilities = new bool[schedulePeriod.Length];

            var emptyShiftsOfEmployee = _schedulingBenchmarkModel.FixedAssignments
                .Where(a => a.EmployeeId == employeeId && a.Assign.Shift == Shift.NoneShiftId);

            var notAvailable = new HashSet<int>(emptyShiftsOfEmployee.Select(a => a.Assign.Day));

            for (int i = 0; i < availabilities.Length; i++)
            {
                if (!notAvailable.Contains(i))
                {
                    availabilities[i] = true;
                }
            }

            return availabilities;
        }

        private bool[] MapShiftOffRequests(string employeeId, Range schedulePeriod)
        {
            var shiftOffRequests = new bool[schedulePeriod.Length];

            // TODO: include shift types
            var requestsOfEmployee = _schedulingBenchmarkModel.ShiftOffRequests
                .Where(x => x.EmployeeID == employeeId)
                .Select(x => x.Day);

            foreach (var day in requestsOfEmployee)
            {
                shiftOffRequests[day] = true;
            }

            return shiftOffRequests;
        }

        private WorkSchedule MapWorkSchedule(string[] contractIds)
        {
            var minRestTime = contractIds.Contains(Contract.UniversalContractId)
                ? _schedulingBenchmarkModel.Contracts.SingleOrDefault(c => c.ID == Contract.UniversalContractId)?.MinRestTime.Value ?? 0
                : 0;

            var contract = _schedulingBenchmarkModel.Contracts.Single(c => c.ID == contractIds[1]);

            var minTotalWorkTime = contract.Workload.First(x => x.Min != null).Min.Count;
            var maxTotalWorkTime = contract.Workload.First(x => x.Max != null).Max.Count;
            var minConsecutiveShifts = contract.MinSeq.First(x => x.Shift == Shift.AnyShiftId).Value;
            var maxConsecutiveShifts = contract.MaxSeq.Value;
            var minConsecutiveDayOffs = contract.MinSeq.First(x => x.Shift == Shift.NoneShiftId).Value;

            return new WorkSchedule(
                minRestTime,
                minTotalWorkTime,
                maxTotalWorkTime,
                minConsecutiveShifts,
                maxConsecutiveShifts,
                minConsecutiveDayOffs,
                1);
        }

        private bool[] MapShiftOnRequests(string employeeId, Range schedulePeriod)
        {
            var shiftOnRequests = new bool[schedulePeriod.Length];

            // TODO: include shift types
            var requestsOfEmployee = _schedulingBenchmarkModel.ShiftOnRequests
                .Where(x => x.EmployeeID == employeeId)
                .Select(x => x.Day);

            foreach (var day in requestsOfEmployee)
            {
                shiftOnRequests[day] = true;
            }

            return shiftOnRequests;
        }

        private Demand[] MapDemands(Range schedulePeriod)
        {
            var demands = new Demand[schedulePeriod.Length];

            // TODO: include shift types
            foreach (var coverRequirement in _schedulingBenchmarkModel.CoverRequirements)
            {
                var demand = new Demand(
                    coverRequirement.Day,
                    coverRequirement.Cover.Shift,
                    coverRequirement.Cover.Min.Value,
                    coverRequirement.Cover.Max.Value);

                demands[coverRequirement.Day] = demand;
            }

            return demands;
        }
    }
}

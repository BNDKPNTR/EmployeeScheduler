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
        private readonly Dictionary<int, Activity> _activities;

        private SchedulingPeriodMapper(SchedulingPeriod inputModel)
        {
            _schedulingBenchmarkModel = inputModel ?? throw new ArgumentNullException(nameof(inputModel));
            _activities = new Dictionary<int, Activity>();
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
                .Where(a => a.EmployeeId == employeeId && a.Assign.Shift == Shift.EmptyShiftId);

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

            var requestsOfEmployee = _schedulingBenchmarkModel.ShiftOffRequests
                .Where(x => x.EmployeeID == employeeId)
                .Select(x => x.Day);

            foreach (var day in requestsOfEmployee)
            {
                shiftOffRequests[day] = true;
            }

            return shiftOffRequests;
        }

        private bool[] MapShiftOnRequests(string employeeId, Range schedulePeriod)
        {
            var shiftOnRequests = new bool[schedulePeriod.Length];

            var requestsOfEmployee = _schedulingBenchmarkModel.ShiftOnRequests
                .Where(x => x.EmployeeID == employeeId)
                .Select(x => x.Day);

            foreach (var day in requestsOfEmployee)
            {
                shiftOnRequests[day] = true;
            }

            return shiftOnRequests;
        }
    }
}

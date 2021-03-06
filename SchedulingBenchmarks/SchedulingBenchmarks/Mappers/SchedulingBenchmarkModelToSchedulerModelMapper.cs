﻿using SchedulingBenchmarks.Models;
using SchedulingBenchmarks.SchedulingBenchmarksModel;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SchedulerDemand = SchedulingBenchmarks.Models.Demand;
using Demand = SchedulingBenchmarks.SchedulingBenchmarksModel.Demand;
using SchedulerShift = SchedulingBenchmarks.Models.Shift;
using Shift = SchedulingBenchmarks.SchedulingBenchmarksModel.Shift;
using SchedulerShiftRequest = SchedulingBenchmarks.Models.ShiftRequest;
using ShiftRequest = SchedulingBenchmarks.SchedulingBenchmarksModel.ShiftRequest;

namespace SchedulingBenchmarks.Mappers
{
    class SchedulingBenchmarkModelToSchedulerModelMapper
    {
        private readonly SchedulingBenchmarkModel _schedulingBenchmarkModel;
        private readonly Dictionary<string, SchedulerShift> _shifts;

        private SchedulingBenchmarkModelToSchedulerModelMapper(SchedulingBenchmarkModel schedulingBenchmarkModel)
        {
            _schedulingBenchmarkModel = schedulingBenchmarkModel ?? throw new ArgumentNullException(nameof(schedulingBenchmarkModel));

            _shifts = MapShifts();
        }

        public static SchedulerModel MapToScheduleModel(SchedulingBenchmarkModel schedulingBenchmarkModel) 
            => new SchedulingBenchmarkModelToSchedulerModelMapper(schedulingBenchmarkModel).MapToScheduleModel();

        private SchedulerModel MapToScheduleModel()
        {
            var schedulePeriod = Range.Of(start: 0, length: _schedulingBenchmarkModel.Duration);
            var people = MapPeople(schedulePeriod);
            var demands = MapDemands(schedulePeriod);
            var calendar = new Calendar();
            var parallelOptions = new ParallelOptions()
                //{ MaxDegreeOfParallelism = 1 }
            ;

            return new SchedulerModel(
                schedulePeriod,
                people,
                demands,
                calendar,
                parallelOptions);
        }

        private List<Person> MapPeople(Range schedulePeriod)
        {
            return _schedulingBenchmarkModel.Employees.Select((e, i) => new Person(
                i,
                e.Id,
                new State(),
                MapWorkSchedule(e.Contract),
                MapAvailabilities(e, schedulePeriod),
                MapShiftRequests(e))
            ).ToList();
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

        private Dictionary<int, SchedulerShiftRequest> MapShiftRequests(Employee employee)
        {
            var shiftRequests = new Dictionary<int, SchedulerShiftRequest>();

            foreach (var request in employee.ShiftOnRequests)
            {
                AddOrThrow(request, RequestType.On);
            }

            foreach (var request in employee.ShiftOffRequests)
            {
                AddOrThrow(request, RequestType.Off);
            }

            return shiftRequests;

            void AddOrThrow(ShiftRequest request, RequestType requestType)
            {
                if (shiftRequests.ContainsKey(request.Day))
                {
                    throw new InvalidOperationException($"A shift request already exists on day {request.Day} for employee '{employee.Id}'");
                }

                shiftRequests[request.Day] = new SchedulerShiftRequest(request.ShiftId, request.Penalty, requestType);
            }
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
                new HashSet<SchedulerShift>(contract.ValidShiftIds.Select(id => _shifts[id])),
                contract.MaxShifts.ToDictionary(x => _shifts[x.Key], x => x.Value));
        }

        private Dictionary<int, SchedulerDemand[]> MapDemands(Range schedulePeriod)
        {
            return _schedulingBenchmarkModel.Demands.ToDictionary(x => x.Key, x => x.Value.Select(d => MapDemand(d)).ToArray());

            SchedulerDemand MapDemand(Demand demand)
            {
                if (demand.MinEmployeeCount != demand.MaxEmployeeCount)
                {
                    throw new Exception($"{nameof(Demand)}.{nameof(demand.MinEmployeeCount)} and {nameof(Demand)}.{nameof(demand.MaxEmployeeCount)} must be the same");
                }

                return new SchedulerDemand(demand.Day, _shifts[demand.ShiftId], demand.MaxEmployeeCount);
            }
        }

        private Dictionary<string, SchedulerShift> MapShifts()
        {
            var shifts = new Dictionary<string, SchedulerShift>();

            for (int i = 0; i < _schedulingBenchmarkModel.Shifts.Length; i++)
            {
                var shift = _schedulingBenchmarkModel.Shifts[i];

                shifts[shift.Id] = new SchedulerShift(i, shift.Id, (int)shift.StartTime.TotalMinutes, (int)shift.Duration.TotalMinutes);
            }

            return shifts;
        }
    }
}

using SchedulingBenchmarks.Dto;
using SchedulingBenchmarks.SchedulingBenchmarksModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Shift = SchedulingBenchmarks.SchedulingBenchmarksModel.Shift;
using ShiftDto = SchedulingBenchmarks.Dto.Shift;
using Employee = SchedulingBenchmarks.SchedulingBenchmarksModel.Employee;
using EmployeeDto = SchedulingBenchmarks.Dto.Employee;
using Contract = SchedulingBenchmarks.SchedulingBenchmarksModel.Contract;
using ContractDto = SchedulingBenchmarks.Dto.Contract;

namespace SchedulingBenchmarks.Mappers
{
    public class DtoToSchedulingBenchmarkModelMapper
    {
        private readonly SchedulingPeriod _schedulingPeriod;
        private readonly Dictionary<string, HashSet<int>> _dayOffs;
        private readonly Dictionary<string, List<ShiftRequest>> _shiftOffRequests;
        private readonly Dictionary<string, List<ShiftRequest>> _shiftOnRequests;
        private readonly Dictionary<string, Contract> _contracts;

        private DtoToSchedulingBenchmarkModelMapper(SchedulingPeriod schedulingPeriod)
        {
            _schedulingPeriod = schedulingPeriod ?? throw new ArgumentNullException(nameof(schedulingPeriod));
            _dayOffs = MapDayOffs();
            _shiftOffRequests = MapShiftOffRequests();
            _shiftOnRequests = MapShiftOnRequests();
            _contracts = MapContracts();
        }

        public static SchedulingBenchmarkModel MapToSchedulingBenchmarkModel(SchedulingPeriod schedulingPeriod)
            => new DtoToSchedulingBenchmarkModelMapper(schedulingPeriod).MapToSchedulingBenchmarkModel();

        private SchedulingBenchmarkModel MapToSchedulingBenchmarkModel()
        {
            var model = new SchedulingBenchmarkModel();

            model.Duration = (_schedulingPeriod.EndDate.AddDays(1) - _schedulingPeriod.StartDate).Days;
            model.Shifts = MapShifts();
            model.Demands = MapDemands();
            model.Employees = MapEmployees();

            return model;
        }

        private Shift[] MapShifts()
        {
            return _schedulingPeriod.Shifts.Select(dto => new Shift
            {
                Id = dto.ID,
                StartTime = TimeSpan.Parse(dto.StartTime),
                Duration = TimeSpan.FromMinutes(dto.Duration)
            }).ToArray();
        }

        private Employee[] MapEmployees()
        {
            var employees = new List<Employee>();

            foreach (var dto in _schedulingPeriod.Employees)
            {
                var employee = new Employee
                {
                    Id = dto.Id,
                    DayOffs = _dayOffs.TryGetValue(dto.Id, out var dayOffs) ? dayOffs : new HashSet<int>(),
                    ShiftOffRequests = _shiftOffRequests.TryGetValue(dto.Id, out var shiftOffRequests) ? shiftOffRequests.ToArray() : new ShiftRequest[] { },
                    ShiftOnRequests = _shiftOnRequests.TryGetValue(dto.Id, out var shiftOnRequests) ? shiftOnRequests.ToArray() : new ShiftRequest[] { },
                    Contract = _contracts[dto.ContractIds.Single(x => x != ContractDto.UniversalContractId)]
                };

                employees.Add(employee);
            }

            return employees.ToArray();
        }

        private Demand[] MapDemands()
        {
            return _schedulingPeriod.CoverRequirements.Select(dto => new Demand
            {
                Day = dto.Day,
                ShiftId = dto.Cover.Shift,
                MinEmployeeCount = dto.Cover.Min.Value,
                MaxEmployeeCount = dto.Cover.Max.Value,
                UnderMinEmployeeCountPenalty = dto.Cover.Min.Weight,
                OverMaxEmployeeCountPenalty = dto.Cover.Max.Weight
            }).ToArray();
        }

        private Dictionary<string, HashSet<int>> MapDayOffs()
        {
            var dayOffs = new Dictionary<string, HashSet<int>>();

            foreach (var fixedAssignment in _schedulingPeriod.FixedAssignments)
            {
                if (!dayOffs.TryGetValue(fixedAssignment.EmployeeId, out var dayNumbers))
                {
                    dayNumbers = dayOffs[fixedAssignment.EmployeeId] = new HashSet<int>();
                }

                dayNumbers.Add(fixedAssignment.Assign.Day);
            }

            return dayOffs;
        }

        private Dictionary<string, List<ShiftRequest>> MapShiftOffRequests()
        {
            var shiftOffRequests = new Dictionary<string, List<ShiftRequest>>();

            foreach (var dto in _schedulingPeriod.ShiftOffRequests)
            {
                if (!shiftOffRequests.TryGetValue(dto.EmployeeID, out var requests))
                {
                    requests = shiftOffRequests[dto.EmployeeID] = new List<ShiftRequest>();
                }

                requests.Add(new ShiftRequest
                {
                    Day = dto.Day,
                    ShiftId = dto.Shift,
                    Penalty = dto.Weight
                });
            }

            return shiftOffRequests;
        }

        private Dictionary<string, List<ShiftRequest>> MapShiftOnRequests()
        {
            var shiftOnRequests = new Dictionary<string, List<ShiftRequest>>();

            foreach (var dto in _schedulingPeriod.ShiftOnRequests)
            {
                if (!shiftOnRequests.TryGetValue(dto.EmployeeID, out var requests))
                {
                    requests = shiftOnRequests[dto.EmployeeID] = new List<ShiftRequest>();
                }

                requests.Add(new ShiftRequest
                {
                    Day = dto.Day,
                    ShiftId = dto.Shift,
                    Penalty = dto.Weight
                });
            }

            return shiftOnRequests;
        }

        private Dictionary<string, Contract> MapContracts()
        {
            var contracts = new Dictionary<string, Contract>();

            var minRestTime = _schedulingPeriod.Contracts.Single(dto => dto.ID == ContractDto.UniversalContractId).MinRestTime.Value;

            foreach (var dto in _schedulingPeriod.Contracts.Where(dto => dto.ID != ContractDto.UniversalContractId))
            {
                var minTotalWorkTime = dto.Workload.Single(x => x.Min != null).Min.Count;
                var maxTotalWorkTime = dto.Workload.Single(x => x.Max != null).Max.Count;
                var minConsecutiveShifts = dto.MinSeq.Single(x => x.Shift == ShiftDto.AnyShiftId).Value;
                var maxConsecutiveShifts = dto.MaxSeq.Value;
                var minConsecutiveDayOffs = dto.MinSeq.Single(x => x.Shift == ShiftDto.NoneShiftId).Value;

                contracts[dto.ID] = new Contract
                {
                    MinRestTime = minRestTime,
                    MinTotalWorkTime = minTotalWorkTime,
                    MaxTotalWorkTime = maxTotalWorkTime,
                    MinConsecutiveShifts = minConsecutiveShifts,
                    MaxConsecutiveShifts = maxConsecutiveShifts,
                    MinConsecutiveDayOffs = minConsecutiveDayOffs
                };
            }

            return contracts;
        }
    }
}

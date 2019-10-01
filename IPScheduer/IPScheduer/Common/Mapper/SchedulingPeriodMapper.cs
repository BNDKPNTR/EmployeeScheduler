using Google.OrTools.LinearSolver;
using IPScheduler.Inputs;
using IPScheduler.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace IPScheduler.Common.Mapper
{
    public  class SchedulingPeriodMapper
    {
        private static SchedulingIpContext scheduleContext = new SchedulingIpContext();
        public static SchedulingIpContext Map(SchedulingPeriod schedulingPeriod)
        {
            MapShiftTypes(schedulingPeriod.ShiftTypes);
            MapPersons(schedulingPeriod.Employees);
            MapContracts(schedulingPeriod.Contracts);
            MapShifts(schedulingPeriod.CoverRequirements);
            MapShiftOnRequests(schedulingPeriod.ShiftOnRequests);
            MapShiftOffRequests(schedulingPeriod.ShiftOffRequests);
            MapFixedAssignments(schedulingPeriod.FixedAssignments);

            MapExtraInfomraiton();
            return scheduleContext;
        }

        private static void MapExtraInfomraiton()
        {
            scheduleContext.DayCount = scheduleContext.Shifts.Max(s => s.Day);
            var weekCount = scheduleContext.DayCount % 7 == 0
                ? scheduleContext.DayCount / 7
                : (scheduleContext.DayCount / 7) + 1;
            scheduleContext.WeekCount = weekCount;
            foreach (var person in scheduleContext.Persons.Values)
            {
                var varlist = new Dictionary<int, Variable>();
                for (var i = 0; i < weekCount; i++)
                {
                    var variable =
                        scheduleContext.Solver.MakeIntVar(0.0, 1.0,
                            $"weekendwork for person: {person.ID}, week: {i + 1}");
                    varlist.Add(i, variable);
                }

                scheduleContext.WeekEndVariables.Add(person.ID, varlist);
            }
        }


        private static void MapContracts(IEnumerable<SchedulingPeriodContract> schedulingPeriodContracts)
        {
            foreach (var contract in schedulingPeriodContracts)
            {
                var maxSeq = SchedulingMaxSeq.Empty;
                var minSeq = ImmutableList<SchedulingMinSeq>.Empty;
                if (contract.MaxSeq != null)
                {
                    maxSeq = new SchedulingMaxSeq
                    {
                        Shift = scheduleContext.ShiftTypeDicitonary[contract.MaxSeq.shift],
                        MaxValue = Convert.ToInt32(contract.MaxSeq.value)
                    };
                }

                if (contract.MinSeq != null)
                {
                    minSeq = contract.MinSeq?
                            .Select(minseq => new SchedulingMinSeq()
                            {
                                Shift = scheduleContext.ShiftTypeDicitonary[minseq.shift],
                                MinValue = Convert.ToInt32(minseq.value)
                            })
                            .ToImmutableList()
                        ;
                }

                int? minWork = null;
                int? maxWork = null;
                if (contract.Workload != null)
                {
                    minWork = (int)contract.Workload?.Min(wl => wl.Min).Count;
                    maxWork = (int)contract.Workload?.Max(wl => wl.Max).Count;
                }

                string[] validIDs = {""};
                if (contract.ValidShifts != null)
                {
                    validIDs = contract.ValidShifts.shift.Split(",").Where(cid => cid.Length > 0).ToArray();
                }

                scheduleContext.ContractDictionary.Add(contract.ID, new SchedulingContract
                {
                    MaxSeqs = maxSeq,
                    MinSeqs = minSeq,
                    MinWork = minWork,
                    MaxWork = maxWork,
                    ValidShiftIDs = validIDs
                });
            }
        }


        private static void MapFixedAssignments(IEnumerable<SchedulingPeriodEmployee1> schedulingPeriodFixedAssignments)
        {
            foreach (var fixedAssignment in schedulingPeriodFixedAssignments)
            {
                var day = fixedAssignment.Assign.Day;
                if (fixedAssignment.Assign.Shift.Equals(SchedulingGlobalConstants.FreeDayShiftId))
                {
                    var ff = new FixedFreeDay()
                    {
                        Day = day,
                    };
                    scheduleContext.Persons[fixedAssignment.EmployeeID].FixedFreeDays.Add(ff);
                }
                else
                {
                    var fa = new FixedAssaignment()
                    {
                        Type = scheduleContext.ShiftTypeDicitonary[fixedAssignment.Assign.Shift],
                        Day = day
                    };
                    scheduleContext.Persons[fixedAssignment.EmployeeID].FixedAssignments.Add(fa);
                }
            }
        }

        private static void MapShiftOffRequests(IEnumerable<SchedulingPeriodShiftOff> schedulingPeriodShiftOffRequests)
        {
            foreach (var shiftOffRequest in schedulingPeriodShiftOffRequests)
            {
                var day = shiftOffRequest.Day;
                var offRequest = new ShiftOffRequest()
                {
                    Day = shiftOffRequest.Day,
                    Type = scheduleContext.ShiftTypeDicitonary[shiftOffRequest.Shift],
                    Weight = Convert.ToInt32(shiftOffRequest.weight)
                };

                scheduleContext.Persons[shiftOffRequest.EmployeeID].ShiftOffRequests.Add(day, offRequest);
            }
        }

        private static void MapShiftTypes(IEnumerable<SchedulingPeriodShift> schedulingPeriodShiftTypes)
        {
            var shiftTypeCounter = 1;
            scheduleContext.ShiftTypeDicitonary.Add(SchedulingGlobalConstants.AllShiftId, new ShiftType()
            {
                Index = ++shiftTypeCounter,
                ID = SchedulingGlobalConstants.AllShiftId,
                Color = default,
                StartTime = new Time(),
            });
            scheduleContext.ShiftTypeDicitonary.Add(SchedulingGlobalConstants.FreeDayShiftId, new ShiftType()
            {
                Index = ++shiftTypeCounter,
                ID = SchedulingGlobalConstants.FreeDayShiftId,
                Color = default,
                StartTime = new Time(),
            });
            foreach (var shiftType in schedulingPeriodShiftTypes)
            {
                var type = new ShiftType()
                {
                    Index = ++shiftTypeCounter,
                    ID = shiftType.ID,
                    Color = shiftType.Color,
                    StartTime = CreateDateStartTime(shiftType.StartTime),
                    DurationInMnutes = shiftType.Duration
                };
                scheduleContext.ShiftTypeDicitonary.Add(shiftType.ID, type);
            }
        }

        private static Time CreateDateStartTime(string shiftTypeStartTime)
        {
            var components = shiftTypeStartTime.Split(":");
            var time = new Time()
            {
                Hour = Convert.ToInt32(components[0]),
                Minute = Convert.ToInt32(components[1])
            };
            return time;
        }


        private static void MapShiftOnRequests(IEnumerable<SchedulingPeriodShiftOn> schedulingPeriodShiftOnRequests)
        {
            foreach (var shiftOnRequest in schedulingPeriodShiftOnRequests)
            {
                var day = shiftOnRequest.Day;
                var onRequest = new ShiftOnRequest()
                {
                    Day = shiftOnRequest.Day,
                    Type = scheduleContext.ShiftTypeDicitonary[shiftOnRequest.Shift],
                    ShiftOnRrequestVariable = scheduleContext.Solver.MakeIntVar(0.0, 1.0,
                        $"ShiftOnRequeest person: {shiftOnRequest.EmployeeID}, day: {shiftOnRequest.Day}, shift: {shiftOnRequest.Shift}"),
                    Weight = Convert.ToInt32(shiftOnRequest.weight)
                };
                scheduleContext.Persons[shiftOnRequest.EmployeeID].ShiftOnRequests.Add(day, onRequest);
            }
        }

        private static void MapShifts(IEnumerable<SchedulingPeriodDateSpecificCover> schedulingPeriodDateSpecificCovers)
        {
            var shiftCount = 0;
            foreach (var datespecificCover in schedulingPeriodDateSpecificCovers)
            {
                foreach (var cover in datespecificCover.Cover)
                {
                    var shift = new Shift()
                    {
                        Index = shiftCount++,
                        Day = datespecificCover.Day,
                        Name = cover.Shift + "(" +
                               datespecificCover.Day + ": " +
                               cover.Min + " - " +
                               cover.Max + ")",
                        Type = scheduleContext.ShiftTypeDicitonary[cover.Shift],
                        Max = Convert.ToInt32(cover.Max.Value),
                        Min = Convert.ToInt32(cover.Min.Value),
                        MaxWeight = Convert.ToInt32(cover.Max.weight),
                        MinWeight = Convert.ToInt32(cover.Min.weight),
                        OverMax = scheduleContext.Solver.MakeIntVar(0.0,
                            scheduleContext.PersonCount - Convert.ToInt32(cover.Max.Value),
                            $"OverMax on shift: {cover.Shift}, day: {datespecificCover.Day}"),
                        UnderMin = scheduleContext.Solver.MakeIntVar(0.0, Convert.ToInt32(cover.Min.Value),
                            $"UnderMin on shift: {cover.Shift}, day: {datespecificCover.Day}"),
                    };
                    scheduleContext.Shifts.Add(shift);
                }
            }
        }

        private static void MapPersons(IReadOnlyCollection<SchedulingPeriodEmployee> schedulingPeriodEmployees)
        {
            var employeeCount = 0;
            foreach (var employee in schedulingPeriodEmployees)
            {
                var person = new Person
                {
                    Name = $"{employee.ID}",
                    Index = ++employeeCount,
                    ID = employee.ID,
                    ContractIDs = employee.ContractID.ToList()
                };
                scheduleContext.Persons.Add(person.ID, person);
            }

            scheduleContext.PersonCount = schedulingPeriodEmployees.Count;
        }
    }
}

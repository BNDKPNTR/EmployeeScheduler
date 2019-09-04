using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net.Sockets;
using System.Reflection.Metadata.Ecma335;
using Google.OrTools.LinearSolver;
using IPScheduler.Models;
using IPScheduler.Inputs;

namespace IPScheduler.Common
{
    public class IpProblemMapper
    {
        private const string AllShiftId = "$";
        private const string FreeDayShiftId = "-";

        private readonly SchedulingIpContext scheduleContext = new SchedulingIpContext();

        public SchedulingIpContext MapToSolver(SchedulingPeriod schedulingPeriod)
        {
            Map(schedulingPeriod);

            CreateConstraints();
            return scheduleContext;
        }

        private void CreateConstraints()
        {
            CreateAssignmentGraph();

            OnlyOnePersonForAnAssignmentCOnstraint();
            OneAssignmentPerDayConstraint();

            FixedFreeDayConstraint();
            FixedAssaignmentConstraint();

            ShiftOnRequestConstraints();
            ShiftOffRequestConstraints();

            CoverRequirementsConstraints();

        }

        private void CoverRequirementsConstraints()
        {
            foreach (var shift in scheduleContext.Shifts.Values)
            {
                var potentialAssaignmentVariables = scheduleContext.Assignments
                    .Where(a => a.Shift.Index == shift.Index)
                    .Select(a => a.assigningGraphEdge);
                scheduleContext.Solver.MakeConstraint();
                foreach (var variable in potentialAssaignmentVariables)
                {
                    
                }
            }
        }

        private void ShiftOffRequestConstraints()
        {
            foreach (var person in scheduleContext.Persons.Values)
            {
                foreach (var (day, shiftOffRequest) in person.ShiftOffRequests)
                {
                    var constraint = scheduleContext.Solver.MakeConstraint(1.0, 1.0,
                        $"ShiftOnRequestConstraint {person.ID}, day: {day}, shift: {shiftOffRequest.Type}");
                    var potentialPersonAssaignments = scheduleContext.Assignments.Where(a =>
                            a.Person.ID.Equals(person.ID) && a.Shift.Day == day &&
                            a.Shift.Type.ID.Equals(shiftOffRequest.Type.ID))
                        .Select(a => a.assigningGraphEdge);
                    foreach (var variable in potentialPersonAssaignments)
                    {
                        constraint.SetCoefficient(variable, 1.0);
                    }
                    constraint.SetCoefficient(shiftOffRequest.ShiftOffRrequestVariable, 1.0);

                    //Objectivebe is mappelni

                }
            }
        }

        private void ShiftOnRequestConstraints()
        {
            foreach (var person in scheduleContext.Persons.Values)
            {
                foreach (var (day, shiftOnRequest) in person.ShiftOnRequests)
                {
                    var constraint = scheduleContext.Solver.MakeConstraint(1.0, 1.0,
                        $"ShiftOnRequestConstraint {person.ID}, day: {day}, shift: {shiftOnRequest.Type}");
                    var potentialPersonAssaignments = scheduleContext.Assignments.Where(a =>
                        a.Person.ID.Equals(person.ID) && a.Shift.Day == day &&
                        a.Shift.Type.ID.Equals(shiftOnRequest.Type.ID))
                        .Select(a => a.assigningGraphEdge);
                    foreach (var variable in potentialPersonAssaignments)
                    {
                        constraint.SetCoefficient(variable,1.0);
                    }
                    constraint.SetCoefficient(shiftOnRequest.ShiftOnRrequestVariable,1.0);

                    //Objectivebe is mappelni

                }   
            }

        
        }

        private void FixedAssaignmentConstraint()
        {
            foreach (var person in scheduleContext.Persons.Values)
            {
                foreach (var fixedAssaignment in person.FixedAssignments)
                {
                    var perosnsAssaignmentThatDay = scheduleContext.Assignments
                        .Where(a => a.Person.Index == person.Index && a.Shift.Day == fixedAssaignment.Day  && a.Shift.Type.ID == fixedAssaignment.Type.ID)
                        .Select(a => a.assigningGraphEdge);
                    var constraint = scheduleContext.Solver.MakeConstraint(1.0, 1.0, $"FixedAssaignmentConstraint person: {person.ID}, day: {fixedAssaignment.Day}, shift: {fixedAssaignment.Type.ID}");
                    foreach (var variable
                        in perosnsAssaignmentThatDay)
                    {
                        constraint.SetCoefficient(variable, 1.0);
                    }
                }
            }
        }

        private void FixedFreeDayConstraint()
        {
            foreach (var person in scheduleContext.Persons.Values)
            {
                foreach (var freeDay in person.FixedFreeDays)
                {
                    var perosnsAssaignmentThatDay = scheduleContext.Assignments
                        .Where(a => a.Person.Index == person.Index && a.Shift.Day == freeDay.Day)
                        .Select(a => a.assigningGraphEdge);
                    var constraint = scheduleContext.Solver.MakeConstraint(0.0,0.0,$"FixedFreeDayConstraint person: {person.ID}, day: {freeDay.Day}");
                    foreach (var variable
                        in perosnsAssaignmentThatDay)
                    {
                        constraint.SetCoefficient(variable,1.0);
                    }
                }
            }
        }

        private void OneAssignmentPerDayConstraint()
        {
            for (var day = 0; day < scheduleContext.DayCount; day++)
            {
                foreach (var person in scheduleContext.Persons.Values)
                {
                    var day1 = day;
                    var perosnPotentialAssignmentsThatDay =
                        scheduleContext.Assignments.Where(a => a.Person.ID.Equals(person.ID) && a.Shift.Day == day1).Select(a => a.assigningGraphEdge);

                   var constraint = scheduleContext.Solver.MakeConstraint(0.0, 1.0, $"person: {person} only one shift in day: {day1}");
                    foreach (var potentialAssaignment in perosnPotentialAssignmentsThatDay)
                    {
                        constraint.SetCoefficient(potentialAssaignment, 1.0);
                    }
                }


            }
        }

        private void OnlyOnePersonForAnAssignmentCOnstraint()
        {
            foreach (var shift in scheduleContext.Shifts.Values)
            {
                var shiftEmployeePairs = scheduleContext.Assignments
                    .Where(a => a.Shift.Index == shift.Index)
                    .Select(a => a.assigningGraphEdge);



                // Megkötés, hogy egy műszakot csak egy ember vihet
                var constraint = scheduleContext.Solver.MakeConstraint(1.0, 1.0, $"shiftGraphConstraint: {shift.Index}");
                foreach (var shiftEmployeePair in shiftEmployeePairs)
                {
                    constraint.SetCoefficient(shiftEmployeePair, 1);
                }
            }
        }

        private void Map(SchedulingPeriod schedulingPeriod)
        {

            MapShiftTypes(schedulingPeriod.ShiftTypes);
            MapPersons(schedulingPeriod.Employees);
            MapShifts(schedulingPeriod.CoverRequirements);
            MapShiftOnRequests(schedulingPeriod.ShiftOnRequests);
            MapShiftOffRequests(schedulingPeriod.ShiftOffRequests);
            MapFixedAssignments(schedulingPeriod.FixedAssignments);
            MapContracts(schedulingPeriod.Contracts);

            MapExtraInfomraiton();
        }

        private void MapExtraInfomraiton()
        {
            scheduleContext.DayCount = scheduleContext.Shifts.Keys.Max();
        }


        private void MapContracts(IEnumerable<SchedulingPeriodContract> schedulingPeriodContracts)
        {
            foreach (var contract in schedulingPeriodContracts)
            {
                scheduleContext.ContractDictionary.Add(contract.ID, new SchedulingContract
                {
                    MaxSeqs = contract.MaxSeq?
                        .Select(maxseq => new SchedulingMaxSeq
                        {
                            Shift = scheduleContext.ShiftTypeDicitonary[maxseq.shift],
                            MaxValue = Convert.ToInt32(maxseq.value)
                        })
                        .ToImmutableList(),
                    MinSeqs = contract.MinSeq?
                        .Select(minseq => new SchedulingMinSeq()
                        {
                            Shift = scheduleContext.ShiftTypeDicitonary[minseq.shift],
                            MinValue = Convert.ToInt32(minseq.value)
                        })
                        .ToImmutableList()
                });
            }
        }



        private void MapFixedAssignments(IEnumerable<SchedulingPeriodEmployee1> schedulingPeriodFixedAssignments)
        {
            foreach (var fixedAssignment in schedulingPeriodFixedAssignments)
            {
                var day = fixedAssignment.Assign.Day;
                if (fixedAssignment.Assign.Shift.Equals(FreeDayShiftId))
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

        private void MapShiftOffRequests(IEnumerable<SchedulingPeriodShiftOff> schedulingPeriodShiftOffRequests)
        {

            foreach (var shiftOffRequest in schedulingPeriodShiftOffRequests)
            {
                var day = shiftOffRequest.Day;
                var offRequest = new ShiftOffRequest()
                {
                    Day = shiftOffRequest.Day,
                    Type = scheduleContext.ShiftTypeDicitonary[shiftOffRequest.Shift],
                    ShiftOffRrequestVariable = scheduleContext.Solver.MakeIntVar(0.0,1.0, $"ShiftOffRequeest person: {shiftOffRequest.EmployeeID}, day: {shiftOffRequest.Day}")
                };

                scheduleContext.Persons[shiftOffRequest.EmployeeID].ShiftOffRequests.Add(day, offRequest);

            }
        }

        private void MapShiftTypes(IEnumerable<SchedulingPeriodShift> schedulingPeriodShiftTypes)
        {
            var shiftTypeCounter = 1;
            scheduleContext.ShiftTypeDicitonary.Add(AllShiftId, new ShiftType()
            {
                Index = ++shiftTypeCounter,
                ID = "$",
                Color = default,
                StartTime = new Time(),

            });
            scheduleContext.ShiftTypeDicitonary.Add(FreeDayShiftId, new ShiftType()
            {
                Index = ++shiftTypeCounter,
                ID = "$",
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


        private void MapShiftOnRequests(IEnumerable<SchedulingPeriodShiftOn> schedulingPeriodShiftOnRequests)
        {
            foreach (var shiftOnRequest in schedulingPeriodShiftOnRequests)
            {
                var day = shiftOnRequest.Day;
                var onRequest = new ShiftOnRequest()
                {
                    Day = shiftOnRequest.Day,
                    Type = scheduleContext.ShiftTypeDicitonary[shiftOnRequest.Shift],
                    ShiftOnRrequestVariable = scheduleContext.Solver.MakeIntVar(0.0, 1.0, $"ShiftOnRequeest person: {shiftOnRequest.EmployeeID}, day: {shiftOnRequest.Day}, shift: {shiftOnRequest.Shift}")
                };
                scheduleContext.Persons[shiftOnRequest.EmployeeID].ShiftOnRequests.Add(day, onRequest);
            }
        }

        private void MapShifts(IEnumerable<SchedulingPeriodDateSpecificCover> schedulingPeriodDateSpecificCovers)
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
                        OverMax = scheduleContext.Solver.MakeIntVar(0.0, scheduleContext.PersonCount - Convert.ToInt32(cover.Max.Value), $"OverMax on shift: {cover.Shift}, day: {datespecificCover.Day}"),
                        UnderMin = scheduleContext.Solver.MakeIntVar(0.0, Convert.ToInt32(cover.Min.Value), $"UnderMin on shift: {cover.Shift}, day: {datespecificCover.Day}"),
                    };
                    scheduleContext.Shifts.Add(shift.Day, shift);
                }
            }
        }

        private void MapPersons(IReadOnlyCollection<SchedulingPeriodEmployee> schedulingPeriodEmployees)
        {
            var employeeCount = 0;
            foreach (var employee in schedulingPeriodEmployees)
            {
                var person = new Person
                {
                    Name = $"{employee.ID}",
                    Index = ++employeeCount,
                    ID = employee.ID
                };
                scheduleContext.Persons.Add(person.ID, person);
            }

            scheduleContext.PersonCount = schedulingPeriodEmployees.Count;
        }

        private void CreateAssignmentGraph()
        {
            var graphEdges = 0;
            var graphStarts = scheduleContext.Solver.NumConstraints();
            foreach (var shift in scheduleContext.Shifts.Values)
            {
                var shiftEmployeePairs = new List<Variable>();
                foreach (var employee in scheduleContext.Persons.Values)
                {
                    // Változó egy összerendelési élre
                    var v = scheduleContext.Solver.MakeIntVar(0.0, 1.0, $"{employee.ID}-{shift.Index}");
                    //  scheduleContext.Variables.Add(v);
                    var assignment = new Assignment()
                    {
                        Index = graphEdges,
                        assigningGraphEdge = v,
                        Person = employee,
                        Shift = shift
                    };
                    scheduleContext.Assignments.Add(assignment);
                    shiftEmployeePairs.Add(v);
                    graphEdges++;
                }

                // Megkötés, hogy egy műszako csak egy ember vihet
                var constraint = scheduleContext.Solver.MakeConstraint(1.0, 1.0, $"shiftGraphConstraint: {shift.Index}");
                foreach (var shiftEmployeePair in shiftEmployeePairs)
                {
                    constraint.SetCoefficient(shiftEmployeePair, 1);
                }
            }

            scheduleContext.GraphStartsAt = graphStarts;
            scheduleContext.GraphEdges = graphEdges;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
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

            MapObjective();
            return scheduleContext;
        }

        private void MapObjective()
        {
            var objective = scheduleContext.Solver.Objective();

            SetObjectiveForShiftOnRequests(objective);
            SetObjectiveForShiftOffRequests(objective);
            SetObjectiveForCoverRequirements(objective);
            objective.SetMinimization();
        }

        private void SetObjectiveForCoverRequirements(Objective objective)
        {
            foreach (var assignment in scheduleContext.Assignments)
            {
                objective.SetCoefficient(assignment.Shift.OverMax,assignment.Shift.MaxWeight);
                objective.SetCoefficient(assignment.Shift.UnderMin,assignment.Shift.MinWeight);
            }
        }

        private void SetObjectiveForShiftOffRequests(Objective objective)
        {
            foreach (var shiftOffRequest in scheduleContext.Persons.Values.SelectMany(person => person.ShiftOffRequests.Values))
            {
                objective.SetCoefficient(shiftOffRequest.ShiftOffRrequestVariable, shiftOffRequest.Weight);
            }
        }

        private void SetObjectiveForShiftOnRequests(Objective objective)
        {
            foreach (var shiftOnRequest in scheduleContext.Persons.Values.SelectMany(person => person.ShiftOnRequests.Values))
            {
                objective.SetCoefficient(shiftOnRequest.ShiftOnRrequestVariable, shiftOnRequest.Weight);
            }
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

            SequenceConstraints();


            OnlyOneWeekendConstraints();

        }

        private void SequenceConstraints()
        {
            foreach (var person in scheduleContext.Persons.Values)
            {
                var assaignments = scheduleContext.Assignments.Where(a => a.Person.ID == person.ID);
                //int personMinValue = person.ContractID.Max(c => c.MinSeqs.Max(ms => ms.MinValue));
                //int personMaxValue = person.Contracts.Min(c => c.MaxSeqs.Min(ms => ms.MaxValue));
                //var constraint = scheduleContext.Solver.MakeConstraint(personMinValue,personMaxValue
                //    ,$"Person : {person.ID} needs to work min: {personMinValue}, but can only work max: {personMaxValue}");
                //foreach (var assaignment in assaignments.Where(a => a.Person.ID.Equals(person.ID)))
                //{
                //    constraint.SetCoefficient(assaignment.assigningGraphEdge,assaignment.Shift.Type.DurationInMnutes);
                //}


            }
        }

        private void OnlyOneWeekendConstraints()
        {
            foreach (var person in scheduleContext.Persons.Values)
            {
                var perosnsAssaignments = scheduleContext.Assignments.Where(a => a.Person.ID.Equals(person.ID));
                var sundayAssaignments = perosnsAssaignments.Where(a => a.Shift.Day % 7 == 6);
                var saturdayAssaignments = perosnsAssaignments.Where(a => a.Shift.Day % 7 == 5);
                var saturdayDays = saturdayAssaignments.Select(a => a.Shift.Day).Distinct();
                var sundayDays = sundayAssaignments.Select(a => a.Shift.Day).Distinct();



                var constraint = scheduleContext.Solver.MakeConstraint(0.0,1.0, $"OnlyOneWeekendConstraint person: {person.ID}");

                foreach (var saturday in saturdayDays)
                {
                    var thatSaturdayDayAssaignments = saturdayAssaignments.Where(a => a.Shift.Day == saturday);
                    var thatSundayDayAssaignments = sundayAssaignments.Where(a => a.Shift.Day == saturday + 1);
                    // TODO : only one weekend on scheduling period


                    constraint.SetCoefficient(scheduleContext.WeekEndVariables[person.ID][saturday / 7], 1.0);
                }
            }
        }

        private void CoverRequirementsConstraints()
        {
            foreach (var (day, shift) in scheduleContext.Shifts)
            {
                var maxConstraint = scheduleContext.Solver.MakeConstraint(0.0, shift.Max, $"CoverRequirementMaxConstraint shift: {shift.Type.ID}, day: {day}, max: {shift.Max}");
                var minConstraint = scheduleContext.Solver.MakeConstraint(shift.Min, scheduleContext.PersonCount, $"CoverRequirementMinConstraint shift: {shift.Type.ID}, day: {day}, min: {shift.Min}");
                foreach (var assignment in scheduleContext.Assignments.Where(a => a.Shift.Index == shift.Index))
                {
                    maxConstraint.SetCoefficient(assignment.assigningGraphEdge,1.0);
                    minConstraint.SetCoefficient(assignment.assigningGraphEdge,1.0);
                }
                maxConstraint.SetCoefficient(shift.OverMax,-1.0);
                minConstraint.SetCoefficient(shift.UnderMin,1.0);
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
            MapContracts(schedulingPeriod.Contracts);
            MapShifts(schedulingPeriod.CoverRequirements);
            MapShiftOnRequests(schedulingPeriod.ShiftOnRequests);
            MapShiftOffRequests(schedulingPeriod.ShiftOffRequests);
            MapFixedAssignments(schedulingPeriod.FixedAssignments);

            MapExtraInfomraiton();
        }

        private void MapExtraInfomraiton()
        {
            scheduleContext.DayCount = scheduleContext.Shifts.Keys.Max();
            var weekCount = scheduleContext.DayCount % 7 == 0 ? scheduleContext.DayCount / 7 : (scheduleContext.DayCount / 7) + 1;
            scheduleContext.WeekCount = weekCount;
            foreach (var person in scheduleContext.Persons.Values)
            {
                var varlist = new Dictionary<int, Variable>();
                for (var i = 0; i < weekCount; i++)
                {
                    var variable =
                        scheduleContext.Solver.MakeIntVar(0.0, 1.0, $"weekendwork for person: {person.ID}, week: {i+1}");
                    varlist.Add(i,variable);
                }
                scheduleContext.WeekEndVariables.Add(person.ID, varlist);
            }
        }


        private void MapContracts(IEnumerable<SchedulingPeriodContract> schedulingPeriodContracts)
        {
            foreach (var contract in schedulingPeriodContracts)
            {
                var maxSeq = contract.MaxSeq?
                    .Select(maxseq => new SchedulingMaxSeq
                    {
                        Shift = scheduleContext.ShiftTypeDicitonary[maxseq.shift],
                        MaxValue = Convert.ToInt32(maxseq.value)
                    })
                    .ToImmutableList()
                    ;
                var minSeq = contract.MinSeq?
                    .Select(minseq => new SchedulingMinSeq()
                    {
                        Shift = scheduleContext.ShiftTypeDicitonary[minseq.shift],
                        MinValue = Convert.ToInt32(minseq.value)
                    })
                    .ToImmutableList()
                    ;
               
                scheduleContext.ContractDictionary.Add(contract.ID, new SchedulingContract
                {

                    MaxSeqs = maxSeq,
                    MinSeqs = minSeq
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
                    ShiftOffRrequestVariable = scheduleContext.Solver.MakeIntVar(0.0,1.0, $"ShiftOffRequeest person: {shiftOffRequest.EmployeeID}, day: {shiftOffRequest.Day}"),
                    Weight = Convert.ToInt32(shiftOffRequest.weight)
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
                ID = "-",
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
                    ShiftOnRrequestVariable = scheduleContext.Solver.MakeIntVar(0.0, 1.0, $"ShiftOnRequeest person: {shiftOnRequest.EmployeeID}, day: {shiftOnRequest.Day}, shift: {shiftOnRequest.Shift}"),
                    Weight = Convert.ToInt32(shiftOnRequest.weight)
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
                    ID = employee.ID,
                    ContractIDs = employee.ContractID.ToList()
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
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
                objective.SetCoefficient(assignment.Shift.OverMax, assignment.Shift.MaxWeight);
                objective.SetCoefficient(assignment.Shift.UnderMin, assignment.Shift.MinWeight);
            }
        }

        private void SetObjectiveForShiftOffRequests(Objective objective)
        {
            foreach (var shiftOffRequest in scheduleContext.Persons.Values.SelectMany(person =>
                person.ShiftOffRequests.Values))
            {
                objective.SetCoefficient(shiftOffRequest.ShiftOffRrequestVariable, shiftOffRequest.Weight);
            }
        }

        private void SetObjectiveForShiftOnRequests(Objective objective)
        {
            foreach (var shiftOnRequest in scheduleContext.Persons.Values.SelectMany(person =>
                person.ShiftOnRequests.Values))
            {
                objective.SetCoefficient(shiftOnRequest.ShiftOnRrequestVariable, shiftOnRequest.Weight);
            }
        }

        private void CreateConstraints()
        {
            CreateAssignmentGraph();

           // OnlyOnePersonForAnAssignmentCOnstraint();
            OneAssignmentPerDayConstraint();

            FixedFreeDayConstraint();
            FixedAssaignmentConstraint();

            ShiftOnRequestConstraints();
            ShiftOffRequestConstraints();

            CoverRequirementsConstraints();

            SequenceConstraints();
            WorkLoadConstraints();


            OnlyOneWeekendConstraints();
        }

        private void WorkLoadConstraints()
        {
            Console.WriteLine("berfore: " + scheduleContext.Solver.NumConstraints());


            //foreach (var (cid, contract) in scheduleContext.ContractDictionary)
            //{
            //    foreach (var person in scheduleContext.Persons.Values)
            //    {

            //        if (scheduleContext.ContractDictionary[cid].MinWork.HasValue
            //            && scheduleContext.ContractDictionary[cid].MaxWork.HasValue
            //            && person.ContractIDs.Contains(cid))
            //        {
            //            var min = scheduleContext.ContractDictionary[cid].MinWork.Value;
            //            var max = scheduleContext.ContractDictionary[cid].MaxWork.Value;
            //            var constraint = scheduleContext.Solver.MakeConstraint(0.0, 0.0, $"workload constraint for person: {person.ID}");
            //            var personassignments = scheduleContext.Assignments.Where(a => a.Person.ID == person.ID);
            //            var personworkedVariable = scheduleContext.Solver.MakeIntVar(0.0, personassignments.Count(), $"perons {person.ID} assignemntcount");
            //            constraint.SetCoefficient(personworkedVariable, -1);
            //            var contractlength = personassignments.First().Shift.Type.DurationInMnutes;
            //            foreach (var assignment in personassignments)
            //            {
            //                constraint.SetCoefficient(assignment.assigningGraphEdge,1.0);
            //            }
            //            scheduleContext.Solver.Add(contractlength * personworkedVariable >= min);
            //            scheduleContext.Solver.Add(contractlength * personworkedVariable <= max);

            //        }
            //    }
            //}




                foreach (var person in scheduleContext.Persons.Values)
                {


                var contract = scheduleContext.ContractDictionary.Where(cid => person.ContractIDs.Contains(cid.Key) && cid.Value.MinWork.HasValue).Select(c => c.Value).Single();
                var min = contract.MinWork.Value / 480;
                var max = contract.MaxWork.Value / 480;
                
                        var constraint = scheduleContext.Solver.MakeConstraint(min, max, $"workload constraint for person: {person.ID}");
                var assignments = scheduleContext.Assignments.Where(a => a.Person.ID.Equals(person.ID));
                foreach (var assignment in assignments)
                {
                    constraint.SetCoefficient(assignment.assigningGraphEdge, 1.0);
                }
                //var personassignments = scheduleContext.Assignments.Where(a => a.Person.ID == person.ID);
                        //var personworkedVariable = scheduleContext.Solver.MakeIntVar(0.0, personassignments.Count(), $"perons {person.ID} assignemntcount");
                        //constraint.SetCoefficient(personworkedVariable, -1);
                        //var contractlength = personassignments.First().Shift.Type.DurationInMnutes;
                        //foreach (var assignment in personassignments)
                        //{
                        //    constraint.SetCoefficient(assignment.assigningGraphEdge, 1.0);
                        //}
                        //scheduleContext.Solver.Add(contractlength * personworkedVariable >= min);
                        //scheduleContext.Solver.Add(contractlength * personworkedVariable <= max);

                }








































            Console.WriteLine("after: " + scheduleContext.Solver.NumConstraints());
        }

        private void SequenceConstraints()
        {
            foreach (var person in scheduleContext.Persons.Values)
            {
                foreach (var contractID in person.ContractIDs)
                {
                    // if contranct contains minseq
                    if (!scheduleContext.ContractDictionary[contractID].MinSeqs.IsEmpty)
                    {
                        foreach (var minSeq in scheduleContext.ContractDictionary[contractID].MinSeqs)
                        {
                            // if a minseq is valid for general shift
                            if (minSeq.Shift.ID.Equals(AllShiftId))
                            {
                                CreateMinSeqConstraint(person, minSeq.MinValue,
                                    scheduleContext.Assignments.Where(a => a.Person.ID.Equals(person.ID)));
                            }

                            // if a minseq refers to freedays
                            if (minSeq.Shift.ID.Equals(FreeDayShiftId))
                            {
                                CreateMinFreeSeqConstraint(person, minSeq.MinValue,
                                    scheduleContext.Assignments.Where(a => a.Person.ID.Equals(person.ID)));
                            }
                        }
                    }
                    if (!scheduleContext.ContractDictionary[contractID].MaxSeqs.IsEmpty)
                    {

                        // if a maxseq is valid for general shift
                        if (scheduleContext.ContractDictionary[contractID].MaxSeqs.Shift.ID.Equals(AllShiftId))
                        {
                            CreateMaxSeqConstraint(person, scheduleContext.ContractDictionary[contractID].MaxSeqs.MaxValue,
                                scheduleContext.Assignments.Where(a => a.Person.ID.Equals(person.ID)));
                        }


                    }
                }
            }
        }

        private void CreateMaxSeqConstraint(Person person, int maxSeqMaxValue, IEnumerable<Assignment> assaignmentCollection)
        {
            var assignments = assaignmentCollection as Assignment[] ?? assaignmentCollection.ToArray();
            var days = assignments.Select(a => a.Shift.Day).Max();

            if (days < maxSeqMaxValue)
                return;

            for (var i = 0; i <= days - maxSeqMaxValue; i++)
            {
                var currentAssaignments = assignments.Where(a => a.Shift.Day >= i && a.Shift.Day <= i + maxSeqMaxValue).ToList();
                var constraint = scheduleContext.Solver.MakeConstraint(0.0, maxSeqMaxValue,
                    $"MaxSeqconstraint for person: {person.ID}, during days: {i}-{i + maxSeqMaxValue}");
                AddCoefficients(currentAssaignments, constraint);
            }
        }

        private void CreateMinFreeSeqConstraint(Person person, int minSeqMinValue, IEnumerable<Assignment> assaignments)
        {
            var assignments = assaignments as Assignment[] ?? assaignments.ToArray();
            var days = assignments.Select(a => a.Shift.Day).Max();

            if (days < minSeqMinValue)
                throw new ArgumentException("There are not enough days in Scdeuling period for this MinSequence");

            var beforeDayVariable =
                scheduleContext.Solver.MakeIntVar(0.0, 0.0, $"-1st day variable for perosn: {person.ID}");
            var firstDayAssignments = assignments.Where(a => a.Shift.Day == 0).ToList();
            var secondDayAssignments = assignments.Where(a => a.Shift.Day == 1).ToList();
            var startConstraint = scheduleContext.Solver.MakeConstraint(-1, 1,
                $"MinSeqconstraint for person: {person.ID}, during days: {-1}-{2}");
            startConstraint.SetCoefficient(beforeDayVariable, 1.0);
            AddCoefficients(firstDayAssignments, startConstraint, -1.0);
            AddCoefficients(secondDayAssignments, startConstraint, 1.0);

            var lastDayAssignments = assignments.Where(a => a.Shift.Day == days).ToList();
            var secondToLastDayAssignments = assignments.Where(a => a.Shift.Day == days - 1).ToList();
            var afterDayVariable =
                scheduleContext.Solver.MakeIntVar(0.0, 0.0, $"+1 day variable for perosn: {person.ID}");
            var endConstraint = scheduleContext.Solver.MakeConstraint(-1, 1,
                $"MinSeqconstraint for person: {person.ID}, during days: {days - 1}-{days + 1}");
            AddCoefficients(secondToLastDayAssignments, endConstraint, 1.0);
            AddCoefficients(lastDayAssignments, endConstraint, -1.0);
            endConstraint.SetCoefficient(afterDayVariable, 1.0);

            for (var i = 1; i <= days - minSeqMinValue - 1; i++)
            {
                var currentAssaignments = assignments.Where(a => a.Shift.Day == i).ToList();
                var nextDayAssaignments = assignments.Where(a => a.Shift.Day == i + 1).ToList();
                var dayAfterNextDayAssaignments = assignments.Where(a => a.Shift.Day == i + 2).ToList();
                var constraint = scheduleContext.Solver.MakeConstraint(-1.0, 1.0,
                    $"MinSeqconstraint for person: {person.ID}, during days: {i}-{i + 2}");
                AddCoefficients(currentAssaignments, constraint, 1.0);
                AddCoefficients(nextDayAssaignments, constraint, -1.0);
                AddCoefficients(dayAfterNextDayAssaignments, constraint, 1.0);
            }
        }

        private static void AddCoefficients(IEnumerable<Assignment> assaignmentList, Constraint constraint,
            double coefficient = 1.0)
        {
            foreach (var assaignment in assaignmentList)
            {
                constraint.SetCoefficient(assaignment.assigningGraphEdge, coefficient);
            }
        }

        private void CreateMinSeqConstraint(Person person, int minSeqMinValue, IEnumerable<Assignment> assaignments)
        {
            var assignments = assaignments as Assignment[] ?? assaignments.ToArray();
            var days = assignments.Select(a => a.Shift.Day).Max();

            if (days < minSeqMinValue)
                throw new ArgumentException("There are not enough days in Scdeuling period for this MinSequence");

            var beforeDayVariable =
                scheduleContext.Solver.MakeIntVar(0.0, 0.0, $"-1st day variable for perosn: {person.ID}");
            var firstDayAssignments = assignments.Where(a => a.Shift.Day == 0).ToList();
            var secondDayAssignments = assignments.Where(a => a.Shift.Day == 1).ToList();
            var startConstraint = scheduleContext.Solver.MakeConstraint(-2, 0,
                $"MinSeqconstraint for person: {person.ID}, during days: {-1}-{2}");
            startConstraint.SetCoefficient(beforeDayVariable, -1.0);
            AddCoefficients(firstDayAssignments, startConstraint);
            AddCoefficients(secondDayAssignments, startConstraint, -1.0);


            var lastDayAssignments = assignments.Where(a => a.Shift.Day == days).ToList();
            var secondToLastDayAssignments = assignments.Where(a => a.Shift.Day == days - 1).ToList();
            var afterDayVariable =
                scheduleContext.Solver.MakeIntVar(0.0, 0.0, $"+1 day variable for perosn: {person.ID}");
            var endConstraint = scheduleContext.Solver.MakeConstraint(-2, 0,
                $"MinSeqconstraint for person: {person.ID}, during days: {days - 1}-{days + 1}");
            AddCoefficients(secondToLastDayAssignments, endConstraint, -1.0);
            AddCoefficients(lastDayAssignments, endConstraint);
            endConstraint.SetCoefficient(afterDayVariable, -1);

            for (var i = 1; i <= days - minSeqMinValue - 1; i++)
            {
                var currentAssaignments = assignments.Where(a => a.Shift.Day == i).ToList();
                var nextDayAssaignments = assignments.Where(a => a.Shift.Day == i + 1).ToList();
                var dayAfterNextDayAssaignments = assignments.Where(a => a.Shift.Day == i + 2).ToList();
                var constraint = scheduleContext.Solver.MakeConstraint(-2, 0,
                    $"MinSeqconstraint for person: {person.ID}, during days: {i}-{i + 2}");
                AddCoefficients(currentAssaignments, constraint, -1.0);
                AddCoefficients(nextDayAssaignments, constraint);
                AddCoefficients(dayAfterNextDayAssaignments, constraint, -1.0);
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


                var constraint =
                    scheduleContext.Solver.MakeConstraint(0.0, 1.0, $"OnlyOneWeekendConstraint person: {person.ID}");
                var perosnWeekendVariables = new List<Variable>();
                foreach (var saturday in saturdayDays)
                {
                    var thisWeekendConstraint = scheduleContext.Solver.MakeConstraint(0.0, 1.5,
                        $"WeekendConstraint for person: {person.ID}, on week: {(saturday + 1) / 7}");
                    var weekendVariable = scheduleContext.Solver.MakeIntVar(0.0, 1.0,
                        $"Variable for person: {person.ID}, on week: {(saturday + 1) / 7}");
                    perosnWeekendVariables.Add(weekendVariable);
                    var thatSaturdayDayAssaignments = saturdayAssaignments.Where(a => a.Shift.Day == saturday);
                    var thatSundayDayAssaignments = sundayAssaignments.Where(a => a.Shift.Day == saturday + 1);
                    // TODO : only one weekend on scheduling period


                    thisWeekendConstraint.SetCoefficient(weekendVariable, 2.0);
                    AddCoefficients(thatSundayDayAssaignments, thisWeekendConstraint, -0.5);
                    AddCoefficients(thatSaturdayDayAssaignments, thisWeekendConstraint, -0.5);



                    constraint.SetCoefficient(scheduleContext.WeekEndVariables[person.ID][saturday / 7], 1.0);
                }

                foreach (var variable in perosnWeekendVariables)
                {
                    constraint.SetCoefficient(variable, 1.0);
                }
            }
        }

        private void CoverRequirementsConstraints()
        {
            foreach (var shift in scheduleContext.Shifts)
            {
                var maxConstraint = scheduleContext.Solver.MakeConstraint(0.0, shift.Max,
                    $"CoverRequirementMaxConstraint shift: {shift.Type.ID}, day: {shift.Day}, max: {shift.Max}");
                var minConstraint = scheduleContext.Solver.MakeConstraint(shift.Min, scheduleContext.PersonCount,
                    $"CoverRequirementMinConstraint shift: {shift.Type.ID}, day: {shift.Day}, min: {shift.Min}");
                var e = scheduleContext.Assignments.Where(a => a.Shift.Day == shift.Day && a.Shift.Type.ID == shift.Type.ID);

                foreach (var assignment in scheduleContext.Assignments.Where(a =>
                    a.Shift.Day == shift.Day && a.Shift.Type.ID == shift.Type.ID))
                {
                    maxConstraint.SetCoefficient(assignment.assigningGraphEdge, 1.0);
                    minConstraint.SetCoefficient(assignment.assigningGraphEdge, 1.0);
                }

                maxConstraint.SetCoefficient(shift.OverMax, -1.0);
                minConstraint.SetCoefficient(shift.UnderMin, 1.0);
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
                        constraint.SetCoefficient(variable, 1.0);
                    }

                    constraint.SetCoefficient(shiftOnRequest.ShiftOnRrequestVariable, 1.0);
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
                        .Where(a => a.Person.Index == person.Index && a.Shift.Day == fixedAssaignment.Day &&
                                    a.Shift.Type.ID == fixedAssaignment.Type.ID)
                        .Select(a => a.assigningGraphEdge);
                    var constraint = scheduleContext.Solver.MakeConstraint(1.0, 1.0,
                        $"FixedAssaignmentConstraint person: {person.ID}, day: {fixedAssaignment.Day}, shift: {fixedAssaignment.Type.ID}");
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
                    var constraint = scheduleContext.Solver.MakeConstraint(0.0, 0.0,
                        $"FixedFreeDayConstraint person: {person.ID}, day: {freeDay.Day}");
                    foreach (var variable
                        in perosnsAssaignmentThatDay)
                    {
                        constraint.SetCoefficient(variable, 1.0);
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
                        scheduleContext.Assignments.Where(a => a.Person.ID.Equals(person.ID) && a.Shift.Day == day1)
                            .Select(a => a.assigningGraphEdge);

                    var constraint = scheduleContext.Solver.MakeConstraint(0.0, 1.0,
                        $"person: {person} only one shift in day: {day1}");
                    foreach (var potentialAssaignment in perosnPotentialAssignmentsThatDay)
                    {
                        constraint.SetCoefficient(potentialAssaignment, 1.0);
                    }
                }
            }
        }

        private void OnlyOnePersonForAnAssignmentCOnstraint()
        {
            foreach (var shift in scheduleContext.Shifts)
            {
                var shiftEmployeePairs = scheduleContext.Assignments
                    .Where(a => a.Shift.Index == shift.Index)
                    .Select(a => a.assigningGraphEdge);

                // Megkötés, hogy egy műszakot csak egy ember vihet
                var constraint =
                    scheduleContext.Solver.MakeConstraint(1.0, 1.0, $"shiftGraphConstraint: {shift.Index}");
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


        private void MapContracts(IEnumerable<SchedulingPeriodContract> schedulingPeriodContracts)
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

                string[] validIDs = null;
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
                    ShiftOffRrequestVariable = scheduleContext.Solver.MakeIntVar(0.0, 1.0,
                        $"ShiftOffRequeest person: {shiftOffRequest.EmployeeID}, day: {shiftOffRequest.Day}"),
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


        private void MapShiftOnRequests(IEnumerable<SchedulingPeriodShiftOn> schedulingPeriodShiftOnRequests)
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
            foreach (var shift in scheduleContext.Shifts)
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

                //// Megkötés, hogy egy műszako csak egy ember vihet
                //var constraint =
                //    scheduleContext.Solver.MakeConstraint(1.0, 1.0, $"shiftGraphConstraint: {shift.Index}");
                //foreach (var shiftEmployeePair in shiftEmployeePairs)
                //{
                //    constraint.SetCoefficient(shiftEmployeePair, 1);
                //}
            }

            scheduleContext.GraphStartsAt = graphStarts;
            scheduleContext.GraphEdges = graphEdges;
        }
    }
}

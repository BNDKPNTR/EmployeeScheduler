using Google.OrTools.LinearSolver;
using IPScheduler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IPScheduler.Common.Mapper
{
   public static class ConstraintMapper
    {
        public static void CreateConstraints(SchedulingIpContext context)
        {
            CreateAssignmentGraph(context);
            ShiftOnRequestConstraints(context);
            ShiftOffRequestConstraints(context);
            CoverRequirementsConstraints(context);
            SequenceConstraints(context);
            OnlyOneWeekendConstraints(context);
        }
      

        private static void SequenceConstraints(SchedulingIpContext scheduleContext)
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
                            if (minSeq.Shift.ID.Equals(SchedulingGlobalConstants.AllShiftId))
                            {
                                CreateMinSeqConstraint(person, minSeq.MinValue,
                                    scheduleContext.Assignments.Where(a => a.Person.ID.Equals(person.ID)), scheduleContext.Solver);
                            }

                            // if a minseq refers to freedays
                            if (minSeq.Shift.ID.Equals(SchedulingGlobalConstants.FreeDayShiftId))
                            {
                                CreateMinFreeSeqConstraint(person, minSeq.MinValue,
                                    scheduleContext.Assignments.Where(a => a.Person.ID.Equals(person.ID)), scheduleContext.Solver);
                            }
                        }
                    }

                    if (!scheduleContext.ContractDictionary[contractID].MaxSeqs.IsEmpty)
                    {

                        // if a maxseq is valid for general shift
                        if (scheduleContext.ContractDictionary[contractID].MaxSeqs.Shift.ID.Equals(SchedulingGlobalConstants.AllShiftId))
                        {
                            CreateMaxSeqConstraint(person, scheduleContext.ContractDictionary[contractID].MaxSeqs.MaxValue,
                                scheduleContext.Assignments.Where(a => a.Person.ID.Equals(person.ID)), scheduleContext.Solver);
                        }
                    }
                }
            }
        }

        private static void CreateMaxSeqConstraint(Person person, int maxSeqMaxValue, IEnumerable<Assignment> assaignmentCollection, Solver solver)
        {
            var assignments = assaignmentCollection as Assignment[] ?? assaignmentCollection.ToArray();
            var days = assignments.Select(a => a.Shift.Day).Max();

            if (days < maxSeqMaxValue)
                return;

            for (var i = 0; i <= days - maxSeqMaxValue; i++)
            {
                var currentAssaignments = assignments.Where(a => a.Shift.Day >= i && a.Shift.Day <= i + maxSeqMaxValue).ToList();
                var constraint = solver.MakeConstraint(0.0, maxSeqMaxValue,
                    $"MaxSeqconstraint for person: {person.ID}, during days: {i}-{i + maxSeqMaxValue}");
                AddCoefficients(currentAssaignments, constraint);
            }
        }

        private static void CreateMinFreeSeqConstraint(Person person, int minSeqMinValue, IEnumerable<Assignment> assaignments, Solver solver)
        {
            var assignments = assaignments as Assignment[] ?? assaignments.ToArray();
            var days = assignments.Select(a => a.Shift.Day).Max();

            if (days < minSeqMinValue)
                throw new ArgumentException("There are not enough days in Scdeuling period for this MinSequence");

            var beforeDayVariable =
                solver.MakeIntVar(0.0, 0.0, $"-1st day variable for perosn: {person.ID}");
            var firstDayAssignments = assignments.Where(a => a.Shift.Day == 0).ToList();
            var secondDayAssignments = assignments.Where(a => a.Shift.Day == 1).ToList();
            var startConstraint = solver.MakeConstraint(-1, 1,
                $"MinSeqconstraint for person: {person.ID}, during days: {-1}-{2}");
            startConstraint.SetCoefficient(beforeDayVariable, 1.0);
            AddCoefficients(firstDayAssignments, startConstraint, -1.0);
            AddCoefficients(secondDayAssignments, startConstraint, 1.0);

            var lastDayAssignments = assignments.Where(a => a.Shift.Day == days).ToList();
            var secondToLastDayAssignments = assignments.Where(a => a.Shift.Day == days - 1).ToList();
            var afterDayVariable =
                solver.MakeIntVar(0.0, 0.0, $"+1 day variable for perosn: {person.ID}");
            var endConstraint = solver.MakeConstraint(-1, 1,
                $"MinSeqconstraint for person: {person.ID}, during days: {days - 1}-{days + 1}");
            AddCoefficients(secondToLastDayAssignments, endConstraint, 1.0);
            AddCoefficients(lastDayAssignments, endConstraint, -1.0);
            endConstraint.SetCoefficient(afterDayVariable, 1.0);

            for (var i = 1; i <= days - minSeqMinValue; i++)
            {
                var currentAssaignments = assignments.Where(a => a.Shift.Day == i).ToList();
                var nextDayAssaignments = assignments.Where(a => a.Shift.Day == i + 1).ToList();
                var dayAfterNextDayAssaignments = assignments.Where(a => a.Shift.Day == i + 2).ToList();
                var constraint = solver.MakeConstraint(-1.0, 1.0,
                    $"MinSeqconstraint for person: {person.ID}, during days: {i}-{i + 2}");
                AddCoefficients(currentAssaignments, constraint, 1.0);
                AddCoefficients(nextDayAssaignments, constraint, -1.0);
                AddCoefficients(dayAfterNextDayAssaignments, constraint, 1.0);
            }
        }

        private static void CreateMinSeqConstraint(Person person, int minSeqMinValue, IEnumerable<Assignment> assaignments, Solver solver)
        {
            var assignments = assaignments as Assignment[] ?? assaignments.ToArray();
            var days = assignments.Select(a => a.Shift.Day).Max();

            if (days < minSeqMinValue)
                throw new ArgumentException("There are not enough days in Scdeuling period for this MinSequence");

            var beforeDayVariable =
                solver.MakeIntVar(0.0, 0.0, $"-1st day variable for perosn: {person.ID}");
            var firstDayAssignments = assignments.Where(a => a.Shift.Day == 0).ToList();
            var secondDayAssignments = assignments.Where(a => a.Shift.Day == 1).ToList();
            var startConstraint = solver.MakeConstraint(-2, 0,
                $"MinSeqconstraint for person: {person.ID}, during days: {-1}-{2}");
            startConstraint.SetCoefficient(beforeDayVariable, -1.0);
            AddCoefficients(firstDayAssignments, startConstraint);
            AddCoefficients(secondDayAssignments, startConstraint, -1.0);


            var lastDayAssignments = assignments.Where(a => a.Shift.Day == days).ToList();
            var secondToLastDayAssignments = assignments.Where(a => a.Shift.Day == days - 1).ToList();
            var afterDayVariable =
                solver.MakeIntVar(0.0, 0.0, $"+1 day variable for perosn: {person.ID}");
            var endConstraint = solver.MakeConstraint(-2, 0,
                $"MinSeqconstraint for person: {person.ID}, during days: {days - 1}-{days + 1}");
            AddCoefficients(secondToLastDayAssignments, endConstraint, -1.0);
            AddCoefficients(lastDayAssignments, endConstraint);
            endConstraint.SetCoefficient(afterDayVariable, -1);

            for (var i = 1; i <= days - minSeqMinValue; i++)
            {
                var currentAssaignments = assignments.Where(a => a.Shift.Day == i).ToList();
                var nextDayAssaignments = assignments.Where(a => a.Shift.Day == i + 1).ToList();
                var dayAfterNextDayAssaignments = assignments.Where(a => a.Shift.Day == i + 2).ToList();
                var constraint = solver.MakeConstraint(-2, 0,
                    $"MinSeqconstraint for person: {person.ID}, during days: {i}-{i + 2}");
                AddCoefficients(currentAssaignments, constraint, -1.0);
                AddCoefficients(nextDayAssaignments, constraint);
                AddCoefficients(dayAfterNextDayAssaignments, constraint, -1.0);
            }
        }

        private static void OnlyOneWeekendConstraints(SchedulingIpContext scheduleContext)
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

        private static void CoverRequirementsConstraints(SchedulingIpContext scheduleContext)
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

        private static void ShiftOffRequestConstraints(SchedulingIpContext scheduleContext)
        {
            foreach (var person in scheduleContext.Persons.Values)
            {
                foreach (var (day, shiftOffRequest) in person.ShiftOffRequests)
                {
                    var constraint = scheduleContext.Solver.MakeConstraint(0.0, 0.0,
                        $"ShiftOnRequestConstraint {person.ID}, day: {day}, shift: {shiftOffRequest.Type}");
                    var potentialPersonAssaignments = scheduleContext.Assignments.Where(a =>
                            a.Person.ID.Equals(person.ID) && a.Shift.Day == day &&
                            a.Shift.Type.ID.Equals(shiftOffRequest.Type.ID))
                        .Select(a => a.assigningGraphEdge);
                    foreach (var variable in potentialPersonAssaignments)
                    {
                        constraint.SetCoefficient(variable, 1.0);
                    }

                    constraint.SetCoefficient(shiftOffRequest.ShiftOffRrequestVariable, -1.0);
                }
            }
        }

        private static void ShiftOnRequestConstraints(SchedulingIpContext scheduleContext)
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

        private static void CreateAssignmentGraph(SchedulingIpContext scheduleContext)
        {
            var graphEdges = 0;
            var graphStarts = scheduleContext.Solver.NumConstraints();

            var shiftEmployeePairs = new List<Variable>();
            foreach (var person in scheduleContext.Persons.Values)
            {
                var contract = scheduleContext.ContractDictionary.Where(cid => person.ContractIDs.Contains(cid.Key) && cid.Value.MinWork.HasValue).Select(c => c.Value).Single();
                var min = contract.MinWork.Value;
                var max = contract.MaxWork.Value;
                var constraint = scheduleContext.Solver.MakeConstraint(min, max, $"workload constraint for person: {person.ID}");
                foreach (var shift in scheduleContext.Shifts)

                {
                    // Változó egy összerendelési élre
                    Variable v;
                    if (person.FixedAssignments.Any(a => a.Day == shift.Day && shift.Type.ID == a.Type.ID))
                    {
                        v = scheduleContext.Solver.MakeIntVar(1.0, 1.0, $"{person.ID}-{shift.Index}");
                    }
                    else if (person.FixedFreeDays.Any(a => a.Day == shift.Day))
                    {
                        v = scheduleContext.Solver.MakeIntVar(0.0, 0.0, $"{person.ID}-{shift.Index}");
                    }
                    else
                    {
                        v = scheduleContext.Solver.MakeIntVar(0.0, 1.0, $"{person.ID}-{shift.Index}");
                    }

                    //var assignments = scheduleContext.Assignments.Where(a => a.Person.ID.Equals(person.ID));
                    constraint.SetCoefficient(v, shift.Type.DurationInMnutes);

                    //  scheduleContext.Variables.Add(v);
                    var assignment = new Assignment()
                    {
                        Index = graphEdges,
                        assigningGraphEdge = v,
                        Person = person,
                        Shift = shift
                    };
                    scheduleContext.Assignments.Add(assignment);
                    shiftEmployeePairs.Add(v);
                    graphEdges++;
                }
            }

            scheduleContext.GraphStartsAt = graphStarts;
            scheduleContext.GraphEdges = graphEdges;
        }


        private static void AddCoefficients(IEnumerable<Assignment> assaignmentList, Constraint constraint,
           double coefficient = 1.0)
        {
            foreach (var assaignment in assaignmentList)
            {
                constraint.SetCoefficient(assaignment.assigningGraphEdge, coefficient);
            }
        }
    }
}

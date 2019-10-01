using SchedulingBenchmarks.CostFunctions;
using SchedulingBenchmarks.Models;
using SchedulingBenchmarks.StateCalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulingBenchmarks.Schedulers
{
    internal class ReplaceSingleWeekendsWithDoubleWeekendsScheduler : SchedulerBase
    {
        private readonly StateCalculator _stateCalculator;

        public ReplaceSingleWeekendsWithDoubleWeekendsScheduler(SchedulerModel model, StateCalculator stateCalculator, WorkEligibilityChecker workEligibilityChecker, CostFunctionBase costFunction) 
            : base(model, costFunction, workEligibilityChecker)
        {
            _stateCalculator = stateCalculator ?? throw new ArgumentNullException(nameof(stateCalculator));
        }

        public override void Run()
        {
            Parallel.ForEach(_model.People, p =>
            {
                p.Assignments.StartNewRound();
                _stateCalculator.InitializeState(p);
            });

            foreach (var day in _model.SchedulePeriod)
            {
                Parallel.ForEach(_model.People, p => _stateCalculator.RefreshState(p, day));

                if (_model.Calendar.IsWeekend(day))
                {
                    foreach (var person in _model.People)
                    {
                        if (HasOneAssignmentOnThisWeekend(person, day, out var d) && CanRemoveAssignment(person, d))
                        {
                            var originalState = person.State.Clone();
                            var originalAssignments = person.Assignments.Clone();

                            person.Assignments.Remove(d);
                            _stateCalculator.InitializeState(person);

                            var originalWeekEnd = _model.Calendar.IsSaturday(day) ? day + 1 : day;
                            var newAssignments = new Assignment[2];
                            State stateOnSaturday = null;
                            AssignmentsCollection assignmentsCollectionOnSaturday = null;

                            foreach (var day2 in _model.SchedulePeriod)
                            {
                                _stateCalculator.RefreshState(person, day2);

                                if (day2 > originalWeekEnd)
                                {
                                    if (_model.Calendar.IsSaturday(day2) && _workEligibilityChecker.CanWorkOnDay(person, day2))
                                    {
                                        foreach (var demand in SelectDemandsForDay(day2))
                                        {
                                            var cost = _costFunction.CalculateCost(person, demand, day2);
                                            if (Math.Abs(cost - _costFunction.MaxCost) > double.Epsilon)
                                            {
                                                stateOnSaturday = person.State.Clone();
                                                assignmentsCollectionOnSaturday = person.Assignments.Clone();
                                                newAssignments[0] = person.Assignments.Add(day2, new Assignment(person, day2, demand.Shift));
                                                break;
                                            }
                                        }
                                    }

                                    if (newAssignments[0] != null && _model.Calendar.IsSunday(day2) && _workEligibilityChecker.CanWorkOnDay(person, day2))
                                    {
                                        foreach (var demand in SelectDemandsForDay(day2))
                                        {
                                            var cost = _costFunction.CalculateCost(person, demand, day2);
                                            if (Math.Abs(cost - _costFunction.MaxCost) > double.Epsilon)
                                            {
                                                newAssignments[1] = person.Assignments.Add(day2, new Assignment(person, day2, demand.Shift));
                                                goto NewAssigningEnded;
                                            }
                                        }
                                    }

                                    if (_model.Calendar.IsSunday(day2) && newAssignments[0] != null)
                                    {
                                        person.Assignments = assignmentsCollectionOnSaturday;
                                        person.State = stateOnSaturday;
                                        _stateCalculator.RefreshState(person, day2);
                                        newAssignments[0] = null;
                                    }
                                }
                            }

                        NewAssigningEnded:
                            person.Assignments = originalAssignments;
                            person.State = originalState;

                            if (newAssignments[0] != null && newAssignments[1] != null)
                            {
                                person.Assignments.Remove(d);
                                person.Assignments.Add(newAssignments[0].Day, newAssignments[0]);
                                person.Assignments.Add(newAssignments[1].Day, newAssignments[1]);
                            }

                            _stateCalculator.InitializeState(person);
                            foreach (var dd in _model.SchedulePeriod.Mutate(end: day)) _stateCalculator.RefreshState(person, dd);
                        }
                    }
                }
            }
        }

        private bool HasOneAssignmentOnThisWeekend(Person p, int day, out int d)
        {
            if (_model.Calendar.IsSaturday(day))
            {
                if (p.Assignments.AllRounds.ContainsKey(day)) { d = day; return true; }
                if (p.Assignments.AllRounds.ContainsKey(day + 1)) { d = day + 1; return true; }
            }

            if (_model.Calendar.IsSunday(day))
            {
                if (p.Assignments.AllRounds.ContainsKey(day - 1)) { d = day - 1; return true; }
                if (p.Assignments.AllRounds.ContainsKey(day)) { d = day; return true; }
            }

            d = 0;
            return false;
        }

        private bool CanRemoveAssignment(Person p, int day)
        {
            if (p.WorkSchedule.MinConsecutiveWorkDays < 2) return true;

            return Range.Of(day + 1, length: p.WorkSchedule.MinConsecutiveWorkDays).All(d => p.Assignments.AllRounds.ContainsKey(d))
                || Range.Of(end: day - 1, length: p.WorkSchedule.MinConsecutiveWorkDays).All(d => p.Assignments.AllRounds.ContainsKey(d));
        }
    }
}

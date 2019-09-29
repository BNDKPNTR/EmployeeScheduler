using SchedulingBenchmarks.CostFunctions;
using SchedulingBenchmarks.LapAlgorithms;
using SchedulingBenchmarks.Models;
using SchedulingBenchmarks.StateCalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulingBenchmarks
{
    class SchedulerAlgorithm
    {
        private readonly SchedulerModel _model;
        private readonly StateCalculator _stateCalculator;
        private readonly WorkEligibilityChecker _workEligibilityChecker;
        private readonly CostFunctionBase _costFunction;

        public SchedulerAlgorithm(SchedulerModel model)
        {
            _model = model;
            _stateCalculator = new StateCalculator(_model.SchedulePeriod, _model.Calendar);
            _workEligibilityChecker = new WorkEligibilityChecker(_model);
            _costFunction = CreateCompositeCostFunction(_model);
        }

        public void Run()
        {
            //SchedulePeopleForMinDemands();
            //SchedulePeopleForWeekends();
            SchedulePeopleForAllDemands();
            SchedulePeopleUntilMinTotalWorkTime();
            ReplaceSingleWeekendsWithDoubleWeekends();
            RemoveUnderMinConsecutiveShifts();
        }

        private void SchedulePeopleForMinDemands()
        {
            Parallel.ForEach(_model.People, _model.ParallelOptions, person =>
            {
                person.Assignments.StartNewRound();
                _stateCalculator.InitializeState(person);
            });

            foreach (var day in _model.SchedulePeriod)
            {
                Parallel.ForEach(_model.People, _model.ParallelOptions, person => _stateCalculator.RefreshState(person, day));

                var availablePeople = SelectPeopleForMinDemands(day);
                var demands = SelectDemandsForDay(day);

                if (availablePeople.Count > 0 && demands.Length > 0 && availablePeople.Count <= demands.Length)
                {
                    SchedulePeople(day, availablePeople, demands);
                }
            }
        }

        private void SchedulePeopleForWeekends()
        {
            Parallel.ForEach(_model.People, _model.ParallelOptions, person =>
            {
                person.Assignments.StartNewRound();
                _stateCalculator.InitializeState(person);
            });

            foreach (var day in _model.SchedulePeriod)
            {
                Parallel.ForEach(_model.People, _model.ParallelOptions, person => _stateCalculator.RefreshState(person, day));

                if (_model.Calendar.IsWeekend(day))
                {
                    var availablePeople = SelectPeopleForDay(day);
                    var demands = SelectDemandsForDay(day);

                    if (availablePeople.Count > 0 && demands.Length > 0)
                    {
                        SchedulePeople(day, availablePeople, demands);
                    }
                }
            }
        }

        private void SchedulePeopleForAllDemands()
        {
            Parallel.ForEach(_model.People, _model.ParallelOptions, person =>
            {
                person.Assignments.StartNewRound();
                _stateCalculator.InitializeState(person);
            });

            foreach (var day in _model.SchedulePeriod)
            {
                Parallel.ForEach(_model.People, _model.ParallelOptions, person => _stateCalculator.RefreshState(person, day));

                var availablePeople = SelectPeopleForDay(day);
                var demands = SelectDemandsForDay(day);

                if (availablePeople.Count > 0 && demands.Length > 0)
                {
                    SchedulePeople(day, availablePeople, demands);
                }
            }
        }

        private void SchedulePeople(int day, List<Person> people, Demand[] demands)
        {
            var costMatrix = CreateCostMatrix(size: Math.Max(people.Count, demands.Length));

            Parallel.For(0, people.Count, _model.ParallelOptions, i =>
            {
                var person = people[i];

                for (int j = 0; j < demands.Length; j++)
                {
                    costMatrix[i][j] = _costFunction.CalculateCost(person, demands[j], day);
                }
            });

            var (copulationVerticesX, _) = EgervaryAlgorithmV2.RunAlgorithm(costMatrix, _costFunction.MaxCost);
            CreateAssignments(day, costMatrix, copulationVerticesX, people, demands);
        }

        private void CreateAssignments(int day, double[][] costMatrix, int[] copulationVerticesX, List<Person> people, Demand[] demands)
        {
            for (int i = 0; i < copulationVerticesX.Length; i++)
            {
                var j = copulationVerticesX[i];
                if (Math.Abs(costMatrix[i][j] - _costFunction.MaxCost) > double.Epsilon)
                {
                    if (i < people.Count && j < demands.Length)
                    {
                        var person = people[i];
                        var demand = demands[j];

                        person.Assignments.Add(day, new Assignment(person, day, demand.Shift));
                    }
                }
            }
        }

        private List<Person> SelectPeopleForMinDemands(int day)
            => _model.People
            .Where(p => p.Availabilities[day])
            .ToList();

        private List<Person> SelectPeopleForDay(int day)
            => _model.People
            .Where(p => _workEligibilityChecker.CanWorkOnDay(p, day))
            .ToList();

        private Demand[] SelectDemandsForDay(int day)
        {
            var demands = _model.Demands[day];
            var demandsByIndex = new List<Demand>();
            var assignedShiftCounts = _model.People
                .Where(p => p.Assignments.AllRounds.ContainsKey(day))
                .Select(p => p.Assignments.AllRounds[day])
                .GroupBy(a => a.Shift)
                .ToDictionary(g => g.Key, g => g.Count());

            for (int i = 0; i < demands.Length; i++)
            {
                var demand = demands[i];
                assignedShiftCounts.TryGetValue(demand.Shift, out var assignedPeopleCount);
                var requiredPeopleCount = demand.RequiredPeopleCount - assignedPeopleCount;

                for (int j = 0; j < requiredPeopleCount; j++)
                {
                    demandsByIndex.Add(demand);
                }
            }

            return demandsByIndex.ToArray();
        }

        private double[][] CreateCostMatrix(int size)
        {
            var costMatrix = new double[size][];

            for (int i = 0; i < size; i++)
            {
                costMatrix[i] = new double[size];

                for (int j = 0; j < size; j++)
                {
                    costMatrix[i][j] = _costFunction.MaxCost;
                }
            }

            return costMatrix;
        }

        private void SchedulePeopleUntilMinTotalWorkTime()
        {
            Parallel.ForEach(_model.People.Where(p => p.State.TotalWorkTime < p.WorkSchedule.MinTotalWorkTime), _model.ParallelOptions, person =>
            {
                person.Assignments.StartNewRound();
                _stateCalculator.InitializeState(person);

                foreach (var day in _model.SchedulePeriod)
                {
                    _stateCalculator.RefreshState(person, day);

                    if (!_workEligibilityChecker.CanWorkOnDay(person, day)) continue;
                    if (person.State.TotalWorkTime >= person.WorkSchedule.MinTotalWorkTime)
                    {
                        if (person.State.ConsecutiveWorkDayCount >= person.WorkSchedule.MinConsecutiveWorkDays 
                        || person.State.TotalWorkTime >= person.WorkSchedule.MaxTotalWorkTime)
                        {
                            break;
                        }
                    }
                    if (person.Assignments.AllRounds.ContainsKey(day)) continue;

                    foreach (var demand in _model.Demands[day])
                    {
                        if (_costFunction.CalculateCost(person, demand, day) < _costFunction.MaxCost)
                        {
                            var assignment = new Assignment(person, day, demand.Shift);

                            person.Assignments.Add(day, assignment);
                            break;
                        }
                    }
                }
            });
        }

        private void ReplaceSingleWeekendsWithDoubleWeekends()
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

            bool HasOneAssignmentOnThisWeekend(Person p, int day, out int d)
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

            bool CanRemoveAssignment(Person p, int day)
            {
                if (p.WorkSchedule.MinConsecutiveWorkDays < 2) return true;

                return Range.Of(day + 1, length: p.WorkSchedule.MinConsecutiveWorkDays).All(d => p.Assignments.AllRounds.ContainsKey(d))
                    || Range.Of(end: day - 1, length: p.WorkSchedule.MinConsecutiveWorkDays).All(d => p.Assignments.AllRounds.ContainsKey(d));
            }
        }

        private void RemoveUnderMinConsecutiveShifts()
        {
            Parallel.ForEach(_model.People, person =>
            {
                foreach (var assignmentGroup in GetAssignmentGroups(person).Where(r => r.Start != 0 && r.End != _model.SchedulePeriod.End))
                {
                    if (assignmentGroup.Length < person.WorkSchedule.MinConsecutiveWorkDays)
                    {
                        foreach (var day in assignmentGroup)
                        {
                            person.Assignments.Remove(day);
                        }
                    }
                }
            });

            List<Range> GetAssignmentGroups(Person p)
            {
                var assignmentGroups = new List<Range>();

                if (p.Assignments.AllRounds.Count == 0) return assignmentGroups;

                var firstAssignmentInRow = p.Assignments.AllRounds.Values.First();
                var lastAssignmentInRow = firstAssignmentInRow;

                foreach (var assignment in p.Assignments.AllRounds.Values.Skip(1))
                {
                    if (assignment.Day != lastAssignmentInRow.Day + 1)
                    {
                        assignmentGroups.Add(Range.Of(firstAssignmentInRow.Day, lastAssignmentInRow.Day));

                        firstAssignmentInRow = assignment;
                        lastAssignmentInRow = firstAssignmentInRow;
                    }
                    else
                    {
                        lastAssignmentInRow = assignment;
                    }
                }

                assignmentGroups.Add(Range.Of(firstAssignmentInRow.Day, lastAssignmentInRow.Day));

                return assignmentGroups;
            }
        }

        private CompositeCostFunction CreateCompositeCostFunction(SchedulerModel model)
        {
            var (maxShiftOffRequestWeight, maxShiftOnRequestWeight) = ShiftRequestCostFunction.GetMaxWeights(model);

            var costFunctions = new CostFunctionBase[]
            {
                new WeekendWorkCostFunction(_model.Calendar),
                new TotalWorkTimeCostFunction(_model, _workEligibilityChecker, _stateCalculator),
                new ShiftRequestCostFunction(maxShiftOffRequestWeight, maxShiftOnRequestWeight),
                new ConsecutiveShiftCostFunction(_model.Calendar),
                new DayOffCostFunction(),
                new ValidShiftCostFunction(),
                new MaxShiftCostFunction(),
                new MinRestTimeCostFunction()
            };

            return new CompositeCostFunction(costFunctions);
        }
    }
}

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
        private readonly CostFunctionBase _costFunction;

        public SchedulerAlgorithm(SchedulerModel model)
        {
            _model = model;
            _stateCalculator = new StateCalculator(_model.SchedulePeriod, _model.Calendar);
            _costFunction = CreateCompositeCostFunction();
        }

        public void Run()
        {
            SchedulePeopleForMinDemands();
            SchedulePeopleForAllDemands();
            SchedulePeopleUntilMinTotalWorkTime();
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
            .Where(p => CanWorkOnDay(p, day))
            .ToList();

        private bool CanWorkOnDay(Person person, int day)
        {
            if (!Available(person, day)) return false;
            if (AlreadyHasAssignmentOnDay(person, day)) return false;
            if (WouldWorkLessThanMinConsecutiveDays(person, day)) return false;
            if (WouldWorkMoreThanMaxConsecutiveDays(person, day)) return false;
            if (WouldRestLessThanMinConsecutiveDayOff(person, day)) return false;
            if (WouldWorkMoreThanMaxWeekends(person, day)) return false;

            return true;

            bool Available(Person p, int d) => p.Availabilities[d];
            bool AlreadyHasAssignmentOnDay(Person p, int d) => p.Assignments.AllRounds.ContainsKey(d);

            bool WouldWorkLessThanMinConsecutiveDays(Person p, int d)
            {
                if (p.State.ConsecutiveWorkDayCount > 0) return false;
                if (d == 0) return false; // We assume that the person worked infinite numbers of days before the schedule period

                // TODO: check MaxTotalWorkTime

                for (int i = d; i < d + p.WorkSchedule.MinConsecutiveWorkDays; i++)
                {
                    if (i < p.Availabilities.Length && !p.Availabilities[i]) return true;
                    if (_model.Calendar.IsWeekend(i) && p.State.WorkedWeekendCount >= p.WorkSchedule.MaxWorkingWeekendCount) return true;
                }

                return false;
            }

            bool WouldWorkMoreThanMaxConsecutiveDays(Person p, int d)
            {
                var alreadyWorkedConsecutiveDays = p.State.ConsecutiveWorkDayCount;
                var todaysPossibleWork = 1;
                var consecutiveWorkDaysInFuture = 0;

                // Count until has assignment AND consecutive work days DO NOT exceed max. consecutive workdays
                var dayIndex = d + 1;
                while (p.Assignments.AllRounds.ContainsKey(dayIndex++) && ++consecutiveWorkDaysInFuture < p.WorkSchedule.MaxConsecutiveWorkDays) { }

                return alreadyWorkedConsecutiveDays + todaysPossibleWork + consecutiveWorkDaysInFuture > p.WorkSchedule.MaxConsecutiveWorkDays;
            }

            bool WouldRestLessThanMinConsecutiveDayOff(Person p, int d)
            {
                if (p.State.ConsecutiveWorkDayCount > 0)
                {
                    //var maxConsecutiveShiftDay = day - p.State.ConsecutiveWorkDayCount + p.WorkSchedule.MaxConsecutiveWorkDays;

                    //for (int i = maxConsecutiveShiftDay; i < maxConsecutiveShiftDay + p.WorkSchedule.MinConsecutiveDayOffs; i++)
                    //{
                    //    if (p.Assignments.AllRounds.ContainsKey(i)) return true;
                    //}

                    return false;
                }
                else
                {
                    if (p.State.ConsecutiveDayOffCount < p.WorkSchedule.MinConsecutiveDayOffs) return true;

                    // If today's shift would be the continuation of tomorrow's shift
                    if (p.Assignments.AllRounds.ContainsKey(d + 1)) return false;

                    /* Today's shift would be the first in a row. Check if person could work min. consecutive days and then rest min. consecutive days before next work */
                    var firstDayOfRestPeriod = d + p.WorkSchedule.MinConsecutiveWorkDays;
                    var lastDayOfRestPeriod = firstDayOfRestPeriod + p.WorkSchedule.MinConsecutiveDayOffs;

                    for (int i = firstDayOfRestPeriod; i < lastDayOfRestPeriod; i++)
                    {
                        if (p.Assignments.AllRounds.ContainsKey(i)) return true;
                    }

                    return false;
                }
            }

            bool WouldWorkMoreThanMaxWeekends(Person p, int d)
            {
                if (!_model.Calendar.IsWeekend(d)) return false;

                return p.State.WorkedWeekendCount >= p.WorkSchedule.MaxWorkingWeekendCount;
            }
        }

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
                var requiredPeopleCount = demand.MaxPeopleCount - assignedPeopleCount;

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

                    if (!CanWorkOnDay(person, day)) continue;
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

        private CompositeCostFunction CreateCompositeCostFunction()
        {
            var costFunctions = new CostFunctionBase[]
            {
                new WeekendWorkCostFunction(_model.Calendar),
                new TotalWorkTimeCostFunction(),
                new ShiftRequestCostFunction(),
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

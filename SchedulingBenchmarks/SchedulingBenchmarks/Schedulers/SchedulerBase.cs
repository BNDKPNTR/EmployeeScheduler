using SchedulingBenchmarks.CostFunctions;
using SchedulingBenchmarks.LapAlgorithms;
using SchedulingBenchmarks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulingBenchmarks.Schedulers
{
    internal abstract class SchedulerBase
    {
        protected readonly SchedulerModel _model;
        protected readonly CostFunctionBase _costFunction;
        protected readonly WorkEligibilityChecker _workEligibilityChecker;

        protected SchedulerBase(SchedulerModel model, CostFunctionBase costFunction, WorkEligibilityChecker workEligibilityChecker)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
            _costFunction = costFunction ?? throw new ArgumentNullException(nameof(costFunction));
            _workEligibilityChecker = workEligibilityChecker ?? throw new ArgumentNullException(nameof(workEligibilityChecker));
        }

        public abstract void Run();

        protected List<Person> SelectPeopleForDay(int day)
            => _model.People
            .Where(p => _workEligibilityChecker.CanWorkOnDay(p, day))
            .ToList();

        protected Demand[] SelectDemandsForDay(int day)
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

        protected void SchedulePeople(int day, List<Person> people, Demand[] demands)
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

        protected void CreateAssignments(int day, double[][] costMatrix, int[] copulationVerticesX, List<Person> people, Demand[] demands)
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
    }
}

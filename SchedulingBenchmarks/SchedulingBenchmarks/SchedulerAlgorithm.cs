using SchedulingBenchmarks.CostFunctions;
using SchedulingBenchmarks.LapAlgorithms;
using SchedulingBenchmarks.Models;
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
            _stateCalculator = new StateCalculator(_model.SchedulePeriod);
            _costFunction = CreateCompositeCostFunction();
        }

        public void Run()
        {
            Parallel.ForEach(_model.People, person => _stateCalculator.InitializeState(person));

            foreach (var day in _model.SchedulePeriod)
            {
                Parallel.ForEach(_model.People, person => _stateCalculator.RefreshState(person, day));

                var availablePeople = SelectPeopleForDay(day);
                var demands = SelectDemandsForDay(day);

                if (availablePeople.Count > 0 && demands.Length > 0)
                {
                    SchedulePeople(day, availablePeople, demands);
                }
            }

            SchedulePeopleUntilMinTotalWorkTime();
        }

        private void SchedulePeople(int day, List<Person> people, Demand[] demands)
        {
            var costMatrix = CreateCostMatrix(size: Math.Max(people.Count, demands.Length));

            Parallel.For(0, people.Count, i =>
            {
                var person = people[i];

                for (int j = 0; j < demands.Length; j++)
                {
                    costMatrix[i][j] = _costFunction.CalculateCost(person, demands[j], day);
                }
            });

            //DumpCostMatrix(costMatrix);
            //var result = JonkerVolgenantAlgorithmV2.RunAlgorithm(costMatrix);
            var result = EgervaryAlgorithmV2.RunAlgorithm(costMatrix, _costFunction.MaxCost);
            CreateAssignments(day, costMatrix, result.copulationVerticesX, people, demands);
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

                        person.Assignments[day] = new Assignment(person, day, demand.Shift); 
                    }
                }
            }
        }

        private List<Person> SelectPeopleForDay(int day)
            => _model.People
            //.Where(p => p.Availabilities[timeSlot])
            .ToList();

        private Demand[] SelectDemandsForDay(int day)
        {
            var demands = _model.Demands[day];
            var demandsByIndex = new Demand[demands.Sum(d => d.MaxPeopleCount)];

            var k = 0;
            for (int i = 0; i < demands.Length; i++)
            {
                var demand = demands[i];

                for (int j = 0; j < demand.MaxPeopleCount; j++)
                {
                    demandsByIndex[k++] = demand;
                }
            }

            return demandsByIndex;
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
            Parallel.ForEach(_model.People.Where(p => p.State.TotalWorkTime < p.WorkSchedule.MinTotalWorkTime), person =>
            {
                _stateCalculator.InitializeState(person);

                foreach (var day in _model.SchedulePeriod)
                {
                    if (person.State.TotalWorkTime >= person.WorkSchedule.MinTotalWorkTime)
                    {
                        break;
                    }

                    _stateCalculator.RefreshState(person, day);

                    if (person.Assignments.ContainsKey(day))
                    {
                        continue;
                    }

                    foreach (var demand in _model.Demands[day])
                    {
                        if (_costFunction.CalculateCost(person, demand, day) < _costFunction.MaxCost)
                        {
                            var assignment = new Assignment(person, day, demand.Shift);
                            person.Assignments[day] = assignment;
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
                new AvailabilityCostFunction(),
                new WeekendWorkCostFunction(),
                new TotalWorkTimeCostFunction(),
                new ShiftRequestCostFunction(),
                new ConsecutiveShiftCostFunction(),
                new DayOffCostFunction(),
                new ValidShiftCostFunction(),
                new MaxShiftCostFunction(),
                new MinRestTimeCostFunction()
            };

            return new CompositeCostFunction(costFunctions);
        }

        private static void DumpCostMatrix(double[][] costMatrix)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(double[][]));
            using (var stream = System.IO.File.Create("costMatrix.xml"))
            {
                serializer.Serialize(stream, costMatrix);
            }
        }
    }
}

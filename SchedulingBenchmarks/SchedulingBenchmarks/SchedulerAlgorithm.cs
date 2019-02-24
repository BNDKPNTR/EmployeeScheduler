using SchedulingBenchmarks.CostFunctions;
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
            _stateCalculator = new StateCalculator(_model.Calendar);
            _costFunction = CreateCompositeCostFunction();
        }

        public void Run()
        {
            foreach (var timeSlot in _model.SchedulePeriod)
            {
                Parallel.ForEach(_model.People, person => _stateCalculator.RefreshState(person, timeSlot));

                var availablePeople = SelectAvailablePeopleForTimeSlot(timeSlot);
                var demands = SelectDemandsForTimeSlot(timeSlot);

                if (availablePeople.Count > 0 && demands.Length > 0)
                {
                    SchedulePeople(timeSlot, availablePeople, demands);
                }
            }
        }

        private List<Person> SelectAvailablePeopleForTimeSlot(int timeSlot)
            => _model.People.Where(p => p.Availabilities[timeSlot]).ToList();

        private void SchedulePeople(int timeSlot, List<Person> people, Demand[] demands)
        {
            var costMatrix = CreateCostMatrix(size: Math.Max(people.Count, demands.Length));

            Parallel.For(0, people.Count, i =>
            {
                var person = people[i];

                for (int j = 0; j < demands.Length; j++)
                {
                    costMatrix[i][j] = _costFunction.CalculateCost(person, demands[j], timeSlot);
                }
            });

            var result = JonkerVolgenantAlgorithm.RunAlgorithm(costMatrix);
            CreateAssignments(timeSlot, costMatrix, result.copulationVerticesX, people, demands);
        }

        private void CreateAssignments(int timeSlot, double[][] costMatrix, int[] copulationVerticesX, List<Person> people, Demand[] demands)
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

                        person.Assignments[timeSlot] = new Assignment(person, timeSlot, demand.ShifId); 
                    }
                }
            }
        }

        private Demand[] SelectDemandsForTimeSlot(int timeSlot)
        {
            var currentDemand = _model.Demands[timeSlot];
            var demandsByIndex = new Demand[currentDemand.MinPeopleCount];

            for (int i = 0; i < currentDemand.MinPeopleCount; i++)
            {
                demandsByIndex[i] = currentDemand;
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

        private CompositeCostFunction CreateCompositeCostFunction()
        {
            var costFunctions = new CostFunctionBase[]
            {
                //new AvailabilityCostFunction(),
                new WorkStartTimeCostFunction(_model.Calendar)
            };

            return new CompositeCostFunction(costFunctions);
        }
    }
}

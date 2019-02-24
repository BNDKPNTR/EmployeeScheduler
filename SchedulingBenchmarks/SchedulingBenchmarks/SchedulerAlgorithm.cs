using Scheduler.CostFunctions;
using Scheduler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InputModel = Scheduler.Dto.InputModel;

namespace Scheduler
{
    class SchedulerAlgorithm
    {
        private readonly SchedulerModel _model;
        private readonly StateCalculator _stateCalculator;
        private readonly CostFunctionBase _costFunction;

        private SchedulerAlgorithm(SchedulerModel model)
        {
            _model = model;
            _stateCalculator = new StateCalculator(_model.Calendar);
            _costFunction = CreateCompositeCostFunction();
        }

        public static InputModel Run(InputModel input)
        {
            var model = InputModelMapper.MapToScheduleModel(input);
            var scheduler = new SchedulerAlgorithm(model);

            scheduler.RunInternal();

            return SchedulerModelMapper.MapToInputModel(model, input);
        }

        private void RunInternal()
        {
            foreach (var timeSlot in _model.SchedulePeriod)
            {
                _model.AllDemands.MoveNext(timeSlot);

                Parallel.ForEach(_model.People, person => _stateCalculator.RefreshState(person, timeSlot));

                var availablePeople = SelectAvailablePeopleForTimeSlot();
                var demands = SelectDemandsForTimeSlot();

                if (availablePeople.Count > 0 && demands.Count > 0)
                {
                    SchedulePeople(timeSlot, availablePeople, demands);
                }
            }
        }

        private List<Person> SelectAvailablePeopleForTimeSlot() => _model.People.Where(p => p.Available).ToList();

        private void SchedulePeople(int timeSlot, List<Person> people, Dictionary<int, Demand> demands)
        {
            var costMatrix = CreateCostMatrix(size: Math.Max(people.Count, demands.Count));

            Parallel.For(0, people.Count, i =>
            {
                var person = people[i];

                for (int j = 0; j < demands.Count; j++)
                {
                    costMatrix[i][j] = _costFunction.CalculateCost(person, demands[j], timeSlot);
                }
            });

            var result = JonkerVolgenantAlgorithm.RunAlgorithm(costMatrix);
            CreateAssignments(timeSlot, costMatrix, result.copulationVerticesX, people, demands);
        }

        private void CreateAssignments(int timeSlot, double[][] costMatrix, int[] copulationVerticesX, List<Person> people, Dictionary<int, Demand> demands)
        {
            for (int i = 0; i < copulationVerticesX.Length; i++)
            {
                var j = copulationVerticesX[i];
                if (Math.Abs(costMatrix[i][j] - _costFunction.MaxCost) > double.Epsilon)
                {
                    if (i < people.Count && demands.TryGetValue(j, out var demand))
                    {
                        var person = people[i];

                        person.Assignments[timeSlot] = new Assignment(person, timeSlot, demand.Activity); 
                    }
                }
            }
        }

        private Dictionary<int, Demand> SelectDemandsForTimeSlot()
        {
            var demandsByIndex = new Dictionary<int, Demand>();
            var demands = _model.AllDemands.Current;

            for (int i = 0; i < demands.Count; i++)
            {
                for (int j = 0; j < demands[i].RequiredPeopleCount; j++)
                {
                    demandsByIndex[i + j] = demands[i];
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

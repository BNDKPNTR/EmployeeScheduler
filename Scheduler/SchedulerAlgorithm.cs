using Scheduler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler
{
    class SchedulerAlgorithm
    {
        private const double MaxValue = 1000;
        private readonly SchedulerModel _model;
        private readonly StateCalculator _stateCalculator;

        private SchedulerAlgorithm(SchedulerModel model)
        {
            _model = model;
            _stateCalculator = new StateCalculator(_model.Calendar);
        }

        public static Dto.InputModel Run(Dto.InputModel input)
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
                var demands = SelectDemandsForTimeSlot(timeSlot);

                var matrixSize = Math.Max(availablePeople.Count, demands.Count);
                var costMatrix = CreateCostMatrix(matrixSize);
                
                for (int i = 0; i < availablePeople.Count; i++)
                {
                    var person = availablePeople[i];

                    for (int j = 0; j < demands.Count; j++)
                    {
                        costMatrix[i][j] = 0;
                    }
                }

                var result = JonkerVolgenantAlgorithm.RunAlgorithm(costMatrix);
                CreateAssignments(timeSlot, costMatrix, result.copulationVerticesX, availablePeople, demands);
            }
        }

        private List<Person> SelectAvailablePeopleForTimeSlot() => _model.People.Where(p => p.Available).ToList();

        private void CreateAssignments(int timeSlot, double[][] costMatrix, int[] copulationVerticesX, List<Person> people, Dictionary<int, Demand> demands)
        {
            for (int i = 0; i < copulationVerticesX.Length; i++)
            {
                var j = copulationVerticesX[i];
                if (Math.Abs(costMatrix[i][j] - MaxValue) > double.Epsilon)
                {
                    if (i < people.Count && demands.TryGetValue(j, out var demand))
                    {
                        var person = people[i];

                        person.Assignments[timeSlot] = new Assignment(person, timeSlot, demand.Activity); 
                    }
                }
            }
        }

        private Dictionary<int, Demand> SelectDemandsForTimeSlot(int timeSlot)
        {
            var demandsByIndex = new Dictionary<int, Demand>();
            var demands = _model.AllDemands.Current.Where(d => d.Period.Contains(timeSlot)).ToArray();

            for (int i = 0; i < demands.Length; i++)
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
                    costMatrix[i][j] = MaxValue;
                }
            }

            return costMatrix;
        }
    }
}

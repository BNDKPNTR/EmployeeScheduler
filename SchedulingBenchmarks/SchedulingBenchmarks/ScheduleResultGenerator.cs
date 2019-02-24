using SchedulingBenchmarks.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchedulingBenchmarks
{
    public class ScheduleResultGenerator
    {
        private readonly Func<InputModel> _inputFactory;

        public ScheduleResultGenerator(Func<InputModel> inputFactory)
        {
            _inputFactory = inputFactory ?? throw new ArgumentNullException(nameof(inputFactory));
        }

        public IEnumerable<InputModel> GenerateResults()
        {
            foreach (var product in CartesianProduct(GenerateCartesianProductSequences()))
            {
                var input = _inputFactory();

                foreach (var person in input.People)
                {
                    person.Assignments = new List<Assignment>();
                }

                var demands = DemandFactory(input.Demands).ToList();

                var copulationVerticesY = product.ToArray();
                for (int i = 0; i < copulationVerticesY.Length; i++)
                {
                    var personIndex = copulationVerticesY[i];
                    var demandIndex = i;

                    var person = input.People[personIndex];
                    var demand = demands[demandIndex];

                    person.Assignments.Add(CreateAssignment(person, demand));
                }

                yield return input;
            }
        }

        private IEnumerable<Demand> DemandFactory(List<Demand> demands)
        {
            foreach (var demand in demands)
            {
                for (int i = 0; i < demand.RequiredPersonCount; i++)
                {
                    yield return demand;
                }
            }
        }

        private Assignment CreateAssignment(Person person, Demand demand)
        {
            return new Assignment
            {
                Start = demand.Start,
                End = demand.End,
                Person = person,
                Activity = demand.Activity
            };
        }

        private IEnumerable<IEnumerable<T>> CartesianProduct<T>(IEnumerable<IEnumerable<T>> sequences)
        {
            IEnumerable<IEnumerable<T>> emptyProduct = new[] { Enumerable.Empty<T>() };

            return sequences.Aggregate(
              emptyProduct,
              (accumulator, sequence) =>
                from accseq in accumulator
                from item in sequence
                select accseq.Concat(new[] { item }));
        }

        /// <summary>
        /// Annyi sor van, ahány beosztás elkészülhet, mindegyik egy, még beosztatlan beosztást reprezentál.
        /// Egy-egy sorban azoknak a dolgozóknak az indexei vannak, akik hozzárendelhetőek az adott beosztáshoz. Jelen esetben az összes beosztáshoz az összes dolgozó.
        /// </summary>
        private int[][] GenerateCartesianProductSequences()
        {
            var input = _inputFactory();
            var personIndexArray = Enumerable.Range(0, input.People.Count).Select(i => i).ToArray();

            return Enumerable.Range(0, input.Demands.Sum(d => d.RequiredPersonCount)).Select(_ => personIndexArray).ToArray();
        }
    }
}

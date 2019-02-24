using SchedulingBenchmarks.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchedulingBenchmarks
{
    public class ScheduleResultGenerator
    {
        private readonly Func<SchedulingPeriod> _inputFactory;

        public ScheduleResultGenerator(Func<SchedulingPeriod> inputFactory)
        {
            _inputFactory = inputFactory ?? throw new ArgumentNullException(nameof(inputFactory));
        }

        public IEnumerable<SchedulingPeriod> GenerateResults()
        {
            foreach (var product in CartesianProduct(GenerateCartesianProductSequences()))
            {
                var input = _inputFactory();

                throw new NotImplementedException();

                yield return input;
            }
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
            throw new NotImplementedException();
        }
    }
}

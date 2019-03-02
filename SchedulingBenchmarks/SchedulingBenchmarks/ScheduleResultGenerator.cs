using SchedulingBenchmarks.SchedulingBenchmarksModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchedulingBenchmarks
{
    public class ScheduleResultGenerator
    {
        private readonly Func<SchedulingBenchmarkModel> _inputFactory;
        private readonly Demand[] _allDemands;
        private readonly string[] _employeeIds;

        public ScheduleResultGenerator(Func<SchedulingBenchmarkModel> inputFactory)
        {
            _inputFactory = inputFactory ?? throw new ArgumentNullException(nameof(inputFactory));

            var input = _inputFactory();

            _allDemands = input.Demands.Values.SelectMany(d => d).SelectMany(d => Enumerable.Range(0, d.MaxEmployeeCount).Select(i => d)).ToArray();
            _employeeIds = input.Employees.Select(e => e.Id).ToArray();
        }

        public IEnumerable<SchedulingBenchmarkModel> GenerateResults(Action<int> reportGeneratedResultCount = null)
        {
            var generatedResultCount = 0;

            foreach (var product in CartesianProduct(GenerateCartesianProductSequences()))
            {
                var input = _inputFactory();

                var employees = input.Employees.ToDictionary(e => e.Id);
                var assignmentEmployeeProduct = product.ToArray();

                for (int i = 0; i < assignmentEmployeeProduct.Length; i++)
                {
                    var assignment = new Assignment
                    {
                        Day = _allDemands[i].Day,
                        PersonId = assignmentEmployeeProduct[i],
                        ShiftId = _allDemands[i].ShiftId
                    };

                    employees[assignmentEmployeeProduct[i]].Assignments[assignment.Day] = assignment;
                }

                generatedResultCount++;
                reportGeneratedResultCount?.Invoke(generatedResultCount);

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
        private string[][] GenerateCartesianProductSequences()
        {
            return Enumerable.Range(0, _allDemands.Length).Select(i => _employeeIds).ToArray();
        }
    }
}

using SchedulingBenchmarks.Dto;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SchedulingBenchmarks.Cli
{
    class Program
    {
        private static readonly bool ExecuteSchedulerAlgorithm = true;
        private static readonly bool ExecuteResultGenerator = true;

        static void Main(string[] args)
        {
            AlgorithmResult schedulerResult = null;
            List<AlgorithmResult> generatedResults = null;

            if (ExecuteSchedulerAlgorithm) schedulerResult = RunSchedulerAlgorithm(); 
            if (ExecuteResultGenerator) generatedResults = RunResultGenerator();

            CompareAndPrintResults(schedulerResult, generatedResults);
        }

        private static AlgorithmResult RunSchedulerAlgorithm()
        {
            SchedulingPeriod input = null;
            var sw = new Stopwatch();

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            sw.Start();
            var result = SchedulerAlgorithm.Run(input);
            sw.Stop();

            return new AlgorithmResult
            {
                Name = "Scheduler Algorithm Solution",
                Result = result,
                Cost = OptimalityEvaluator.CalculateCost(result),
                Feasible = FeasibilityEvaluator.Feasible(result),
                Duration = sw.Elapsed
            };
        }

        private static List<AlgorithmResult> RunResultGenerator()
        {
            var generator = new ScheduleResultGenerator(() => null);
            var feasibleResults = new ConcurrentBag<AlgorithmResult>();
            var resultNumber = 0;
            
            Parallel.ForEach(generator.GenerateResults(), result =>
            {
                if (FeasibilityEvaluator.Feasible(result))
                {
                    feasibleResults.Add(new AlgorithmResult
                    {
                        Name = $"Timetable {Interlocked.Increment(ref resultNumber)}",
                        Result = result,
                        Cost = OptimalityEvaluator.CalculateCost(result),
                        Feasible = true
                    });
                }
            });

            return feasibleResults.ToList();
        }

        private static void CompareAndPrintResults(AlgorithmResult schedulerResult, List<AlgorithmResult> generatedResults)
        {
            if (ExecuteResultGenerator)
            {
                generatedResults = generatedResults.OrderBy(x => x.Cost).ToList();
            }

            var equalsWithCheapest = string.Empty;
            if (ExecuteSchedulerAlgorithm && ExecuteResultGenerator && generatedResults.Count > 0)
            {
                var cheapestGeneratedResult = generatedResults[0];
                equalsWithCheapest = CompareResults(schedulerResult.Result, cheapestGeneratedResult.Result)
                    ? $"Equals with:\t{cheapestGeneratedResult.Name}"
                    : $"Doesn't match with {cheapestGeneratedResult.Name}";

            }

            if (ExecuteSchedulerAlgorithm)
            {
                PrintResultToConsole(schedulerResult, equalsWithCheapest, $"Duration:\t{schedulerResult.Duration}");
            }

            if (ExecuteResultGenerator)
            {
                foreach (var generatedResult in generatedResults)
                {
                    PrintResultToConsole(generatedResult);
                }
            }
        }

        private static void PrintResultToConsole(AlgorithmResult algorithmResult, params string[] additionalInformation)
        {
            var separator = new string('-', 80);

            Console.WriteLine(separator);
            Console.WriteLine($"Name:\t\t{algorithmResult.Name}");
            Console.WriteLine($"Cost:\t\t{algorithmResult.Cost}");

            if (!algorithmResult.Feasible)
            {
                Console.WriteLine("Feasibility:\tINFEASIBLE");
            }

            if (additionalInformation.Length > 0)
            {
                foreach (var text in additionalInformation.Where(x => !string.IsNullOrEmpty(x)))
                {
                    Console.WriteLine(text);
                }
            }

            Console.WriteLine();
            Console.WriteLine(CreateFormattedString(algorithmResult.Result));
            Console.WriteLine(separator);
        }

        private static string CreateFormattedString(SchedulingPeriod result)
        {
            throw new NotImplementedException();
        }

        private static bool CompareResults(SchedulingPeriod x, SchedulingPeriod y)
        {
            throw new NotImplementedException();
        }

        private class AlgorithmResult
        {
            public string Name { get; set; }
            public SchedulingPeriod Result { get; set; }
            public int Cost { get; set; }
            public bool Feasible { get; set; }
            public TimeSpan Duration { get; set; }
        }
    }
}

using SchedulingBenchmarks.Dto;
using SchedulingBenchmarks.Mappers;
using SchedulingBenchmarks.SchedulingBenchmarksModel;
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
        private static readonly bool ExecuteResultGenerator = false;
        private static readonly int InstanceNumber = 1;

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
            var dto = SchedulingBenchmarkInstanceReader.FromXml(InstanceNumber);
            var schedulingBenchmarkModel = DtoToSchedulingBenchmarkModelMapper.MapToSchedulingBenchmarkModel(dto);
            var sw = new Stopwatch();

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            sw.Start();
            var result = SchedulerAlgorithmRunner.Run(schedulingBenchmarkModel);
            sw.Stop();

            Clipboard.Copy(result.ToRosterViewerFormat());

            var (feasible, messages) = FeasibilityEvaluator.Evaluate(result);

            return new AlgorithmResult
            {
                Name = "Scheduler Algorithm Solution",
                Result = result,
                Penalty = OptimalityEvaluator.CalculatePenalty(result),
                Feasible = feasible,
                FeasibilityMessages = messages,
                Duration = sw.Elapsed
            };
        }

        private static List<AlgorithmResult> RunResultGenerator()
        {
            var dto = SchedulingBenchmarkInstanceReader.FromXml(InstanceNumber);

            var generator = new ScheduleResultGenerator(() => DtoToSchedulingBenchmarkModelMapper.MapToSchedulingBenchmarkModel(dto));
            var feasibleResults = new ConcurrentBag<AlgorithmResult>();
            var resultNumber = 0;
            
            Parallel.ForEach(generator.GenerateResults(ReportProgress), result =>
            {
                if (FeasibilityEvaluator.EvaluateQuickly(result))
                {
                    feasibleResults.Add(new AlgorithmResult
                    {
                        Name = $"Timetable {Interlocked.Increment(ref resultNumber)}",
                        Result = result,
                        Penalty = OptimalityEvaluator.CalculatePenalty(result),
                        Feasible = true
                    });
                }
            });

            return feasibleResults.ToList();

            void ReportProgress(int generatedResultCount) => Console.WriteLine(generatedResultCount);
        }

        private static void CompareAndPrintResults(AlgorithmResult schedulerResult, List<AlgorithmResult> generatedResults)
        {
            if (ExecuteResultGenerator)
            {
                generatedResults = generatedResults.OrderBy(x => x.Penalty).ToList();
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
                PrintResultToConsole(schedulerResult, equalsWithCheapest);
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
            Console.WriteLine($"Penalty:\t{algorithmResult.Penalty}");

            if (!algorithmResult.Feasible)
            {
                Console.WriteLine("Feasibility:\tINFEASIBLE");
            }

            if (algorithmResult.Duration != default)
            {
                Console.WriteLine($"Duration:\t{algorithmResult.Duration}");
            }

            if (additionalInformation.Length > 0)
            {
                foreach (var text in additionalInformation.Where(x => !string.IsNullOrEmpty(x)))
                {
                    Console.WriteLine(text);
                }
            }

            Console.WriteLine();
            algorithmResult.Result.PrintToConsole();

            if (algorithmResult.FeasibilityMessages?.Count > 0)
            {
                Console.WriteLine();

                foreach (var message in algorithmResult.FeasibilityMessages)
                {
                    Console.WriteLine(message);
                }
            }

            Console.WriteLine(separator);
        }

        private static bool CompareResults(SchedulingBenchmarkModel x, SchedulingBenchmarkModel y)
        {
            var yEmployees = y.Employees.ToDictionary(e => e.Id);

            foreach (var xEmployee in x.Employees)
            {
                var yEmployee = yEmployees[xEmployee.Id];

                if (xEmployee.Assignments.Count != yEmployee.Assignments.Count) return false;

                foreach (var xAssignment in xEmployee.Assignments.Values)
                {
                    if (!yEmployee.Assignments.TryGetValue(xAssignment.Day, out var yAssignment)) return false;
                    if (xAssignment.ShiftId != yAssignment.ShiftId) return false;
                }
            }

            return true;
        }
    }
}

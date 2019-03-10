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
    static class AlgorithmRunner
    {
        private static readonly bool ExecuteSchedulerAlgorithm = true;
        private static readonly bool ExecuteResultGenerator = false;
        private static readonly int InstanceNumber = 2;

        public static void RunPerf()
        {
            var dto = SchedulingBenchmarkInstanceReader.FromXml(InstanceNumber);
            var runCount = 0;
            var totalRunTime = TimeSpan.Zero;
            var totalGc0 = 0;
            var totalGc1 = 0;
            var totalGc2 = 0;

            while (true)
            {
                runCount++;

                var schedulingBenchmarkModel = DtoToSchedulingBenchmarkModelMapper.MapToSchedulingBenchmarkModel(dto);
                var sw = new Stopwatch();

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

                var gc0 = GC.CollectionCount(0);
                var gc1 = GC.CollectionCount(1);
                var gc2 = GC.CollectionCount(2);

                sw.Start();
                SchedulerAlgorithmRunner.Run(schedulingBenchmarkModel);
                sw.Stop();

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

                totalGc0 += (GC.CollectionCount(0) - gc0);
                totalGc1 += (GC.CollectionCount(1) - gc1);
                totalGc2 += (GC.CollectionCount(2) - gc2);

                totalRunTime += sw.Elapsed;

                Thread.Sleep(300);
                Console.Clear();
                Console.WriteLine($"Average running time:\t{totalRunTime / runCount}");
                Console.WriteLine($"Average GC0 collects:\t{totalGc0 / runCount}");
                Console.WriteLine($"Average GC1 collects:\t{totalGc1 / runCount}");
                Console.WriteLine($"Average GC2 collects:\t{totalGc2 / runCount}");
            }
        }

        public static void ShowResults()
        {
            AlgorithmResult schedulerResult = null;
            List<AlgorithmResult> generatedResults = null;

            if (ExecuteSchedulerAlgorithm) schedulerResult = RunSchedulerAlgorithm();
            if (ExecuteResultGenerator) generatedResults = RunResultGenerator();

            CompareAndPrintResults(schedulerResult, generatedResults);
            OnKeyDown(schedulerResult);
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

        private static void OnKeyDown(AlgorithmResult algorithmResult)
        {
            var key = Console.ReadKey();

            switch (key.Key)
            {
                case ConsoleKey.B:
                    DisplayBaseline(algorithmResult);
                    OnKeyDown(algorithmResult);
                    break;

                case ConsoleKey.S:
                    SaveAlgorithmResult(algorithmResult);
                    OnKeyDown(algorithmResult);
                    break;

                default:
                    break;
            }
        }

        private static void SaveAlgorithmResult(AlgorithmResult algorithmResult)
        {
            var originalName = algorithmResult.Name;
            algorithmResult.Name = "Baseline";
            BaselineStore.Save(InstanceNumber, algorithmResult);
            algorithmResult.Name = originalName;
            Console.WriteLine("Baseline saved");
        }

        private static bool _displayingBase = false;

        private static void DisplayBaseline(AlgorithmResult algorithmResult)
        {
            if (!_displayingBase)
            {
                var baseline = BaselineStore.Get(InstanceNumber);
                Console.Clear();
                PrintResultToConsole(baseline);
            }
            else
            {
                Console.Clear();
                PrintResultToConsole(algorithmResult);
            }

            _displayingBase = !_displayingBase;
        }
    }
}

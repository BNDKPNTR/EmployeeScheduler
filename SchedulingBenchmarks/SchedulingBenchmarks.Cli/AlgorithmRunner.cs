using Newtonsoft.Json;
using SchedulingBenchmarks.Evaluators;
using SchedulingBenchmarks.Mappers;
using SchedulingBenchmarks.SchedulingBenchmarksModel;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SchedulingBenchmarks.Cli
{
    static class AlgorithmRunner
    {
        private static readonly int InstanceNumber = 1;

        public static void ShowResults()
        {
            var schedulerResult = RunSchedulerAlgorithm();
            PrintResultToConsole(schedulerResult);
            OnKeyDown(schedulerResult);
        }

        public static void GenerateAllBaselines()
        {
            Parallel.For(1, 25, i =>
            {
                var result = RunSchedulerAlgorithm(i, copyToClipboard: false);
                result.Name = "Baseline";
                BaselineStore.Save(i, result);
            });
        }

        public static void GenerateAllExpectedTestResults()
        {
            Console.WriteLine("Generating expected test results based on current result of the algorithm");
            var solutionDir = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.Parent.Parent.FullName;
            var TestsProjDir = Path.Combine(solutionDir, "SchedulingBenchmarks.Tests");
            var outputDir = Path.Combine(TestsProjDir, "bin", "Debug", "netcoreapp2.0", "ExpectedTestResults");

            if (Directory.Exists(outputDir))
            {
                Directory.Delete(outputDir, recursive: true);

                // see https://stackoverflow.com/questions/35069311/why-sometimes-directory-createdirectory-fails
                var retryCount = 0;
                var maxRetryCount = 10;
                while (Directory.Exists(outputDir) && retryCount < maxRetryCount)
                {
                    Thread.Sleep(100);
                    retryCount++;
                }

                if (retryCount == maxRetryCount)
                {
                    throw new Exception($"Could not clean up output folder. Please delete it manually, then run again: '{outputDir}'");
                }
            }

            Directory.CreateDirectory(outputDir);

            Parallel.For(1, 25, i =>
            {
                var expectedTestResult = ExpectedTestResultsGenerator.RunForInstance(i);
                var path = Path.Combine(outputDir, $"{i}.json");
                File.WriteAllText(path, JsonConvert.SerializeObject(expectedTestResult));
                Console.WriteLine($"Instance {i} finished");
            });

            Console.WriteLine("... Done!");
        }

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

        private static AlgorithmResult RunSchedulerAlgorithm(int? instanceNumber = null, bool copyToClipboard = true)
        {
            var dto = SchedulingBenchmarkInstanceReader.FromXml(instanceNumber ?? InstanceNumber);
            var schedulingBenchmarkModel = DtoToSchedulingBenchmarkModelMapper.MapToSchedulingBenchmarkModel(dto);
            var sw = new Stopwatch();

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            sw.Start();
            var result = SchedulerAlgorithmRunner.Run(schedulingBenchmarkModel);
            sw.Stop();

            if (copyToClipboard)
            {
               Clipboard.Copy(result.ToRosterViewerFormat()); 
            }

            var (feasible, messages) = FeasibilityEvaluator.Evaluate(result);

            messages.Sort();

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

        private static void PrintResultToConsole(AlgorithmResult algorithmResult)
        {
            var separator = new string('-', 80);

            Console.WriteLine(separator);
            Console.WriteLine($"Name:\t\t{algorithmResult.Name}");
            Console.WriteLine($"Penalty:\t{algorithmResult.Penalty}");
            Console.WriteLine($"Feasibility:\t{(algorithmResult.Feasible ? "feasible" : "INFEASIBLE")}");

            if (algorithmResult.Duration != default)
            {
                Console.WriteLine($"Duration:\t{algorithmResult.Duration}");
            }

            Console.WriteLine();
            algorithmResult.Result.PrintToConsole();
            Console.WriteLine();

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

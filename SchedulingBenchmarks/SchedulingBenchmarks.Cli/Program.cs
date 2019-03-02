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
            var dto = SchedulingBenchmarkInstanceReader.FromXml(instanceNumber: 1);
            var schedulingBenchmarkModel = DtoToSchedulingBenchmarkModelMapper.MapToSchedulingBenchmarkModel(dto);
            var sw = new Stopwatch();

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            sw.Start();
            var result = SchedulerAlgorithmRunner.Run(schedulingBenchmarkModel);
            sw.Stop();

            Clipboard.Copy(ToRosterViewerFormat(result));

            var (feasible, messages) = FeasibilityEvaluator.Feasible(result);

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

        private static string ToRosterViewerFormat(SchedulingBenchmarkModel schedulingBenchmarkModel)
        {
            var builder = new StringBuilder();

            for (var i = 0; i < schedulingBenchmarkModel.Employees.Length; i++)
            {
                var person = schedulingBenchmarkModel.Employees[i];

                foreach (var day in Enumerable.Range(0, schedulingBenchmarkModel.Duration))
                {
                    if (person.Assignments.TryGetValue(day, out var assignment))
                    {
                        builder.Append($"{assignment.ShiftId}\t");
                    }
                    else
                    {
                        builder.Append("\t");
                    }
                }

                if (i < schedulingBenchmarkModel.Employees.Length - 1)
                {
                    builder.AppendLine();
                }
            }

            return builder.ToString();
        }

        private static List<AlgorithmResult> RunResultGenerator()
        {
            var generator = new ScheduleResultGenerator(() => null);
            var feasibleResults = new ConcurrentBag<AlgorithmResult>();
            var resultNumber = 0;
            
            Parallel.ForEach(generator.GenerateResults(), result =>
            {
                //if (FeasibilityEvaluator.Feasible(result))
                //{
                //    feasibleResults.Add(new AlgorithmResult
                //    {
                //        Name = $"Timetable {Interlocked.Increment(ref resultNumber)}",
                //        Result = result,
                //        Cost = OptimalityEvaluator.CalculateCost(result),
                //        Feasible = true
                //    });
                //}
            });

            return feasibleResults.ToList();
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
            Console.WriteLine(CreateFormattedString(algorithmResult.Result));

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

        private static string CreateFormattedString(SchedulingBenchmarkModel schedulingBenchmarkModel)
        {
            var shiftLengths = schedulingBenchmarkModel.Shifts.ToDictionary(s => s.Id, s => s.Duration);
            var builder = new StringBuilder();

            var personRowWidth = 3;

            builder.Append(new string(' ', personRowWidth));
            foreach (var day in Enumerable.Range(0, schedulingBenchmarkModel.Duration))
            {
                builder.Append(day < 10 ? $"{day}  " : $"{day} ");
            }
            builder.AppendLine();
            builder.AppendLine($"   {new string('_', schedulingBenchmarkModel.Duration * 3)}");

            var assignmentsOnDays = new int[schedulingBenchmarkModel.Duration];

            foreach (var person in schedulingBenchmarkModel.Employees)
            {
                builder.Append($"{person.Id} |");

                foreach (var day in Enumerable.Range(0, schedulingBenchmarkModel.Duration))
                {
                    if (person.Assignments.TryGetValue(day, out var assignment))
                    {
                        builder.Append($"{assignment.ShiftId}  ");
                        assignmentsOnDays[day]++;
                    }
                    else
                    {
                        builder.Append("   ");
                    }
                }

                var workedMinutes = person.Assignments.Values.Sum(a => shiftLengths[a.ShiftId].TotalMinutes);
                var formattedWorkedMinutes = string.Empty;
                if (person.Contract.MinTotalWorkTime <= workedMinutes && workedMinutes <= person.Contract.MaxTotalWorkTime)
                {
                    formattedWorkedMinutes = "0";
                }
                else if (workedMinutes < person.Contract.MinTotalWorkTime)
                {
                    formattedWorkedMinutes = $"-{person.Contract.MinTotalWorkTime - workedMinutes}";
                }
                else if (workedMinutes > person.Contract.MaxTotalWorkTime)
                {
                    formattedWorkedMinutes = $"+{workedMinutes - person.Contract.MaxTotalWorkTime}";
                }

                builder.Append($"| {formattedWorkedMinutes}");

                builder.AppendLine();
            }

            builder.AppendLine($"  |{new string('_', schedulingBenchmarkModel.Duration * 3)}|");
            builder.Append(new string(' ', personRowWidth));
            foreach (var day in Enumerable.Range(0, schedulingBenchmarkModel.Duration))
            {
                var infeasibleDemandCount = schedulingBenchmarkModel.Demands[day].Sum(d => d.MinEmployeeCount) - assignmentsOnDays[day];
                builder.Append(infeasibleDemandCount < 10 ? $"{infeasibleDemandCount}  " : $"{infeasibleDemandCount} ");
            }

            return builder.ToString();
        }

        private static bool CompareResults(SchedulingBenchmarkModel x, SchedulingBenchmarkModel y)
        {
            throw new NotImplementedException();
        }

        private class AlgorithmResult
        {
            public string Name { get; set; }
            public SchedulingBenchmarkModel Result { get; set; }
            public int Penalty { get; set; }
            public bool Feasible { get; set; }
            public List<string> FeasibilityMessages { get; set; }
            public TimeSpan Duration { get; set; }
        }
    }
}

using Scheduler.Dto;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scheduler
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
            var input = InputGenerator.CreateInput();
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
                Cost = ResultCostCalculator.CalculateCost(result),
                Feasible = ResultEvaluator.Evaluate(result),
                Duration = sw.Elapsed
            };
        }

        private static List<AlgorithmResult> RunResultGenerator()
        {
            var generator = new ScheduleResultGenerator(() => InputGenerator.CreateInput());
            var feasibleResults = new ConcurrentBag<AlgorithmResult>();
            var resultNumber = 1;
            
            Parallel.ForEach(generator.GenerateResults(), result =>
            {
                if (ResultEvaluator.Evaluate(result))
                {
                    feasibleResults.Add(new AlgorithmResult
                    {
                        Name = $"Timetable {Interlocked.Increment(ref resultNumber)}",
                        Result = result,
                        Cost = ResultCostCalculator.CalculateCost(result),
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

        private static string CreateFormattedString(InputModel result)
        {
            const string dateFormat = "yyyy.MM.dd HH:mm";
            var builder = new StringBuilder();

            foreach (var person in result.People)
            {
                builder.AppendLine(person.Name);

                foreach (var assignment in person.Assignments)
                {
                    builder.AppendLine($"\t{assignment.Start.ToString(dateFormat)} - {assignment.End.ToString(dateFormat)}: {assignment.Activity.Name}");
                }

                builder.AppendLine();
            }

            return builder.ToString();
        }

        private static bool CompareResults(InputModel x, InputModel y)
        {
            var assignmentComparer = EqualityComparer.Create<Assignment>((a, b) =>
                a.Start == b.Start
                && a.End == b.End
                && a.Activity.Id == b.Activity.Id
                && a.Person.Id == b.Person.Id);

            var personCount = x.People.Count;
            var equal = true;

            Parallel.For(0, personCount, (i, state) =>
            {
                var a = x.People[i].Assignments;
                var b = y.People[i].Assignments;

                if (a.Count != b.Count)
                {
                    equal = false;
                    state.Break();
                }

                var assignments = new HashSet<Assignment>(a, assignmentComparer);

                if (assignments.Count != b.Count)
                {
                    equal = false;
                    state.Break();
                }

                foreach (var assignment in b)
                {
                    if (assignments.Contains(assignment))
                    {
                        assignments.Remove(assignment);
                    }
                    else
                    {
                        equal = false;
                        state.Break();
                    }
                }
            });

            return equal;
        }

        private class AlgorithmResult
        {
            public string Name { get; set; }
            public InputModel Result { get; set; }
            public int Cost { get; set; }
            public bool Feasible { get; set; }
            public TimeSpan Duration { get; set; }
        }
    }
}

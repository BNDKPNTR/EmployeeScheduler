using SchedulingBenchmarks.SchedulingBenchmarksModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchedulingBenchmarks.Cli
{
    static class ScheduleBenchmarkModelExtensions
    {
        public static void PrintToConsole(this SchedulingBenchmarkModel schedulingBenchmarkModel)
            => new ScheduleBenchmarkModelConsolePrinter(schedulingBenchmarkModel).Print();

        public static string ToFormattedString(this SchedulingBenchmarkModel schedulingBenchmarkModel)
        {
            var printer = new ScheduleBenchmarkModelStringPrinter(schedulingBenchmarkModel);

            printer.Print();

            return printer.ToString();
        }

        public static string ToRosterViewerFormat(this SchedulingBenchmarkModel schedulingBenchmarkModel)
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
    }
}

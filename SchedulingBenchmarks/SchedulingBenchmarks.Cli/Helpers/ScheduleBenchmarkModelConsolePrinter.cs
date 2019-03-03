using SchedulingBenchmarks.SchedulingBenchmarksModel;
using System;

namespace SchedulingBenchmarks.Cli
{
    class ScheduleBenchmarkModelConsolePrinter : ScheduleBenchmarkModelPrinterBase
    {
        private readonly ConsoleColor _originalColor;

        public ScheduleBenchmarkModelConsolePrinter(SchedulingBenchmarkModel schedulingBenchmarkModel) : base(schedulingBenchmarkModel)
        {
            _originalColor = Console.BackgroundColor;
        }

        protected override void Write(string value) => Console.Write(value);

        protected override void BeforeWeekendPrintStart() => Console.BackgroundColor = ConsoleColor.DarkGray;

        protected override void AfterWeekendPrintEnd() => Console.BackgroundColor = _originalColor;
    }
}

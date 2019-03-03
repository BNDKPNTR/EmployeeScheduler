using SchedulingBenchmarks.SchedulingBenchmarksModel;
using System.Text;

namespace SchedulingBenchmarks.Cli
{
    class ScheduleBenchmarkModelStringPrinter : ScheduleBenchmarkModelPrinterBase
    {
        private readonly StringBuilder _builder;

        public ScheduleBenchmarkModelStringPrinter(SchedulingBenchmarkModel schedulingBenchmarkModel) : base(schedulingBenchmarkModel)
        {
            _builder = new StringBuilder();
        }

        protected override void Write(string value) => _builder.Append(value);

        public override string ToString() => _builder.ToString();
    }
}

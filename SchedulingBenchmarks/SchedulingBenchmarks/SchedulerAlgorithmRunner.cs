using SchedulingBenchmarks.Mappers;
using SchedulingBenchmarks.Models;
using SchedulingBenchmarks.SchedulingBenchmarksModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SchedulingBenchmarks
{
    public static class SchedulerAlgorithmRunner
    {
        public static readonly Stopwatch _sw = new Stopwatch();
        public static long _elapsed = 0;
        public static long _count = 0;
        public static long _total = 0;

        public static SchedulingBenchmarkModel Run(SchedulingBenchmarkModel schedulingBenchmarkModel)
        {
            var schedulerModel = SchedulingBenchmarkModelToSchedulerModelMapper.MapToScheduleModel(schedulingBenchmarkModel);
            var algorithm = new SchedulerAlgorithm(schedulerModel);

            var sw = Stopwatch.StartNew();
            algorithm.Run();
            _total = sw.ElapsedTicks;

            return SchedulerModelToSchedulingBenchmarkModelMapper.MapToSchedulingBenchmarkModel(schedulerModel, schedulingBenchmarkModel);
        }

        public static void Reset()
        {
            _sw.Reset();
            _elapsed = 0;
            _count = 0;
            _total = 0;
        }
    }
}

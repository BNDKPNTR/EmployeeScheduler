using SchedulingBenchmarks.Mappers;
using SchedulingBenchmarks.Models;
using SchedulingBenchmarks.SchedulingBenchmarksModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchedulingBenchmarks
{
    public static class SchedulerAlgorithmRunner
    {
        public static SchedulingBenchmarkModel Run(SchedulingBenchmarkModel schedulingBenchmarkModel)
        {
            var schedulerModel = SchedulingBenchmarkModelToSchedulerModelMapper.MapToScheduleModel(schedulingBenchmarkModel);
            var algorithm = new SchedulerAlgorithm(schedulerModel);

            algorithm.Run();

            return SchedulerModelToSchedulingBenchmarkModelMapper.MapToSchedulingBenchmarkModel(schedulerModel, schedulingBenchmarkModel);
        }
    }
}

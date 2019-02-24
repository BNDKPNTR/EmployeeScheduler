using SchedulingBenchmarks.Dto;
using SchedulingBenchmarks.Mappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulingBenchmarks
{
    public static class SchedulerAlgorithmRunner
    {
        public static SchedulingPeriod Run(SchedulingPeriod schedulingBenchmarkModel)
        {
            var schedulerModel = SchedulingPeriodMapper.MapToScheduleModel(schedulingBenchmarkModel);
            var algorithm = new SchedulerAlgorithm(schedulerModel);

            algorithm.Run();

            return SchedulerModelMapper.MapToSchedulingBenchmarkModel(schedulerModel, schedulingBenchmarkModel);
        }
    }
}

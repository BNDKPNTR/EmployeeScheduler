using SchedulingBenchmarks.Dto;
using SchedulingBenchmarks.Mappers;
using SchedulingBenchmarks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

            Console.WriteLine(AssignmentsToString(schedulerModel));

            return schedulingBenchmarkModel;
            //return SchedulerModelMapper.MapToSchedulingBenchmarkModel(schedulerModel, schedulingBenchmarkModel);
        }

        private static string AssignmentsToString(SchedulerModel schedulerModel)
        {
            var builder = new StringBuilder();

            var personRowWidth = 3;

            builder.Append(new string(' ', personRowWidth));
            foreach (var day in schedulerModel.SchedulePeriod)
            {
                builder.Append(day < 10 ? $"{day}  " : $"{day} ");
            }
            builder.AppendLine();

            foreach (var person in schedulerModel.People)
            {
                builder.Append($"{person.Id}: ");

                foreach (var day in schedulerModel.SchedulePeriod)
                {
                    if (person.Assignments.TryGetValue(day, out var assignment))
                    {
                        builder.Append($"{assignment.ShiftId}  ");
                    }
                    else
                    {
                        builder.Append("   ");
                    }
                }

                builder.AppendLine();
            }

            return builder.ToString();
        }
    }
}

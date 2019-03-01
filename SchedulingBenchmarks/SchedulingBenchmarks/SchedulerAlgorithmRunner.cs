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

            Console.WriteLine(AssignmentsToString(schedulerModel));

            var res = ToRosterViewerFormat(schedulerModel);

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
            builder.AppendLine($"   {new string('_', schedulerModel.SchedulePeriod.Length * 3)}");

            var assignmentsOnDays = new int[schedulerModel.SchedulePeriod.Length];

            foreach (var person in schedulerModel.People)
            {
                builder.Append($"{person.Id} |");

                foreach (var day in schedulerModel.SchedulePeriod)
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

                var workedMinutes = person.Assignments.Count * WorkSchedule.ShiftLengthInMinutes;
                var formattedWorkedMinutes = string.Empty;
                if (person.WorkSchedule.MinTotalWorkTime <= workedMinutes && workedMinutes <= person.WorkSchedule.MaxTotalWorkTime)
                {
                    formattedWorkedMinutes = "0";
                }
                else if (workedMinutes < person.WorkSchedule.MinTotalWorkTime)
                {
                    formattedWorkedMinutes = $"-{person.WorkSchedule.MinTotalWorkTime - workedMinutes}";
                }
                else if (workedMinutes > person.WorkSchedule.MaxTotalWorkTime)
                {
                    formattedWorkedMinutes = $"+{workedMinutes - person.WorkSchedule.MaxTotalWorkTime}";
                }

                builder.Append($"| {formattedWorkedMinutes}");

                builder.AppendLine();
            }

            builder.AppendLine($"  |{new string('_', schedulerModel.SchedulePeriod.Length * 3)}|");
            builder.Append(new string(' ', personRowWidth));
            foreach (var day in schedulerModel.SchedulePeriod)
            {
                var infeasibleDemandCount = schedulerModel.Demands[day].Sum(d => d.MinPeopleCount) - assignmentsOnDays[day];
                builder.Append(infeasibleDemandCount < 10 ? $"{infeasibleDemandCount}  " : $"{infeasibleDemandCount} ");
            }

            return builder.ToString();
        }

        private static string ToRosterViewerFormat(SchedulerModel schedulerModel)
        {
            var builder = new StringBuilder();

            foreach (var person in schedulerModel.People)
            {
                foreach (var day in schedulerModel.SchedulePeriod)
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

                builder.AppendLine();
            }

            return builder.ToString();
        }
    }
}

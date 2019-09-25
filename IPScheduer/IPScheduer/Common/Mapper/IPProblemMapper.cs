using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Google.OrTools.LinearSolver;
using IPScheduler.Models;
using IPScheduler.Inputs;
using System.Diagnostics;

namespace IPScheduler.Common.Mapper
{
    public class IpProblemMapper
    {


        private SchedulingIpContext scheduleContext = new SchedulingIpContext();

        public SchedulingIpContext MapToSolver(SchedulingPeriod schedulingPeriod)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            scheduleContext = SchedulingPeriodMapper.Map(schedulingPeriod);

            ConstraintMapper.CreateConstraints(scheduleContext);

            ObjectiveMapper.MapObjectives(scheduleContext);
            stopwatch.Stop();
            Console.WriteLine("Mapping time :" + stopwatch.ElapsedMilliseconds);
            Console.WriteLine($"ContraintNum: " + scheduleContext.Solver.NumConstraints());
            Console.WriteLine($"VariableNUm: " + scheduleContext.Solver.NumVariables());
            return scheduleContext;
        }
    }
}

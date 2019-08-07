using Google.OrTools.LinearSolver;
using SchedulingIP.Input;
using System;
using System.Collections.Generic;

namespace IPScheduer.Common
{
    public class SchedulingIpModel
    {
        public SchedulingPeriod Input { get; set; }
        public Solver Solver { get; } = new Solver("solver" + DateTime.Now.ToString(), Solver.OptimizationProblemType.BOP_INTEGER_PROGRAMMING);
        public List<Variable> Variables { get; } = new List<Variable>();
        public int GraphEdges { get; internal set; }
        public int GraphStartsAt { get; internal set; }
    }
}


using System;
using System.Collections.Generic;
using System.Globalization;
using Google.OrTools.LinearSolver;
using IPScheduler.Models;

namespace IPScheduler.Common
{
    public class SchedulingIpContext
    {
        public List<Person> Persons { get; set; } = new List<Person>();
        public List<Shift> Shifts { get; set; } = new List<Shift>();

        public List<Assignment> Assignments { get; set; } = new List<Assignment>();
        
        
        public Solver Solver { get; } = new Solver("solver" + DateTime.Now.ToString(CultureInfo.InvariantCulture), Solver.OptimizationProblemType.BOP_INTEGER_PROGRAMMING);
        public List<Variable> Variables { get; } = new List<Variable>();
        public int GraphEdges { get; internal set; }
        public int GraphStartsAt { get; internal set; }
        public int PersonCount { get; set; }


        public void RunAlgo()
        {
            Solver.Maximize(new LinearExpr());
            var resultStatus = Solver.Solve();

            SchedulingResultGraph resultGraph = SchedulingResultGraph.Create(Assignments);

          resultGraph.WriteToConsole();

            Console.WriteLine(Solver.Objective().Value());
            
            
            
            
        }
    }
}
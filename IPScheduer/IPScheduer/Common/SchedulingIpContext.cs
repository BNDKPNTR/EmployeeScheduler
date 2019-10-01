using System;
using System.Collections.Generic;
using System.Globalization;
using Google.OrTools.LinearSolver;
using IPScheduler.Models;

namespace IPScheduler.Common
{
    public class SchedulingIpContext
    {
        public Dictionary<string, Person> Persons { get; set; } = new Dictionary<string, Person>();
        public List<Shift> Shifts { get; set; } = new List<Shift>();

        public List<Assignment> Assignments { get; set; } = new List<Assignment>();
        
        
        public Solver Solver { get; } = new Solver("solver" + DateTime.Now.ToString(CultureInfo.InvariantCulture), Solver.OptimizationProblemType.BOP_INTEGER_PROGRAMMING);
        public List<Variable> Variables { get; } = new List<Variable>();
        public int GraphEdges { get; internal set; }
        public int GraphStartsAt { get; internal set; }
        public int PersonCount { get; set; }
        public Dictionary<string, ShiftType> ShiftTypeDicitonary { get; set; } = new Dictionary<string, ShiftType>();
        public Dictionary<string,SchedulingContract> ContractDictionary { get; } = new Dictionary<string, SchedulingContract>();
        public int DayCount { get; set; }
        public Dictionary<string, Dictionary<int,Variable>> WeekEndVariables { get; set; } = new Dictionary<string, Dictionary<int,Variable>>();
        public int WeekCount { get; set; }


        public void RunAlgo()
        {
         //   Console.WriteLine(Solver.NumConstraints());


            var resultStatus = Solver.Solve();

            SchedulingResultGraph resultGraph = SchedulingResultGraph.Create(Assignments);

//          resultGraph.WriteToConsole();

            Console.WriteLine(Solver.Objective().Value());
        }
    }
}
using System;
using System.Diagnostics;
using Google.OrTools.LinearSolver;
using IPScheduler.Common;

namespace IPScheduler
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch  =new Stopwatch();
            stopwatch.Start();
            //SolverTryout();


            var input = XmlReader.ReadInstance(20);
          // var input = XmlReader.ReadInputFrom(@"testInstance.xml");

            IpProblemMapper mapper = new IpProblemMapper();
            
               var s = mapper.MapToSolver(input);
               
               s.RunAlgo();

               var resgraph = SchedulingResultGraph.Create(s.Assignments);

               resgraph.WriteToConsole();
               var result = SchedulingResultGraph.ToRosterViewerFormat(s);
               Clipboard.Copy(result);

               Console.WriteLine($"Objective: {s.Solver.Objective().Value()}");
               stopwatch.Stop();
               Console.WriteLine($"elasped: {stopwatch.ElapsedMilliseconds}");
            ;

               //
//            s.Solver.Maximize(s.Variables[0]);
//            Solver.ResultStatus resultStatus = s.Solver.Solve();
//
//            Console.WriteLine(s.Solver.Objective().Value());


        }

        private static void SolverTryout()
        {
            Solver s = new Solver("trysolver", Solver.OptimizationProblemType.BOP_INTEGER_PROGRAMMING);

            Variable x = s.MakeIntVar(0.0, 2, "x");
            Variable y = s.MakeIntVar(0.0, 2, "y");

            Console.WriteLine(s.NumVariables());

            s.Add(10 * x + 4 * y >= 20);
            s.Add(5 * x + 5 * y >= 20);
            s.Add(2 * x + 6 * y >= 12);

            Console.WriteLine(s.NumConstraints());

            s.Minimize(1100 * x + 1000 * y);

            Solver.ResultStatus resultStatus = s.Solve();

            if (!resultStatus.Equals(Solver.ResultStatus.OPTIMAL))
            {
                Console.WriteLine("error");
                return;
            }

            Console.WriteLine("Solution:");

            Console.WriteLine("value: " + s.Objective().Value());

            Console.WriteLine("x: " + x.SolutionValue());
            Console.WriteLine("y: " + y.SolutionValue());
        }
    }
}

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
            long min = long.MaxValue, max = long.MinValue, average = 0, sum = 0;
            int LOOPMAX = 1;
            for (var i = 0; i < LOOPMAX; i++)
            {
                long runtime =RunInstance();
                if (runtime > max)
                    max = runtime;
                if(runtime < min)
                    min = runtime;
                sum += runtime;
            }

            average = sum / LOOPMAX;
            Console.WriteLine($"avg: {average}");
            Console.WriteLine($"max: {max}");
            Console.WriteLine($"min: {min}");


        }

        private static long RunInstance()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var input = XmlReader.ReadInstance(13);


            IpProblemMapper mapper = new IpProblemMapper();

            var s = mapper.MapToSolver(input);

            s.RunAlgo();
            stopwatch.Stop();
            var elaspedMilisecs = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"elasped: {elaspedMilisecs}");

            var result = SchedulingResultGraph.ToRosterViewerFormat(s);
            Clipboard.Copy(result);
            Console.WriteLine("Constraint Count:" + s.Solver.NumConstraints());
            Console.WriteLine($"Objective: {s.Solver.Objective().Value()}");

            return elaspedMilisecs;
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

            Console.WriteLine("Constraint Count:" + s.NumConstraints());

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

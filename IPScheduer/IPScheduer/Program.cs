using System;
using System.Diagnostics;
using IPScheduler.Common;
using IPScheduler.Common.Mapper;

namespace IPScheduler
{
    class Program
    {
        public static int INSTANCENUM = 8;
        static void Main(string[] args)
        {
            long min = long.MaxValue, max = long.MinValue, average = 0, sum = 0;
            int LOOPMAX = 1;
            for (var i = 0; i < LOOPMAX; i++)
            {
                long runtime = RunInstance();
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
            Console.WriteLine("DONE");
        }

        private static long RunInstance()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var input = XmlReader.ReadInstance(INSTANCENUM);

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
    }
}

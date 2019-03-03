using SchedulingBenchmarks.Dto;
using SchedulingBenchmarks.Mappers;
using SchedulingBenchmarks.SchedulingBenchmarksModel;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SchedulingBenchmarks.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            AlgorithmRunner.ShowResults();
        }
    }
}

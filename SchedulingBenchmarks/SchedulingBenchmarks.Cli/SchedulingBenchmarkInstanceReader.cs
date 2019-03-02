using SchedulingBenchmarks.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace SchedulingBenchmarks.Cli
{
    public static class SchedulingBenchmarkInstanceReader
    {
        public static SchedulingPeriod FromXml(int instanceNumber)
        {
            var assemblyPath = Assembly.GetAssembly(typeof(Program)).Location;
            var assemblyDir = Path.GetDirectoryName(assemblyPath);
            var instancePath = Path.Combine(assemblyDir, "Instances", $"Instance{instanceNumber}.ros");

            var serializer = new XmlSerializer(typeof(SchedulingPeriod));

            using (var stream = File.OpenRead(instancePath))
            {
                return (SchedulingPeriod)serializer.Deserialize(stream);
            }
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace SchedulingBenchmarks.Cli
{
    static class BaselineStore
    {
        private const string FileNameTemplate = "{0}.json";

        private static readonly string _baselinesDirPath;
        private static readonly JsonSerializer _serializer;

        static BaselineStore()
        {
            var assemblyPath = Assembly.GetAssembly(typeof(Program)).Location;
            var assemblyDir = Path.GetDirectoryName(assemblyPath);
            _baselinesDirPath = Path.Combine(assemblyDir, "Baselines");

            _serializer = JsonSerializer.CreateDefault();
        }

        public static void Save(int instanceNumber, AlgorithmResult algorithmResult)
        {
            EnsureBaselineDirExists();

            var filePath = Path.Combine(_baselinesDirPath, string.Format(FileNameTemplate, instanceNumber));

            using (var stream = File.Create(filePath))
            using (var writer = new StreamWriter(stream))
            {
                _serializer.Serialize(writer, algorithmResult);
            }
        }

        public static AlgorithmResult Get(int instanceNumber)
        {
            var filePath = Path.Combine(_baselinesDirPath, string.Format(FileNameTemplate, instanceNumber));

            using (var stream = File.OpenRead(filePath))
            using (var reader = new StreamReader(stream))
            using (var jsonReader = new JsonTextReader(reader))
            {
                return _serializer.Deserialize<AlgorithmResult>(jsonReader);
            }
        }

        private static void EnsureBaselineDirExists()
        {
            if (!Directory.Exists(_baselinesDirPath))
            {
                Directory.CreateDirectory(_baselinesDirPath);
            }
        }
    }
}

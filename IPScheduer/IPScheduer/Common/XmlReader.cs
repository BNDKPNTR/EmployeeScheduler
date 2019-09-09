using System.IO;
using System.Xml.Serialization;
using IPScheduler.Inputs;

namespace IPScheduler.Common
{
    public class XmlReader
    {
        public static SchedulingPeriod ReadInputFrom(string filePath)
        {
            SchedulingPeriod schedulingPeriod;

            using (var fileStream = new FileStream(filePath, FileMode.Open))
                schedulingPeriod = new XmlSerializer(typeof(SchedulingPeriod)).Deserialize(fileStream) as SchedulingPeriod;

            return schedulingPeriod ?? throw new IOException($"File : {filePath} could not be serialized");
        }

        public static SchedulingPeriod ReadInstance(int instanceNumber)
            => ReadInputFrom(@"../../../Inputs/Instance" + instanceNumber + ".xml");

    }
}

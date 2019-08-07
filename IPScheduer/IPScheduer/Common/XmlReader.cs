using SchedulingIP.Input;
using System.IO;
using System.Xml.Serialization;

namespace Scheduling.Common
{
    public class XmlReader
    {
        public static SchedulingPeriod ReadInputFrom(string path)
        {
            XmlSerializer dser = new XmlSerializer(typeof(SchedulingPeriod));

            SchedulingPeriod sp;

            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                sp = dser.Deserialize(fs) as SchedulingPeriod;
            }

            if (sp == null)
                throw new IOException($"File : {path} could not be serialized");

            return sp;
        }

        public static SchedulingPeriod ReadInstance(int instanceNumber)
            => ReadInputFrom(@"../../../Inputs/Instance" + instanceNumber + ".xml");



    }
}

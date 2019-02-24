using System;
using System.ComponentModel;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace SchedulingBenchmarks.Dto
{
    public class Shift : IXmlSerializable
    {
        public const string EmptyShiftId = "-";

        [XmlAttribute("ID")]
        public string Id { get; set; }
        
        [XmlIgnore]
        public TimeSpan StartTime { get; set; }

        public int Duration { get; set; }

        public XmlSchema GetSchema() => throw new NotImplementedException();

        public void ReadXml(XmlReader reader)
        {
            Id = reader.GetAttribute("ID");

            var readStartTime = false;
            var readDuration = false;

            while (reader.Read() && !(readStartTime && readDuration))
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name == "StartTime")
                    {
                        var startTimeString = reader.ReadElementContentAsString();
                        var components = startTimeString.Split(':');
                        StartTime = new TimeSpan(int.Parse(components[0]), int.Parse(components[1]), 0);
                        readStartTime = true;
                    }

                    if (reader.Name == "Duration")
                    {
                        Duration = reader.ReadElementContentAsInt();
                        readDuration = true;
                    }
                }
            }
        }

        public void WriteXml(XmlWriter writer) => throw new NotImplementedException();
    }
}

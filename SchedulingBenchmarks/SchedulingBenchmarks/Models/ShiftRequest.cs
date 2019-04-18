using System;

namespace SchedulingBenchmarks.Models
{
    public enum RequestType { On, Off }

    public class ShiftRequest
    {
        public string ShiftId { get; }
        public int Weight { get; }
        public RequestType Type { get; }

        public ShiftRequest(string shiftId, int weight, RequestType type)
        {
            ShiftId = shiftId ?? throw new ArgumentNullException(nameof(shiftId));
            Weight = weight;
            Type = type;
        }
    }
}

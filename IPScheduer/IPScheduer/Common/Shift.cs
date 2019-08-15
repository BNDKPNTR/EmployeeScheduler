namespace IPScheduler.Common
{
    public class Shift
    {
        public Shift(string id)
        {
            ID = id;
        }

        public string ID { get; }
        public object Name { get; }
    }
}
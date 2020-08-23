namespace NitroxModel.DataStructures
{
    public class PriorityChain<T>
    {
        public PriorityChain(int priority)
        {
            Priority = priority;
        }

        public int Priority { get; set; }
        public int Count { get; set; }
        public PriorityItem<T> Head { get; set; }
        public PriorityItem<T> Tail { get; set; }
    }
}

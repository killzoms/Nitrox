namespace NitroxModel.DataStructures
{
    public class PriorityItem<T>
    {
        public PriorityItem(T data)
        {
            Data = data;
        }

        public T Data { get; }
        public bool IsQueued => Chain != null;
        public int Priority => Chain.Priority;

        internal PriorityItem<T> SequentialPrev { get; set; }
        internal PriorityItem<T> SequentialNext { get; set; }

        internal PriorityChain<T> Chain { get; set; }
        internal PriorityItem<T> PriorityPrev { get; set; }
        internal PriorityItem<T> PriorityNext { get; set; }
    }
}

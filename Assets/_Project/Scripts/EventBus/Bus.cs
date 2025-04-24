namespace Assets._Project.Scripts.EventBus
{
    public static class Bus<T> where T : IEvent
    {
        public delegate void Event(T evnt);

        public static event Event OnEvent;

        public static void Raise(T evnt) => OnEvent?.Invoke(evnt);
    }
}
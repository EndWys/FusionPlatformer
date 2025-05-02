using UnityEngine;

namespace Assets._Project.Scripts.EventBus
{
    public interface IEvent { }

    public struct CoinsCountChangeEvent : IEvent
    {
        public int Count;
    }

    public struct SomeoneWinEvent: IEvent
    {
        public string WinnerName;
    }

    public struct MatchReloadEvent : IEvent { }

    public struct LevelRunnerFalloutEvent : IEvent
    {
        public Vector3 Posiotion;
    }

    public struct CoinDisapearEvent : IEvent
    {
        public Vector3 Posiotion;
    }

    public struct CloudDisapearEvent : IEvent
    {
        public Vector3 Posiotion;
    }

    public struct PlatformFallEvent : IEvent
    {
        public Vector3 Posiotion;
    }

    public struct CheckpointReachEvent : IEvent
    {
        public Vector3 Posiotion;
    }

    public struct CrownReachEvent : IEvent
    {
        public Vector3 Posiotion;
    }
}
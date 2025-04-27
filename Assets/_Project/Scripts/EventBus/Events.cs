using UnityEngine;

namespace Assets._Project.Scripts.EventBus
{
    public interface IEvent { }

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
}
using UnityEngine;
using UnityEngine.Windows;

namespace Assets._Project.Scripts.Player.PlayerInput
{
    public struct GameplayInput
    {
        public Vector2 LookRotation;
        public Vector2 MoveDirection;
        public bool Jump;
        public bool Sprint;
    }

    public abstract class BasePlayerInputHandler : MonoBehaviour
    {
        public abstract GameplayInput CurrentInput { get; }
        public abstract void ResetInput();
    }
}
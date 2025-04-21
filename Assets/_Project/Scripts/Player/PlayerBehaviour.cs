using Fusion;
using UnityEngine;

namespace Assets._Project.Scripts.Player
{
    public class PlayerBehaviour : NetworkBehaviour
    {
        [SerializeField] private PlayerMovement _movement;

        [Networked, HideInInspector, Capacity(24)]
        public string Nickname { get; set; }

        public void Respawn(Vector3 position, bool resetCoins)
        {
            _movement.Respawn(position);
        }
    }
}
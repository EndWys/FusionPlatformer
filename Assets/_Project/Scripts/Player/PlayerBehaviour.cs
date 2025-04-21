using Fusion;
using UnityEngine;

namespace Assets._Project.Scripts.Player
{
    public class PlayerBehaviour : NetworkBehaviour
    {
        [SerializeField] private PlayerMovement _movement;

        public void Respawn(Vector3 position, bool resetCoins)
        {
            _movement.Respawn(position);
        }
    }
}
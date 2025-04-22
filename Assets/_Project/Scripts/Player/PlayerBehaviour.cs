using Assets._Project.Scripts.Gameplay.LevelObjects;
using Fusion;
using UnityEngine;

namespace Assets._Project.Scripts.Player
{
    public class PlayerBehaviour : NetworkBehaviour
    {
        [SerializeField] private PlayerMovement _movement;

        [Networked]
        public int CollectedCoins { get; set; }

        [Networked, HideInInspector, Capacity(24)]
        public string Nickname { get; set; }

        public void Respawn(Vector3 position, bool resetCoins)
        {
            _movement.Respawn(position);
        }

        private void OnTriggerEnter(Collider other)
        {
            //Coins collecting
            if (HasStateAuthority == false)
                return;

            if (other.TryGetComponent(out CoinBehavour coin))
            {
                CollectedCoins++;
                coin.Collecting();
            }
        }
    }
}
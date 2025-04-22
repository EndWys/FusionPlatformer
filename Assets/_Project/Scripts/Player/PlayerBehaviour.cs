using Assets._Project.Scripts.Gameplay;
using Assets._Project.Scripts.Gameplay.LevelObjects;
using Assets._Project.Scripts.NetworkConnction;
using Fusion;
using UnityEngine;

namespace Assets._Project.Scripts.Player
{
    public class PlayerBehaviour : NetworkBehaviour
    {
        [SerializeField] private PlayerMovement _movement;

        [Networked, OnChangedRender(nameof(OnCoinsAmountChanged))] private int _collectedCoins { get; set; }

        public int CollectedCoins => _collectedCoins;

        private IRespawner _parentSpawner;

        public void Init(IRespawner spawner)
        {
            _parentSpawner = spawner;

            _movement.OnFallOut.AddListener(() => _parentSpawner.RespawnLocalPlayer(resetCoins: false));
        }

        public void Respawn(Vector3 position, bool resetCoins)
        {
            if (resetCoins)
                _collectedCoins = 0;

            _movement.Respawn(position);
        }

        private void OnTriggerEnter(Collider other)
        {
            //Coins collecting
            if (!HasStateAuthority)
                return;

            if (other.TryGetComponent(out CoinBehavour coin))
            {
                _collectedCoins++;
                coin.Collecting();
            }
        }

        private void OnCoinsAmountChanged()
        {
            GameplayController.Instance.OnCoinChanged(_collectedCoins);
        }
    }
}
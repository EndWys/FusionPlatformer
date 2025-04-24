using Assets._Project.Scripts.Gameplay;
using Assets._Project.Scripts.Gameplay.LevelObjects;
using Assets._Project.Scripts.NetworkConnction;
using Assets._Project.Scripts.UI;
using Fusion;
using UnityEngine;

namespace Assets._Project.Scripts.Player
{
    public class PlayerBehaviour : NetworkBehaviour
    {
        [SerializeField] private PlayerMovement _movement;
        [SerializeField] private PlayerNameplate _nameplate;

        [Networked, HideInInspector, Capacity(24), OnChangedRender(nameof(OnNicknameChanged))]
        private string _nickname { get; set; }

        [Networked, OnChangedRender(nameof(OnCoinsAmountChanged))] 
        private int _collectedCoins { get; set; }

        public string Nickname => _nickname;
        public int CollectedCoins => _collectedCoins;

        private IRespawner _parentSpawner;

        public void Init(IRespawner spawner)
        {
            _parentSpawner = spawner;

            _movement.OnFallOut.AddListener(() => _parentSpawner.RespawnLocalPlayer(resetCoins: false));
        }

        public override void Spawned()
        {
            if (HasStateAuthority)
            {
                // Set player nickname that is saved in UIGameMenu
                _nickname = PlayerPrefs.GetString("PlayerName");
            }

            // In case the nickname is already changed,
            // we need to trigger the change manually
            OnNicknameChanged();
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

            if (other.TryGetComponent(out CheckpointBehaviour checkpoint))
            {
                checkpoint.CheckpointReached();
            }
        }

        private void OnCoinsAmountChanged()
        {
            GameplayController.Instance.OnCoinChanged(_collectedCoins);
        }

        private void OnNicknameChanged()
        {
            if (HasStateAuthority)
                return; // Do not show nickname for local player

            _nameplate.SetNickname(_nickname);
        }
    }
}
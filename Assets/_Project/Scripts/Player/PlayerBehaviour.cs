using Assets._Project.Scripts.EventBus;
using Assets._Project.Scripts.Gameplay;
using Assets._Project.Scripts.Gameplay.LevelObjects;
using Assets._Project.Scripts.NetworkConnction;
using Assets._Project.Scripts.UI;
using DG.Tweening;
using Fusion;
using UnityEngine;

namespace Assets._Project.Scripts.Player
{
    public class PlayerBehaviour : NetworkBehaviour
    {
        [SerializeField] private PlayerMovement _movement;
        [SerializeField] private PlayerNameplate _nameplate;
        [SerializeField] private Transform _modelRoot;

        [Networked] private int _collectedCoins { get; set; }

        [Networked, HideInInspector, Capacity(24), OnChangedRender(nameof(OnNicknameChanged))]
        private string _nickname { get; set; }

        public string Nickname => _nickname;

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
            {
                _collectedCoins = 0;
                Bus<CoinsCountChangeEvent>.Raise(new() { Count = _collectedCoins });
            } 

            _movement.Respawn(position);
        }

        public void PlaySpawnAnimation()
        {
            _modelRoot.DOPunchScale(Vector3.one * 0.5f, 0.1f).SetEase(Ease.InOutExpo);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!HasStateAuthority)
                return;

            if (other.TryGetComponent(out CoinBehavour coin))
            {
                _collectedCoins++;
                Bus<CoinsCountChangeEvent>.Raise(new() { Count = _collectedCoins });

                coin.Collecting();
            }

            if (other.TryGetComponent(out CheckpointBehaviour checkpoint))
            {
                checkpoint.CheckpointReached();
            }
        }

        private void OnNicknameChanged()
        {
            if (HasStateAuthority)
                return; // Do not show nickname for local player

            _nameplate.SetNickname(_nickname);
        }

        public bool IsEnoghtCoin(int requiredCoin)
        {
            return _collectedCoins >= requiredCoin;
        }
    }
}
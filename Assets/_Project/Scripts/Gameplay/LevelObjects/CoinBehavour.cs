using Assets._Project.Scripts.EventBus;
using DG.Tweening;
using Fusion;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.LevelObjects
{
    public class CoinBehavour : NetworkBehaviour
    {
        [Header("References")]
        [SerializeField] private Collider _trigger;
        [SerializeField] private GameObject _visualRoot;

        [Header("Timer")]
        [SerializeField] private float _coinRespawnTime = 4f;

        [Networked] private TickTimer _activationCooldown { get; set; }

        [Networked, OnChangedRender(nameof(OnCoinActiveChange))]
        private NetworkBool _isActive { get; set; } = true;

        public void Collecting()
        {
            RPC_OnCollect();

            // Prediction
            _isActive = false;
            _activationCooldown = TickTimer.CreateFromSeconds(Runner, _coinRespawnTime);
            OnCoinActiveChange();
        }

        public override void Render()
        {
            if (!_isActive && _activationCooldown.Expired(Runner))
            {
                _isActive = true;
                _activationCooldown = default;
            }
        }

        private void OnCoinActiveChange()
        {
            //Active state was already chenged by local prediction
            if (_visualRoot.activeInHierarchy == _isActive)
                return;

            if (_isActive)
            {
                _visualRoot.SetActive(true);
                var tr = _visualRoot.transform;
                tr.localScale = Vector3.zero;
                tr.DOScale(Vector3.one, 0.2f)
                    .SetEase(Ease.OutCubic)
                    .OnComplete(() => _trigger.enabled = true);
            }
            else
            {
                _trigger.enabled = false;
                _visualRoot.SetActive(false);
                Bus<CoinDisapearEvent>.Raise(new() { Posiotion = transform.position });
            }
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        private void RPC_OnCollect()
        {
            _isActive = false;
            _activationCooldown = TickTimer.CreateFromSeconds(Runner, _coinRespawnTime);
        }
    }
}
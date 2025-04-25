using Assets._Project.Scripts.EventBus;
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
            CoinActiveChange();
        }

        public override void Render()
        {
            if (!_isActive && _activationCooldown.Expired(Runner))
            {
                _isActive = true;
                _activationCooldown = default;
            }
        }

        private void CoinActiveChange()
        {
            _trigger.enabled = _isActive;
            _visualRoot.SetActive(_isActive);
        }

        private void OnCoinActiveChange()
        {
            CoinActiveChange();

            if (!_isActive)
                Bus<CoinDisapearEvent>.Raise(new() { Posiotion = transform.position });
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        private void RPC_OnCollect()
        {
            _isActive = false;
            _activationCooldown = TickTimer.CreateFromSeconds(Runner, _coinRespawnTime);
        }
    }
}
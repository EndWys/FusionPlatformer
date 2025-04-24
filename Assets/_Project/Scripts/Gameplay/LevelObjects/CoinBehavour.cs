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
            _trigger.enabled = _isActive;
            // Show/hide coin visual
            _visualRoot.SetActive(_isActive);
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        private void RPC_OnCollect()
        {
            _isActive = false;
            _activationCooldown = TickTimer.CreateFromSeconds(Runner, _coinRespawnTime);
        }
    }
}
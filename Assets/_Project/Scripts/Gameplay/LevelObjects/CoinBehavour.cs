using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

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

            // Running timer on non-StateAuthority - prediction
            _activationCooldown = TickTimer.CreateFromSeconds(Runner, _coinRespawnTime);
        }

        public override void Render()
        {
            bool isTimeExpired = _activationCooldown.ExpiredOrNotRunning(Runner);

            if(isTimeExpired && !_isActive)
                _isActive = true;

            if (!isTimeExpired && _isActive)
                _isActive = false;
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
            if (!_isActive)
                return;

            _activationCooldown = TickTimer.CreateFromSeconds(Runner, _coinRespawnTime);
        }
    }
}
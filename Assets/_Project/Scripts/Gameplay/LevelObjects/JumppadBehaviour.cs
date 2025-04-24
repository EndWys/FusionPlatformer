using Assets._Project.Scripts.Player;
using Fusion;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.LevelObjects
{
    public class JumppadBehaviour : NetworkBehaviour
    {
        [SerializeField] private Collider _trigger;
        [SerializeField] private GameObject _root;

        [SerializeField] private float _impulsePower;

        [SerializeField] private float _reactivationDelay = 1f;

        [Networked, OnChangedRender(nameof(OnActiveChange))] private NetworkBool _isActive { get; set; } = true;
        [Networked] private TickTimer _jumppadCooldown { get; set; }

        public void OnCloudLanding(IJumppadActor actor)
        {
            if (!_isActive)
                return;

            if (_jumppadCooldown.IsRunning)
                return;

            actor.GroundOnJumppad = true;
            actor.JumppadImpulse = _impulsePower;

            RPC_OnCloudGround();

            //Prediction
            _isActive = false;
            _jumppadCooldown = TickTimer.CreateFromSeconds(Runner, _reactivationDelay);
            OnActiveChange();
        }

        public override void Render()
        {
            if (!_isActive && _jumppadCooldown.Expired(Runner))
            {
                _isActive = true;
                _jumppadCooldown = default;
            }
        }

        private void OnActiveChange()
        {
            _root.SetActive(_isActive);
            _trigger.enabled = _isActive;
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        private void RPC_OnCloudGround()
        {
            _isActive = false;
            _jumppadCooldown = TickTimer.CreateFromSeconds(Runner, _reactivationDelay);
        }
    }
}
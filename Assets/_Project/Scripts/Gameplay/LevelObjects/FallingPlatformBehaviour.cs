using Assets._Project.Scripts.EventBus;
using Assets._Project.Scripts.Gameplay.LevelObjects.Base;
using Assets._Project.Scripts.Player;
using DG.Tweening;
using Fusion;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.LevelObjects
{
    public class FallingPlatformBehaviour : PlayerContactCooldownLevelObject<IPlayerComponent>
    {
        [Header("References")]
        [SerializeField] private Collider _trigger;
        [SerializeField] private Rigidbody _platformBody;
        [SerializeField] private Transform _platformTransfrom;

        [Header("Time")]
        [SerializeField] private float _delayBeforeFall = 0.2f;

        [Networked] protected TickTimer _delayTimer { get; set; }

        private Vector3 _originalPosition;

        protected override bool _isEnableLocaly => _platformBody.isKinematic && _trigger.enabled;

        public override void Spawned()
        {
            _originalPosition = _platformTransfrom.position;
        }

        protected override bool CheckContactCondition()
        {
            return _isActiveInNetwork && !_activationCooldownTimer.IsRunning && !_delayTimer.IsRunning;
        }
        protected override void ContactAction()
        {
            RPC_StartFallDelayTimer();
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        private void RPC_StartFallDelayTimer()
        {
            _delayTimer = TickTimer.CreateFromSeconds(Runner, _delayBeforeFall);
        }

        protected override void ClientPredition()
        {
            _delayTimer = TickTimer.CreateFromSeconds(Runner, _delayBeforeFall);
        }

        public override void FixedUpdateNetwork()
        {
            if (_delayTimer.Expired(Runner))
            {
                if (_isActiveInNetwork)
                {
                    _isActiveInNetwork = false;
                    _delayTimer = default;
                    StartReactivationTimer();
                }
            }
        }

        protected override void BecomeActive()
        {
            _platformBody.isKinematic = true;

            // Reset fallen platform to its original position
            _platformTransfrom.position = _originalPosition;

            _platformTransfrom.localScale = Vector3.zero;
            _platformTransfrom.DOScale(Vector3.one, 0.2f)
                .SetEase(Ease.OutCubic)
                .OnComplete(() =>
                {
                    _trigger.enabled = true;
                }); 
        }

        protected override void BecomeDiactivated()
        {
            _platformBody.isKinematic = false;
            _trigger.enabled = false;

            Bus<PlatformFallEvent>.Raise(new() { Posiotion = _platformTransfrom.position });

            _platformBody.AddForce(Vector3.down * 30f, ForceMode.Impulse);
        }
    }
}
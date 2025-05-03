using Assets._Project.Scripts.EventBus;
using Assets._Project.Scripts.Gameplay.LevelObjects.Base;
using Assets._Project.Scripts.Player;
using DG.Tweening;
using Fusion;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.LevelObjects
{
    public class JumppadBehaviour : PlayerContactCooldownLevelObject<IJumppadActor>
    {
        [Header("References")]
        [SerializeField] private Collider _contactTrigger;
        [SerializeField] private GameObject _visualRoot;

        [Header("Jumppad Settings")]
        [SerializeField] private float _impulsePower;

        protected override bool _isEnableLocaly => _visualRoot.activeInHierarchy && _contactTrigger.enabled;

        protected override bool CheckContactCondition()
        {
            return _isActiveInNetwork && !_activationCooldownTimer.IsRunning;
        }

        protected override void ContactAction()
        {
            _levelRunnerComponent.BounceFromJumppad(_impulsePower);
            RPC_JumppadDisapearing();
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        private void RPC_JumppadDisapearing()
        {
            _isActiveInNetwork = false;
            StartReactivationTimer();
        }

        protected override void ClientPredition()
        {
            _isActiveInNetwork = false;
            StartReactivationTimer();
            OnStateActiveChange();
        }

        protected override void BecomeActive()
        {
            _visualRoot.SetActive(true);

            _visualRoot.transform.DOScale(Vector3.one, 0.2f)
                .SetEase(Ease.OutCubic)
                .OnComplete(() => _contactTrigger.enabled = true);
        }

        protected override void BecomeDiactivated()
        {
            Bus<CloudDisapearEvent>.Raise(new CloudDisapearEvent() { Posiotion = transform.position });

            _contactTrigger.enabled = false;

            _visualRoot.transform.DOScale(Vector3.zero, 0.2f)
                .SetEase(Ease.InCubic)
                .OnComplete(() => _visualRoot.SetActive(_isActiveInNetwork));
        }
    }
}
using Assets._Project.Scripts.EventBus;
using Assets._Project.Scripts.Gameplay.LevelObjects.Base;
using Assets._Project.Scripts.Player;
using DG.Tweening;
using Fusion;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.LevelObjects
{
    public class CoinBehavour : PlayerContactCooldownLevelObject<IWalletHolder>
    {
        [Header("References")]
        [SerializeField] private Collider _contactTrigger;
        [SerializeField] private GameObject _visualRoot;

        protected override bool _isEnableLocaly => _visualRoot.activeInHierarchy;

        protected override bool CheckContactCondition()
        {
            return _isActiveInNetwork && !_activationCooldownTimer.IsRunning;
        }

        protected override void ContactAction()
        {
            _levelRunnerComponent.AddCoinsToWallet(1);
            RPC_CoinCollecting();
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        private void RPC_CoinCollecting()
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

            var tr = _visualRoot.transform;
            tr.localScale = Vector3.zero;
            tr.DOScale(Vector3.one, 0.2f)
                .SetEase(Ease.OutCubic)
                .OnComplete(() => _contactTrigger.enabled = true);
        }

        protected override void BecomeDiactivated()
        {
            _visualRoot.SetActive(false);
            _contactTrigger.enabled = false;
            Bus<CoinDisapearEvent>.Raise(new() { Posiotion = transform.position });
        }
    }
}
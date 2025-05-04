using Assets._Project.Scripts.EventBus;
using Assets._Project.Scripts.Gameplay.LevelObjects.Base;
using Assets._Project.Scripts.Player;
using Assets._Project.Scripts.ServiceLocatorSystem;
using DG.Tweening;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.LevelObjects
{
    public class CrownBehaviour : PlayerContactLevelObject<PlayerBehaviour>
    {
        [SerializeField] private Transform _root;

        protected override bool CheckContactCondition()
        {
            return HasStateAuthority;
        }

        protected override void ContactAction()
        {
            ServiceLocator.Instance.Get<IMatchFinisherHadler>().TryToFinishMatch(_levelRunnerComponent);
        }

        public void WinnerReachFinish()
        {
            Vector3 staterPos = _root.position;

            _root.DOMoveY(2f, 1f).OnComplete(() =>
            {
                Bus<CrownReachEvent>.Raise(new() { Posiotion = _root.position });
                _root.gameObject.SetActive(false);
                _root.position = staterPos;
            });
        }

        public void EnableCrown()
        {
            _root.gameObject.SetActive(true);
        }
    }
}
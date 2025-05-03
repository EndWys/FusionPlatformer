using Assets._Project.Scripts.EventBus;
using Assets._Project.Scripts.Player;
using DG.Tweening;
using Fusion;
using UnityEngine;
using UnityEngine.Events;

namespace Assets._Project.Scripts.Gameplay.LevelObjects
{
    public class CrownBehaviour : NetworkBehaviour
    {
        [SerializeField] private Transform _root;

        [HideInInspector] public UnityEvent<PlayerBehaviour> OnFlagReached;

        private void OnTriggerEnter(Collider other)
        {
            CheckIfLevelRunnerReachFinish(other);
        }

        public void CheckIfLevelRunnerReachFinish(Collider other)
        {
            if (!HasStateAuthority)
                return;

            var player = other.transform.parent != null ? other.GetComponentInParent<PlayerBehaviour>() : null;
            if (player == null)
                return;

            OnFlagReached.Invoke(player);
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
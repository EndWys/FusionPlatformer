using Assets._Project.Scripts.EventBus;
using Assets._Project.Scripts.Player;
using DG.Tweening;
using Fusion;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Assets._Project.Scripts.Gameplay.LevelObjects
{
    public class FlagBehaviour : NetworkBehaviour
    {
        [SerializeField] private Transform _root;

        [HideInInspector] public UnityEvent<PlayerBehaviour> OnFlagReached;
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

        private void OnTriggerEnter(Collider other)
        {
            // Flag check is triggered only on state authority
            if (!HasStateAuthority)
                return;

            var player = other.transform.parent != null ? other.GetComponentInParent<PlayerBehaviour>() : null;
            if (player != null)
            {
                OnFlagReached.Invoke(player);
            }
        }
    }
}
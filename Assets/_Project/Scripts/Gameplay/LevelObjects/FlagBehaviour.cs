using Assets._Project.Scripts.Player;
using Fusion;
using UnityEngine;
using UnityEngine.Events;

namespace Assets._Project.Scripts.Gameplay.LevelObjects
{
    public class FlagBehaviour : NetworkBehaviour
    {
        [HideInInspector] public UnityEvent<PlayerBehaviour> OnFlagReached;

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
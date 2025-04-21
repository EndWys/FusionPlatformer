using Assets._Project.Scripts.Player;
using Fusion;
using UnityEngine;
using UnityEngine.Events;

namespace Assets._Project.Scripts.Gameplay
{
    public class FlagBehaviour : NetworkBehaviour
    {
        [HideInInspector] public UnityEvent<PlayerBehaviour> OnFlagReached;

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("reached");

            // Flag check is triggered only on state authority
            if (HasStateAuthority == false)
                return;

            var player = other.transform.parent != null ? other.GetComponentInParent<PlayerBehaviour>() : null;
            if (player != null)
            {
                Debug.Log("invoke");
                OnFlagReached.Invoke(player);
            }
        }
    }
}
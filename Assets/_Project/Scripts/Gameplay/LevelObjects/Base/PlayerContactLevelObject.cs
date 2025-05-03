using Assets._Project.Scripts.Player;
using Fusion;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.LevelObjects.Base
{
    public abstract class PlayerContactLevelObject<T> : NetworkBehaviour where T : IPlayerComponent
    {
        protected T _levelRunnerComponent;

        private void OnTriggerEnter(Collider other)
        {
            ContactWithObject(other);
        }

        private void ContactWithObject(Collider contact)
        {
            if (!IsItPlayer(contact))
                return;

            if (!CheckContactCondition())
                return;

            ContactAction();

            ClientPredition();
        }
        protected virtual bool IsItPlayer(Collider contact)
        {
            var levelRunner = contact.GetComponentInParent<PlayerBehaviour>();

            if(levelRunner != null && levelRunner is T component)
            {
                _levelRunnerComponent = component;
                return true;
            }

            return false;
        }

        protected virtual bool CheckContactCondition()
        {
            return true;
        }

        protected abstract void ContactAction();

        protected virtual void ClientPredition()
        {

        }
    }
}
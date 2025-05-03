using Fusion;

namespace Assets._Project.Scripts.Gameplay.LevelObjects.Base
{
    public abstract class PlayerContactActivationLevelObject : PlayerContactLevelObject
    {
        [Networked, OnChangedRender(nameof(OnStateActiveChange))]
        protected NetworkBool _isActiveInNetwork { get; set; } = true;

        protected abstract bool _isEnableLocaly { get; }

        public override void ContactWithPlayer()
        {
            base.ContactWithPlayer();

            ClientPredition();
        }

        protected abstract void ClientPredition();

        protected void OnStateActiveChange()
        {
            if (HaveSynchronizedState())
                return; // if not Synchronized - make it

            if (_isActiveInNetwork)
            {
                BecomeActive();
            }
            else
            {
                BecomeDiactivated();
            }
        }

        protected bool HaveSynchronizedState()
        {
            return _isActiveInNetwork == _isEnableLocaly;
        }

        protected abstract void BecomeActive();

        protected abstract void BecomeDiactivated();
    }
}
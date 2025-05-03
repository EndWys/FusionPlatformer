using Fusion;

namespace Assets._Project.Scripts.Gameplay.LevelObjects.Base
{
    public abstract class PlayerContactLevelObject : NetworkBehaviour
    {
        public virtual void ContactWithPlayer()
        {
            if (!CheckContactCondition())
                return;

            ContactAction();
        }

        protected virtual bool CheckContactCondition()
        {
            return true;
        }

        protected abstract void ContactAction();
    }
}
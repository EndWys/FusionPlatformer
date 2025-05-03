using Fusion;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.LevelObjects.Base
{
    public abstract class PlayerContactCooldownLevelObject : PlayerContactActivationLevelObject
    {
        [Header("Timer")]
        [SerializeField] protected float _reactivationTime = 4f;

        [Networked] protected TickTimer _activationCooldownTimer { get; set; }

        protected void StartReactivationTimer()
        {
            _activationCooldownTimer = TickTimer.CreateFromSeconds(Runner, _reactivationTime);
        }

        public override void Render()
        {
            if (!_isActiveInNetwork && _activationCooldownTimer.Expired(Runner))
            {
                _isActiveInNetwork = true;
                _activationCooldownTimer = default;
            }
        }
    }
}
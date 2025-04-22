using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.LevelObjects
{
    public class FallingPlatformBehaviour : NetworkBehaviour
    {
        [Header("Time")]
        [SerializeField] private float _fallDelay = 0.2f;
        [SerializeField] private float _reactivationDelay = 1f;

        [Header("References")]
        [SerializeField] private Collider _trigger;
        [SerializeField] private Rigidbody _platformBody;
        [SerializeField] private Transform _platformTransfrom;

        [Networked] private NetworkBool _isActive { get; set; } = true;
        [Networked] private TickTimer _platformCooldown { get; set; }

        private Vector3 _originalPosition;

        public override void Spawned()
        {
            // Save original platform position, so we can reset position
            // when platform gets reactivated
            _originalPosition = _platformTransfrom.position;
        }

        public override void FixedUpdateNetwork()
        {
            if (_platformCooldown.Expired(Runner) == true)
            {
                if (_isActive)
                {
                    // Schedule reactivation
                    _isActive = false;
                    _platformCooldown = TickTimer.CreateFromSeconds(Runner, _reactivationDelay);
                }
                else
                {
                    // Platform is active again
                    _isActive = true;
                    _platformCooldown = default;
                }
            }

            _trigger.enabled = _isActive;
        }

        public override void Render()
        {
            // Check the timer to predict _isActive value
            bool isActive = _platformCooldown.Expired(Runner) ? !_isActive : _isActive;

            if (_platformBody.isKinematic == isActive)
                return; // No changes

            _platformBody.isKinematic = isActive;
            _trigger.enabled = isActive;

            if (isActive)
            {
                // Reset fallen platform to its original position
                _platformTransfrom.position = _originalPosition;
            }
            else
            {
                _platformBody.AddForce(Vector3.down * 30f, ForceMode.Impulse);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            // Platforms falling is initiated only on state authority
            if (!HasStateAuthority)
                return;

            if (!_isActive)
                return;

            if (_platformCooldown.IsRunning)
                return;//Already falling

            if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
                return;

            _platformCooldown = TickTimer.CreateFromSeconds(Runner, _fallDelay);
        }
    }
}
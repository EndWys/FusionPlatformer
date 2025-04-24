using Fusion.Addons.SimpleKCC;
using UnityEngine;

namespace Assets._Project.Scripts.Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private ParticleSystem _dustParticle;

        private int _animIDSpeed;
        private int _animIDGrounded;

        private void Awake()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
        }

        public void SetMovementAnimationsAndEffects(float speed, bool isGrounded)
        {
            _animator.SetFloat(_animIDSpeed, speed);
            _animator.SetBool(_animIDGrounded, isGrounded);

            var emission = _dustParticle.emission;
            emission.enabled = isGrounded && speed > 1f;
        }
    }
}
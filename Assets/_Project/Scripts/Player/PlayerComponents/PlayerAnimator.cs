using DG.Tweening;
using UnityEngine;

namespace Assets._Project.Scripts.Player.PlayerComponents
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _visualRoot;
        [SerializeField] private ParticleSystem _dustParticle;

        private int _animIDSpeed;
        private int _animIDGrounded;

        private void Awake()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
        }

        public void PlaySpawnAnimation()
        {
            _visualRoot.DOPunchScale(Vector3.one * 0.5f, 0.1f).SetEase(Ease.InOutExpo);
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
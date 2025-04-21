using UnityEngine;

namespace Assets._Project.Scripts.Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private int _animIDSpeed;
        private int _animIDGrounded;

        private void Awake()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
        }

        public void SetMovementAnimations(float speed, bool isGrounded)
        {
            _animator.SetFloat(_animIDSpeed, speed);
            _animator.SetBool(_animIDGrounded, isGrounded);
        }
    }
}
using Assets._Project.Scripts.EventBus;
using Assets._Project.Scripts.Gameplay;
using Assets._Project.Scripts.Player.PlayerInput;
using Assets._Project.Scripts.ServiceLocatorSystem;
using DG.Tweening;
using ExitGames.Client.Photon.StructWrapping;
using Fusion;
using Fusion.Addons.SimpleKCC;
using UnityEngine;

namespace Assets._Project.Scripts.Player.PlayerComponents
{
    public class PlayerMovement : NetworkBehaviour, IJumppadActor
    {
        [SerializeField] private SimpleKCC _kcc;
        [SerializeReference] private BasePlayerInputHandler _input;
        [SerializeReference] private PlayerAnimator _animator;
        [SerializeReference] private PlayerSounds _sounds;

        [Header("Camera")]
        [SerializeField] private Transform CameraPivot;
        [SerializeField] private Transform CameraHandle;

        [Header("Jump Settings")]
        [SerializeField] private float _jumpImpulse = 10f;
        [SerializeField] private float _upperGravity = 25f;
        [SerializeField] private float _downGravity = 40f;
        [SerializeField] private float _airDeceleration = 1.3f;
        [SerializeField] private float _airAcceleration = 25f;

        [Header("Walk Settings")]
        [SerializeField] private float _walkSpeed = 2f;
        [SerializeField] private float _sprintSpeed = 5f;
        [SerializeField] private float _rotationSpeed = 8f;
        [SerializeField] private float _groundDeceleration = 25f;
        [SerializeField] private float _groundAcceleration = 55f;

        [Networked, OnChangedRender(nameof(OnJumpingChanged))]
        private NetworkBool _isJumping { get; set; }

        private Vector3 _moveVelocity;

        private float _jumppadImpulse = 0f;
        private bool _groundOnJumppad = false;

        private IMatchFinisherHadler _matchFinisher;

        public override void Spawned()
        {
            _matchFinisher = ServiceLocator.Instance.Get<IMatchFinisherHadler>();
            Debug.Log("Player Spawnerd");
        }

        public override void FixedUpdateNetwork()
        {
            if (_matchFinisher.IsMatchFinished)
            {
                ProcessInput(default);
                _input.ResetInput();
                return;
            }

            if (_kcc.Position.y < -15f)
            {
                // Player fell, let's respawn
                ServiceLocator.Instance.Get<IRunnerRespawner>().RestLevelRunnerPosition();
            }

            ProcessInput(_input.CurrentInput);

            if (_kcc.IsGrounded)
            {
                // Stop jumping
                _isJumping = false;
            }

            _input.ResetInput();
        }

        public override void Render()
        {
            _animator.SetMovementAnimationsAndEffects(_kcc.RealSpeed, _kcc.IsGrounded);
            _sounds.SetSoundsSettings(_kcc.RealSpeed, _sprintSpeed, _kcc.IsGrounded);
        }

        public void Respawn(Vector3 position)
        {
            _kcc.SetPosition(position);
            _kcc.SetLookRotation(0f, 0f);

            _moveVelocity = Vector3.zero;

            _animator.PlaySpawnAnimation();
        }

        private void LateUpdate()
        {
            // Only local player needs to update the camera
            if (HasStateAuthority == false)
                return;

            // Update camera pivot and transfer properties from camera handle to Main Camera.
            CameraPivot.rotation = Quaternion.Euler(_input.CurrentInput.LookRotation);
            Camera.main.transform.SetPositionAndRotation(CameraHandle.position, CameraHandle.rotation);
        }

        private void ProcessInput(GameplayInput input)
        {
            float jumpImpulse = 0f;

            if (_groundOnJumppad)
            {
                jumpImpulse = _jumppadImpulse;
                _isJumping = true;
                _groundOnJumppad = false;
            }
            else if (_kcc.IsGrounded && input.Jump)
            {
                jumpImpulse = _jumpImpulse;
                _isJumping = true;
            }

            _kcc.SetGravity(-(_kcc.RealVelocity.y >= 0f ? _upperGravity : _downGravity));

            float speed = input.Sprint ? _sprintSpeed : _walkSpeed;

            var lookRotation = Quaternion.Euler(0f, input.LookRotation.y, 0f);

            var moveDirection = lookRotation * new Vector3(input.MoveDirection.x, 0f, input.MoveDirection.y);
            var desiredMoveVelocity = moveDirection * speed;

            float acceleration;
            if (desiredMoveVelocity == Vector3.zero)
            {
                acceleration = _kcc.IsGrounded ? _groundDeceleration : _airDeceleration;
            }
            else
            {
                var currentRotation = _kcc.TransformRotation;
                var targetRotation = Quaternion.LookRotation(moveDirection);
                var nextRotation = Quaternion.Lerp(currentRotation, targetRotation, _rotationSpeed * Runner.DeltaTime);

                _kcc.SetLookRotation(nextRotation.eulerAngles);

                acceleration = _kcc.IsGrounded ? _groundAcceleration : _airAcceleration;
            }

            _moveVelocity = Vector3.Lerp(_moveVelocity, desiredMoveVelocity, acceleration * Runner.DeltaTime);
 
            _kcc.Move(_moveVelocity, jumpImpulse);
        }

        public void BounceFromJumppad(float impulsePower)
        {
            _jumppadImpulse = impulsePower;
            _groundOnJumppad = true;
        }

        private void OnJumpingChanged()
        {
            if (_isJumping)
            {
                _sounds.PlayJump();
            }
            else
            {
                _sounds.PlayLand();
            }
        }
    }
}
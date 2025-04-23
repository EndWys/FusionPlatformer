using UnityEngine;

namespace Assets._Project.Scripts.Player.PlayerInput
{
    public class DefaultInputHandler : BasePlayerInputHandler
    {
        [SerializeField] private float InitialLookRotation = 18f;

        public override GameplayInput CurrentInput => _input;
        private GameplayInput _input;

        public override void ResetInput()
        {
            // Reset input after it was used to detect changes correctly again
            _input.MoveDirection = default;
            _input.Jump = false;
            _input.Sprint = false;
        }

        private void Start()
        {
            // Set initial camera rotation
            _input.LookRotation = new Vector2(InitialLookRotation, 0f);
        }

        private void Update()
        {
            if (Cursor.lockState != CursorLockMode.Locked)
                return;

            var lookRotationDelta = new Vector2(-Input.GetAxisRaw("Mouse Y"), Input.GetAxisRaw("Mouse X"));
            _input.LookRotation = ClampLookRotation(_input.LookRotation + lookRotationDelta);

            var moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            _input.MoveDirection = moveDirection.normalized;

            _input.Jump |= Input.GetButtonDown("Jump");
            _input.Sprint |= Input.GetButton("Sprint");
        }

        private Vector2 ClampLookRotation(Vector2 lookRotation)
        {
            lookRotation.x = Mathf.Clamp(lookRotation.x, -30f, 70f);
            return lookRotation;
        }
    }
}
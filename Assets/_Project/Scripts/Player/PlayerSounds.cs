using UnityEngine;

namespace Assets._Project.Scripts.Player
{
    public class PlayerSounds : MonoBehaviour
    {
        [SerializeField] private AudioSource _footstepSound;

        [SerializeField] private AudioClip _jumpAudioClip;
        [SerializeField] private AudioClip _landAudioClip;

        private Transform _tr;

        private void Awake()
        {
            _tr = GetComponent<Transform>();
        }

        public void PlayJump()
        {
            AudioSource.PlayClipAtPoint(_jumpAudioClip, _tr.position, 1f);
        }

        public void PlayLand()
        {
            AudioSource.PlayClipAtPoint(_landAudioClip, _tr.position, 1f);
        }

        public void SetSoundsSettings(float speed, float sprint, bool isGrounded)
        {
            _footstepSound.enabled = isGrounded && speed > 1f;
            _footstepSound.pitch = speed > sprint - 1 ? 1.5f : 1f;
        }
    }
}
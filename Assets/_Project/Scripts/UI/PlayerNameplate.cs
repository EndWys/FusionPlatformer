using TMPro;
using UnityEngine;

namespace Assets._Project.Scripts.UI
{
    public class PlayerNameplate : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nicknameText;

        private Transform _tr;
        private Transform _cameraTransform;

        public void SetNickname(string nickname)
        {
            _nicknameText.text = nickname;
        }

        private void Awake()
        {
            _tr = transform;
            _cameraTransform = Camera.main.transform;
            _nicknameText.text = string.Empty;
        }

        private void LateUpdate()
        {
            // Rotate nameplate toward camera
            _tr.rotation = _cameraTransform.rotation;
        }
    }
}
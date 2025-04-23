using Assets._Project.Scripts.NetworkConnction;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Fusion;

namespace Assets._Project.Scripts.UI
{
    public class MenuUIController : MonoBehaviour
    {
        [SerializeField] private NetworkController _networkController;

        [Header("Input Fields")]
        [SerializeField] private TMP_InputField _nicknameText;
        [SerializeField] private TMP_InputField _roomText;

        [Header("Buttons")]
        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _disconnectButton;

        [Header("Canvas")]
        [SerializeField] private CanvasGroup _root;

        private bool _isOpened = true;
        private bool _disconnetingInProgress = false;
        private bool _connected = false;

        private void Awake()
        {
            _isOpened = true;

            _startGameButton.onClick.AddListener(GameStartClick);
            _disconnectButton.onClick.AddListener(DisconnectClick);
        }

        private void GameStartClick()
        {
            PlayerPrefs.SetString("PlayerName", _nicknameText.text);

            _networkController.StartGame(GameMode.Shared,
                OnConnecting, 
                _roomText.text);
        }

        private async void DisconnectClick()
        {
            _disconnetingInProgress = true;
            await _networkController.Disconnect();
        }

        private void Update()
        {
            if (_disconnetingInProgress)
                return;

            if (!_connected)
                return;

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape))
            {
                ToggleVisibility();
            }

            if (_isOpened)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        private void ToggleVisibility()
        {
            _isOpened = !_isOpened;

            _root.alpha = _isOpened ? 1 : 0;
        }

        private void OnConnecting()
        {
            _nicknameText.interactable = false;
            _roomText.interactable = false;

            _startGameButton.gameObject.SetActive(false);
            _disconnectButton.gameObject.SetActive(true);

            ToggleVisibility();

            _connected = true;
        }
    }
}
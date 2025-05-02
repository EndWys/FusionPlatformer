using Assets._Project.Scripts.NetworkConnction;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Fusion;
using System;
using UnityEngine.Events;

namespace Assets._Project.Scripts.UI
{
    public class MenuUIController : MonoBehaviour
    {
        [Header("Input Fields")]
        [SerializeField] private TMP_InputField _nicknameText;
        [SerializeField] private TMP_InputField _roomText;

        [Header("Buttons")]
        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _disconnectButton;

        [Header("Canvas")]
        [SerializeField] private CanvasGroup _root;

        [HideInInspector] public UnityEvent OnGameStartButtonClick;
        [HideInInspector] public UnityEvent OnDisconnectedButtonClick;

        public string Nickname => _nicknameText.text;
        public string RoomName => _roomText.text;

        private bool _isOpened = true;

        private void Awake()
        {
            _isOpened = true;

            _startGameButton.onClick.AddListener(OnGameStartButtonClick.Invoke);
            _disconnectButton.onClick.AddListener(OnDisconnectedButtonClick.Invoke);
        }

        private void Update()
        {
            if (ConnectionController.ConnectionState != ConnectionState.Connected)
                return;

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape))
            {
                ToggleVisibility();
            }
        }

        private void ToggleVisibility()
        {
            _isOpened = !_isOpened;

            _root.alpha = _isOpened ? 1 : 0;

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

        public void OnConnectedSuccessfully()
        {
            _nicknameText.interactable = false;
            _roomText.interactable = false;

            _startGameButton.gameObject.SetActive(false);
            _disconnectButton.gameObject.SetActive(true);

            ToggleVisibility();
        }
    }
}
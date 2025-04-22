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

        [Header("Canvas")]
        [SerializeField] private CanvasGroup _root;

        private void Awake()
        {
            _startGameButton.onClick.AddListener(GameStartClick);
        }

        private void GameStartClick()
        {
            PlayerPrefs.SetString("PlayerName", _nicknameText.text);

            _networkController.StartGame(GameMode.Shared, 
                () => _root.alpha = 0, 
                _roomText.text);
        }
    }
}
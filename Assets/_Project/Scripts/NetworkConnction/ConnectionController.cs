using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using Assets._Project.Scripts.UI;

namespace Assets._Project.Scripts.NetworkConnction
{
    public enum ConnectionState
    {
        Disconnected,
        Disconnecting,
        Connecting,
        Connected,
    }

    public class ConnectionController : MonoBehaviour
    {
        [SerializeField] private MenuUIController _menu;

        private NetworkRunner _runner;
        private NetworkSceneManagerDefault _sceneManager;

        public static ConnectionState ConnectionState { get; private set; }

        public void Init()
        {
            _menu.OnGameStartButtonClick.AddListener(OnGameStartClick);
            _menu.OnDisconnectedButtonClick.AddListener(OnDisconnectedClick);
        }

        private void OnGameStartClick()
        {
            string nicknamePrefKey = "PlayerName";

            PlayerPrefs.SetString(nicknamePrefKey, _menu.Nickname);

            StartGame(GameMode.Shared);
        }

        private async void StartGame(GameMode mode)
        {
            _runner = gameObject.GetOrAddComponent<NetworkRunner>();
            _sceneManager = gameObject.GetOrAddComponent<NetworkSceneManagerDefault>();

            if (ConnectionState != ConnectionState.Disconnected)
                return;

            ConnectionState = ConnectionState.Connecting;

            _runner.ProvideInput = true;

            var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
            var sceneInfo = new NetworkSceneInfo();
            if (scene.IsValid)
            {
                sceneInfo.AddSceneRef(scene, LoadSceneMode.Single);
            }

            var result = await _runner.StartGame(new StartGameArgs()
            {
                GameMode = mode,
                SessionName = _menu.RoomName,
                Scene = scene,
                SceneManager = _sceneManager
            });

            ConnectionState = result.Ok ? ConnectionState.Connected : ConnectionState.Disconnected;

            if (ConnectionState == ConnectionState.Connected)
                _menu.OnConnectedSuccessfully();
        }

        private void OnDisconnectedClick()
        {
            Disconnect();
        }

        public async void Disconnect()
        {
            if (_runner == null)
                return;

            if (ConnectionState != ConnectionState.Connected)
                return;

            ConnectionState = ConnectionState.Disconnecting;

            await _runner.Shutdown();
            _runner = null;

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

            ConnectionState = ConnectionState.Disconnected;
        }

        private void OnDisable()
        {
            _menu.OnGameStartButtonClick.RemoveListener(OnGameStartClick);
            _menu.OnDisconnectedButtonClick.RemoveListener(OnDisconnectedClick);
        }
    }
}
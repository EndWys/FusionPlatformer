using Fusion;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using Unity.VisualScripting;

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
        private NetworkRunner _runner;
        private NetworkSceneManagerDefault _sceneManager;

        public ConnectionState ConnectionState => _connectionState;
        private ConnectionState _connectionState = ConnectionState.Disconnected;

        public async void StartGame(GameMode mode,Action OnConnected, string roomName)
        {
            _runner = gameObject.GetOrAddComponent<NetworkRunner>();
            _sceneManager = gameObject.GetOrAddComponent<NetworkSceneManagerDefault>();

            if (ConnectionState != ConnectionState.Disconnected)
                return;

            _connectionState = ConnectionState.Connecting;

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
                SessionName = roomName,
                Scene = scene,
                SceneManager = _sceneManager
            });

            _connectionState = result.Ok ? ConnectionState.Connected : ConnectionState.Disconnected;

            if(_connectionState == ConnectionState.Connected)
                OnConnected.Invoke();
        }

        public async Task Disconnect()
        {
            if (_runner == null)
                return;

            if (ConnectionState != ConnectionState.Connected)
                return;

            _connectionState = ConnectionState.Disconnecting;

            await _runner.Shutdown();
            _runner = null;

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

            _connectionState = ConnectionState.Disconnected;
        }
    }
}
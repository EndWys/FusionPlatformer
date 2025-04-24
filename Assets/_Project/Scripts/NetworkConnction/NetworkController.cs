using Fusion.Sockets;
using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

namespace Assets._Project.Scripts.NetworkConnction
{
    public enum ConnectionState
    {
        Disconnected,
        Disconnecting,
        Connecting,
        Connected,
    }

    public class NetworkController : MonoBehaviour, INetworkRunnerCallbacks
    {
        [SerializeField] private NetworkRunner _runner;
        [SerializeField] private NetworkSceneManagerDefault _sceneManager;

        [SerializeField] private NetworkPlayerSpawner _playerSpawner;

        public ConnectionState ConnectionState => _connectionState;
        private ConnectionState _connectionState = ConnectionState.Disconnected;

        public async void StartGame(GameMode mode,Action OnConnected, string roomName)
        {
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

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) 
        {
            if (player != runner.LocalPlayer)
                return;

            _playerSpawner.SpawnPlayer(runner, player);
            
        }
        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
        public void OnInput(NetworkRunner runner, NetworkInput input) { }
        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
        public void OnConnectedToServer(NetworkRunner runner) { }
        public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
        public void OnSceneLoadDone(NetworkRunner runner) { }
        public void OnSceneLoadStart(NetworkRunner runner) { }
        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
        public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
    }
}
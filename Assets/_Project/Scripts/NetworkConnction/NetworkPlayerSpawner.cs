using Assets._Project.Scripts.Gameplay;
using Assets._Project.Scripts.Player;
using Fusion;
using UnityEngine;

namespace Assets._Project.Scripts.NetworkConnction
{
    public class NetworkPlayerSpawner : SimulationBehaviour, IPlayerJoined
    {
        [SerializeField] private PlayerBehaviour _playerPrefab;
        [SerializeField] private LevelRunController _runController;

        private PlayerBehaviour _localPlayer;

        public void PlayerJoined(PlayerRef player)
        {
            if (player != Runner.LocalPlayer)
                return;

            _localPlayer = Runner.Spawn(_playerPrefab, inputAuthority: player);

            _runController.Init(_localPlayer);
        }
    }
}
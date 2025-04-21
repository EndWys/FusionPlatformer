using Assets._Project.Scripts.Gameplay;
using Assets._Project.Scripts.Player;
using Fusion;
using UnityEngine;

namespace Assets._Project.Scripts.NetworkConnction
{
    public class NetworkPlayerSpawner : MonoBehaviour
    {
        [SerializeField] private GameplayController _gameplayController;
        [SerializeField] private PlayerBehaviour _playerPrefab;

        public void SpawnPlayer(NetworkRunner runner, PlayerRef player)
        {
            _gameplayController.LocalPlayer = runner.Spawn(_playerPrefab, new Vector3(0, 1, 0), Quaternion.identity, player);
        }
    }
}
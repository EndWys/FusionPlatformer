using Assets._Project.Scripts.Player;
using Fusion;
using UnityEngine;

namespace Assets._Project.Scripts.NetworkConnction
{
    public interface IRespawner
    {
        public void RespawnLocalPlayer(bool resetCoins);
    }
    public class NetworkPlayerSpawner : MonoBehaviour, IRespawner
    {
        [SerializeField] private PlayerBehaviour _playerPrefab;
        [Header("Spawn Settings")]
        [SerializeField] private float _spawnRadius = 3f;

        private PlayerBehaviour _localPlayer;

        public void SpawnPlayer(NetworkRunner runner, PlayerRef player)
        {
            _localPlayer = runner.Spawn(_playerPrefab, GetSpawnPosition(), Quaternion.identity, player);
            _localPlayer.Init(this);
        }

        public void RespawnLocalPlayer(bool resetCoins)
        {
            _localPlayer.Respawn(GetSpawnPosition(), resetCoins);
        }

        private Vector3 GetSpawnPosition()
        {
            var randomPositionOffset = Random.insideUnitCircle * _spawnRadius;
            return transform.position + new Vector3(randomPositionOffset.x, 0, randomPositionOffset.y);
        }
    }
}
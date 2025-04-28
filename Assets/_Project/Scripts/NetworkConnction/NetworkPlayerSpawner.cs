using Assets._Project.Scripts.EventBus;
using Assets._Project.Scripts.Gameplay.LevelObjects;
using Assets._Project.Scripts.Player;
using Fusion;
using System.Collections.Generic;
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

        [Header("Respawn Points")]
        [SerializeField] private List<CheckpointBehaviour> _checkpoints = new();

        [Header("Spawn Settings")]
        [SerializeField] private float _spawnRadius = 3f;

        private int _currentCheckpointIndex = 0; //No checkpoints on start
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
        public void ResetCheckpoints()
        {
            _currentCheckpointIndex = 0;
        }

        private Vector3 GetSpawnPosition()
        {
            var randomPositionOffset = Random.insideUnitCircle * _spawnRadius;

            Vector3 offset = new Vector3(randomPositionOffset.x, 0, randomPositionOffset.y);

            return _checkpoints[_currentCheckpointIndex].transform.position + offset;
        }

        private void Awake()
        {
            for (int i = 0; i < _checkpoints.Count; i++)
            {
                _checkpoints[i].Init(i);
                _checkpoints[i].OnChecnkpointReached += TryToSetCheckpoint;
            }
        }

        private bool TryToSetCheckpoint(int checkpointIndex)
        {
            bool isNextCheckpoint = _currentCheckpointIndex < checkpointIndex;

            if (isNextCheckpoint)
            {
                _currentCheckpointIndex = checkpointIndex;
            }

            return isNextCheckpoint;
        }
    }
}
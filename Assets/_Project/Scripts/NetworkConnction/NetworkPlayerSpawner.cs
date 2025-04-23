using Assets._Project.Scripts.Gameplay.LevelObjects;
using Assets._Project.Scripts.Player;
using Fusion;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

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
        [SerializeField] private Transform _starterPoint;
        [SerializeField] private List<FlagBehaviour> _checkpoints = new();

        [Header("Spawn Settings")]
        [SerializeField] private float _spawnRadius = 3f;

        private int _currentCheckpointIndex = -1; //No checkpoints on start
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
            _currentCheckpointIndex = -1;
        }

        private Vector3 GetSpawnPosition()
        {
            var randomPositionOffset = Random.insideUnitCircle * _spawnRadius;

            Vector3 offset = new Vector3(randomPositionOffset.x, 0, randomPositionOffset.y);

            Debug.Log(_currentCheckpointIndex);

            if (_currentCheckpointIndex != -1)
                return _checkpoints[_currentCheckpointIndex].transform.position + offset;
                
            return transform.position + offset;
        }

        private void Awake()
        {
            Debug.Log(_checkpoints.Count);

            for (int i = 0; i < _checkpoints.Count; i++)
            {
                int currentIndex = i;
                _checkpoints[i].OnFlagReached.AddListener((v) => 
                {
                    if (v.Id == _localPlayer.Id)
                        OnFlagReached(currentIndex);
                });
            }
        }

        private void OnFlagReached(int index)
        {
            if(index > _currentCheckpointIndex)
                _currentCheckpointIndex = index;
        }
    }
}
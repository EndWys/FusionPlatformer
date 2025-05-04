using Assets._Project.Scripts.EventBus;
using Assets._Project.Scripts.Player;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay
{
    public class LevelRunController : MonoBehaviour
    {
        [SerializeField] private CheckpointTracker _checkpoints;

        [Header("Spawn Settings")]
        [SerializeField] private float _spawnRadius = 3f;

        private PlayerBehaviour _levelRunner;

        public void Init(PlayerBehaviour levelRunner)
        {
            _levelRunner = levelRunner;

            _checkpoints.Init();

            Bus<LevelRunnerFalloutEvent>.OnEvent += OnLevelRunnerFall;

            RespawnLevelRunnerWithProgressReset();
        }

        private void OnLevelRunnerFall(LevelRunnerFalloutEvent evnt)
        {
            RestLevelRunnerPosition();
        }
        private void RestLevelRunnerPosition()
        {
            _levelRunner.Respawn(GetSpawnPosition(), false);
        }

        public void RespawnLevelRunnerWithProgressReset()
        {
            _checkpoints.ResetCheckpoints();
            _levelRunner.Respawn(GetSpawnPosition(), true);
        }

        private Vector3 GetSpawnPosition()
        {
            var randomPositionOffset = Random.insideUnitCircle * _spawnRadius;

            Vector3 offset = new Vector3(randomPositionOffset.x, 0, randomPositionOffset.y);

            return _checkpoints.GetCurrentCheckpointPosition() + offset;
        }

        private void OnDisable()
        {
            Bus<LevelRunnerFalloutEvent>.OnEvent -= OnLevelRunnerFall;
        }
    }
}
using Assets._Project.Scripts.EventBus;
using Assets._Project.Scripts.Player;
using Assets._Project.Scripts.ServiceLocatorSystem;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay
{
    public interface IRunnerRespawner : IService
    {
        public void RestLevelRunnerPosition();
        public void RespawnLevelRunnerWithProgressReset();
    }
    public class LevelRunController : MonoBehaviour, IRunnerRespawner
    {
        [SerializeField] private CheckpointTracker _checkpoints;

        [Header("Spawn Settings")]
        [SerializeField] private float _spawnRadius = 3f;

        private PlayerBehaviour _levelRunner;

        public void Init(PlayerBehaviour levelRunner)
        {
            _levelRunner = levelRunner;

            _checkpoints.Init();

            RespawnLevelRunnerWithProgressReset();
        }

        public void RestLevelRunnerPosition()
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
    }
}
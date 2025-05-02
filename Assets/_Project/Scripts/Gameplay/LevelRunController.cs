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

        private PlayerBehaviour _runner;

        public void Init(PlayerBehaviour runner)
        {
            _runner = runner;

            _checkpoints.Init();

            Bus<PlayerFalloutEvent>.OnEvent += OnRunnerFall;

            RespawnRunnerWithProgressReset();
        }

        private void OnRunnerFall(PlayerFalloutEvent evnt)
        {
            RestRunnerPosition();
        }
        private void RestRunnerPosition()
        {
            _runner.Respawn(GetSpawnPosition(), false);
            _runner.PlaySpawnAnimation();
        }

        public void RespawnRunnerWithProgressReset()
        {
            _checkpoints.ResetCheckpoints();
            _runner.Respawn(GetSpawnPosition(), true);
            _runner.PlaySpawnAnimation();
        }

        private Vector3 GetSpawnPosition()
        {
            var randomPositionOffset = Random.insideUnitCircle * _spawnRadius;

            Vector3 offset = new Vector3(randomPositionOffset.x, 0, randomPositionOffset.y);

            return _checkpoints.GetCurrentCheckpointPosition() + offset;
        }

        private void OnDisable()
        {
            Bus<PlayerFalloutEvent>.OnEvent -= OnRunnerFall;
        }
    }
}
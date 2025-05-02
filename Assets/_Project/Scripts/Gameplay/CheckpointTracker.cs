using Assets._Project.Scripts.Gameplay.LevelObjects;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay
{
    public class CheckpointTracker : MonoBehaviour
    {
        [Header("Ckeckpoins List")]
        [SerializeField] private List<CheckpointBehaviour> _checkpoints = new();

        private int _currentCheckpointIndex = 0;

        public void Init()
        {
            InitializeCheckpoints();
        }

        private void InitializeCheckpoints()
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

        public Vector3 GetCurrentCheckpointPosition()
        {
            return _checkpoints[_currentCheckpointIndex].transform.position;
        }

        public void ResetCheckpoints()
        {
            _currentCheckpointIndex = 0;
        }
    }
}
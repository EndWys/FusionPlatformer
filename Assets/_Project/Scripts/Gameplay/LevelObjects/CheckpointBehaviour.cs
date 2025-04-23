using Fusion;
using UnityEngine;
using UnityEngine.Events;

namespace Assets._Project.Scripts.Gameplay.LevelObjects
{
    public class CheckpointBehaviour : NetworkBehaviour
    {
        [HideInInspector] public UnityEvent<int> OnChecnkpointReached;

        private int _index;

        public void Init(int index)
        {
            _index = index;
        }

        public void CheckpointReached()
        {
            OnChecnkpointReached.Invoke(_index);
        }
    }
}
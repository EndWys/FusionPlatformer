using Assets._Project.Scripts.EventBus;
using Fusion;
using System;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.LevelObjects
{
    public class CheckpointBehaviour : NetworkBehaviour
    {
        [HideInInspector] public event Func<int, bool> OnChecnkpointReached;

        private int _index;

        public void Init(int index)
        {
            _index = index;
        }

        public void CheckpointReached()
        {
            bool isSet = OnChecnkpointReached.Invoke(_index);

            if (isSet)
            {
                Bus<CheckpointReachEvent>.Raise(new() { Posiotion = transform.position });
            }
        }
    }
}
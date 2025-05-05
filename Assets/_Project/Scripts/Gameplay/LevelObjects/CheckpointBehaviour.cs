using Assets._Project.Scripts.EventBus;
using Assets._Project.Scripts.Gameplay.LevelObjects.Base;
using Assets._Project.Scripts.Player;
using System;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.LevelObjects
{
    public class CheckpointBehaviour : PlayerContactLevelObject<IPlayerComponent>
    {
        [HideInInspector] public event Func<int, bool> OnChecnkpointReached;

        private int _index;

        public void Init(int index)
        {
            _index = index;
        }

        protected override bool CheckContactCondition()
        {
            return OnChecnkpointReached.Invoke(_index);
        }

        protected override void ContactAction()
        {
            Bus<CheckpointReachEvent>.Raise(new() { Posiotion = transform.position });
        }
    }
}
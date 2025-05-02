using Assets._Project.Scripts.Gameplay.LevelObjects;
using Assets._Project.Scripts.Player;
using Fusion;
using System;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay
{
    public class LevelController : NetworkBehaviour
    {
        [SerializeField] private FlagBehaviour _finish;

        public event Action<PlayerBehaviour> OnLevelRunnerReachFinish;

        public override void Spawned()
        {
            _finish.OnFlagReached.AddListener(OnFinishReach);
        }

        public void OnFinishReach(PlayerBehaviour runner)
        {
            OnLevelRunnerReachFinish?.Invoke(runner);
        }

        public void PlayMatchFinishLevelAnimation()
        {
            _finish.WinnerReachFinish();
        }

        public void ResetLevelForNewMatch()
        {
            _finish.EnableCrown();
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            _finish.OnFlagReached.RemoveListener(OnFinishReach);
        }
    }
}
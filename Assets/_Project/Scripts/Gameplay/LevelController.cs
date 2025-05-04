using Assets._Project.Scripts.Gameplay.LevelObjects;
using Fusion;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay
{
    public class LevelController : NetworkBehaviour
    {
        [SerializeField] private CrownBehaviour _finish;

        public void PlayMatchFinishLevelAnimation()
        {
            _finish.WinnerReachFinish();
        }

        public void ResetLevelForNewMatch()
        {
            _finish.EnableCrown();
        }
    }
}
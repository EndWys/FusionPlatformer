using UnityEngine;

namespace Assets._Project.Scripts.Gameplay
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Game/GameSettings")]
    public class GameSettings : ScriptableObject
    {
        public static GameSettings Instace => _instace;
        private static GameSettings _instace;

        [field: Header("Match Settings")]
        [field: SerializeField] public int CoinsForFinish { get; private set; } = 4;
        [field: SerializeField] public float GameOverTimeout { get; private set; } = 5f;

        public void Init()
        {
            if (_instace == null)
                _instace = this;
        }
    }
}
using Assets._Project.Scripts.Gameplay;
using Assets._Project.Scripts.UI;
using UnityEngine;

namespace Assets._Project.Scripts.ServiceLocatorSystem
{
    public class ServiceLocatorLoader_Game : MonoBehaviour
    {
        [SerializeField] private MatchManager _matrchManager;
        [SerializeField] private LevelRunController _runController;
        [Header("UI")]
        [SerializeField] private GameLevelUIController _gameLevelUI;
        public void Init()
        {
            ServiceLocator.Init();

            ServiceLocator.Instance.Register<IMatchFinisherHadler>(_matrchManager);
            ServiceLocator.Instance.Register<IRunnerRespawner>(_runController);

            ServiceLocator.Instance.Register<ICoinDisplay>(_gameLevelUI);
            ServiceLocator.Instance.Register<IWinnerDisplay>(_gameLevelUI);
        }
    }
}
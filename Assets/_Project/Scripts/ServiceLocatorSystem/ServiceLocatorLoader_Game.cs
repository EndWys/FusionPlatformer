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

            RegisterAll();
        }

        private void RegisterAll()
        {

            ServiceLocator.Instance.Register<IMatchFinisherHadler>(_matrchManager);
            ServiceLocator.Instance.Register<IRunnerRespawner>(_runController);

            ServiceLocator.Instance.Register<ICoinDisplay>(_gameLevelUI);
            ServiceLocator.Instance.Register<IWinnerDisplay>(_gameLevelUI);
        }

        private void OnDestroy()
        {
            UnregisterAll();
        }

        private void UnregisterAll()
        {

            ServiceLocator.Instance.Unregister<IMatchFinisherHadler>();
            ServiceLocator.Instance.Unregister<IRunnerRespawner>();

            ServiceLocator.Instance.Unregister<ICoinDisplay>();
            ServiceLocator.Instance.Unregister<IWinnerDisplay>();
        }
    }
}
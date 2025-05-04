using Assets._Project.Scripts.Gameplay;
using Assets._Project.Scripts.NetworkConnction;
using Assets._Project.Scripts.ServiceLocatorSystem;
using Assets._Project.Scripts.UI;
using UnityEngine;

namespace Assets._Project.GameInitialization
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private ServiceLocatorLoader_Game _serviceLocatorLoader;
        [SerializeField] private GameSettings _gameSettings;
        [SerializeField] private GameLevelUIController _gameUI;
        [SerializeField] private MenuUIController _menuUI;
        [SerializeField] private ConnectionController _connectionController;

        private void Awake()
        {
            _serviceLocatorLoader.Init();

            _gameSettings.Init();
            _gameUI.Init();
            _menuUI.Init();
            _connectionController.Init();
        }
    }
}
using Assets._Project.Scripts.Gameplay.LevelObjects;
using Assets._Project.Scripts.NetworkConnction;
using Assets._Project.Scripts.Player;
using Assets._Project.Scripts.UI;
using Fusion;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay
{
    public class GameplayController : NetworkBehaviour
    {
        public static GameplayController Instance {  get; private set; }

        [SerializeField] private GameLevelUIController _levelUI;
        [SerializeField] private NetworkPlayerSpawner _playerSpawner;
        [SerializeField] private FlagBehaviour _flag;

        [Header("Settings")]
        [SerializeField] private int _coinsForFinish = 4;
        [SerializeField] private float _gameOverTimeout = 5f;

        [Networked] private PlayerRef Winner { get; set; }
        [Networked] private TickTimer _gameOverTimer { get; set; }
        public bool IsGameFinished => _gameOverTimer.IsRunning;

        public override void Spawned()
        {
            Instance = this;

            _flag.OnFlagReached.AddListener(OnFlagReached);

            _levelUI.Init(_coinsForFinish);
        }

        public override void FixedUpdateNetwork()
        {
            if (_gameOverTimer.Expired(Runner))
            {
                ResetGame();
            }
        }

        public void OnCoinChanged(int amount)
        {
            _levelUI.OnCollectedCoinsChanged(amount);
        }

        private void ResetGame()
        {
            Winner = PlayerRef.None;

            RPC_ShowWinner(false);

            foreach (var playerRef in Runner.ActivePlayers)
            {
                RPC_RespawnPlayer(playerRef);
            }

            _gameOverTimer = default;
        }

        private void OnFlagReached(PlayerBehaviour player)
        {
            if (Winner != PlayerRef.None)
                return;

            if (player.CollectedCoins < _coinsForFinish)
                return;

            OnWin(player);
        }

        private void OnWin(PlayerBehaviour player)
        {
            Winner = player.Object.StateAuthority;
            _gameOverTimer = TickTimer.CreateFromSeconds(Runner, _gameOverTimeout);

            RPC_ShowWinner(true, player.Nickname);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_RespawnPlayer([RpcTarget] PlayerRef playerRef)
        {
            _playerSpawner.RespawnLocalPlayer(true);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_ShowWinner(bool show, string name = "")
        {
            _levelUI.ShowWinner(show, name);
        }
    }
}
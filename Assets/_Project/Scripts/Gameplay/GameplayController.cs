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
        [SerializeField] private float _gameOverTimeout = 5f;

        [Networked,HideInInspector] private PlayerRef Winner { get; set; }
        [Networked] private TickTimer _gameOverTimer { get; set; }
        public bool IsGameFinished => _gameOverTimer.IsRunning;

        public override void Spawned()
        {
            Instance = this;

            _flag.OnFlagReached.AddListener(OnFlagReached);
        }

        public override void FixedUpdateNetwork()
        {
            if (_gameOverTimer.Expired(Runner))
            {
                ResetGame();
            }
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
            RPC_ShowWinner(true);

            if (HasStateAuthority == false)
                return;

            if (Winner != PlayerRef.None)
                return;

            OnWin(player);
        }

        private void OnWin(PlayerBehaviour player)
        {
            Winner = player.Object.StateAuthority;
            _gameOverTimer = TickTimer.CreateFromSeconds(Runner, _gameOverTimeout);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_RespawnPlayer([RpcTarget] PlayerRef playerRef)
        {
            _playerSpawner.RespawnLocalPlayer();
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_ShowWinner(bool show)
        {
            _levelUI.ShowWinner(show, "");
        }
    }
}
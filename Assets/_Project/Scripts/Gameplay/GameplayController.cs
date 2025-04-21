using Assets._Project.Scripts.Player;
using Fusion;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay
{
    public class GameplayController : NetworkBehaviour
    {
        [SerializeField] private FlagBehaviour _flag;
        [Header("Settings")]
        [SerializeField] private float _gameOverTimeout = 5f;

        [Networked,HideInInspector] public PlayerRef Winner { get; set; }
        [Networked] private TickTimer _gameOverTimer { get; set; }

        public PlayerBehaviour LocalPlayer { get; set; }
        public bool IsGameFinished => _gameOverTimer.IsRunning;

        public override void Spawned()
        {
            _flag.OnFlagReached.AddListener(OnFlagReached);
        }

        public override void FixedUpdateNetwork()
        {
            if (_gameOverTimer.Expired(Runner))
            {
                Winner = PlayerRef.None;

                foreach (var playerRef in Runner.ActivePlayers)
                {
                    RPC_RespawnPlayer(playerRef, Vector3.up, true);
                }

                _gameOverTimer = default;
            }
        }

        private void OnFlagReached(PlayerBehaviour player)
        {
            if (HasStateAuthority == false)
                return;

            if (Winner != PlayerRef.None)
                return;

            //if (player.CollectedCoins < MinCoinsToWin)
            //    return; 

            Winner = player.Object.StateAuthority;
            _gameOverTimer = TickTimer.CreateFromSeconds(Runner, _gameOverTimeout);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_RespawnPlayer([RpcTarget] PlayerRef playerRef, Vector3 position, bool resetCoins)
        {
            LocalPlayer.Respawn(position, resetCoins);
        }
    }
}
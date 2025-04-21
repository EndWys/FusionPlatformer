using Assets._Project.Scripts.NetworkConnction;
using Assets._Project.Scripts.Player;
using Fusion;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay
{
    public class GameplayController : NetworkBehaviour
    {
        [SerializeField] private NetworkPlayerSpawner _playerSpawner;
        [SerializeField] private FlagBehaviour _flag;
        [Header("Settings")]
        [SerializeField] private float _gameOverTimeout = 5f;

        [Networked,HideInInspector] private PlayerRef Winner { get; set; }
        [Networked] private TickTimer _gameOverTimer { get; set; }
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
                    RPC_RespawnPlayer(playerRef);
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

            Winner = player.Object.StateAuthority;
            _gameOverTimer = TickTimer.CreateFromSeconds(Runner, _gameOverTimeout);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_RespawnPlayer([RpcTarget] PlayerRef playerRef)
        {
            _playerSpawner.RespawnLocalPlayer();
        }
    }
}
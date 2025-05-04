using Assets._Project.Scripts.EventBus;
using Assets._Project.Scripts.Player;
using Fusion;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay
{
    public class MatchManager : NetworkBehaviour
    {
        public static bool IsMatchFinished = false;

        [SerializeField] private LevelRunController _runController;
        [SerializeField] private LevelController _levelController;

        [Networked] private PlayerRef _winner { get; set; }
        [Networked] private TickTimer _matchReloadTimer { get; set; }

        public override void Spawned()
        {
            _levelController.OnLevelRunnerReachFinish += TryToWin;
        }

        private void TryToWin(PlayerBehaviour levelRunner)
        {

            if (_winner != PlayerRef.None)
                return;

            if (!levelRunner.IsEnoghtCoin(GameSettings.Instace.CoinsForFinish))
                return;

            Win(levelRunner);
        }

        private void Win(PlayerBehaviour levelRunner)
        {
            _winner = levelRunner.Object.StateAuthority;
            _matchReloadTimer = TickTimer.CreateFromSeconds(Runner, GameSettings.Instace.GameOverTimeout);

            IsMatchFinished = true;

            RPC_ShowMatchFinishForAllClients(levelRunner.GetNickname());
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_ShowMatchFinishForAllClients(string name)
        {
            Bus<SomeoneWinEvent>.Raise(new() { WinnerName = name });
            _levelController.PlayMatchFinishLevelAnimation();
        }

        public override void FixedUpdateNetwork()
        {
            if (_matchReloadTimer.Expired(Runner))
            {
                ResetMatch();
            }
        }

        private void ResetMatch()
        {
            _winner = PlayerRef.None;

            RPC_ResetMatchForAllClients();

            _matchReloadTimer = default;

            IsMatchFinished = false;
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_ResetMatchForAllClients()
        {
            _levelController.ResetLevelForNewMatch();
            _runController.RespawnLevelRunnerWithProgressReset();

            Bus<MatchReloadEvent>.Raise(new());
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            _levelController.OnLevelRunnerReachFinish -= TryToWin;
        }
    }
}
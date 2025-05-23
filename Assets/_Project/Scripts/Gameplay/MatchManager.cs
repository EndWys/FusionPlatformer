using Assets._Project.Scripts.Player;
using Assets._Project.Scripts.ServiceLocatorSystem;
using Assets._Project.Scripts.UI;
using Fusion;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay
{
    public interface IMatchFinisherHadler : IService
    {
        public bool IsMatchFinished { get; }
        public bool TryToFinishMatch(PlayerBehaviour levelRunner);
    }
    public class MatchManager : NetworkBehaviour, IMatchFinisherHadler
    {
        [SerializeField] private LevelController _levelController;

        [Networked] private PlayerRef _winner { get; set; }
        [Networked] private TickTimer _matchReloadTimer { get; set; }

        public bool IsMatchFinished => _isSpawned && _matchReloadTimer.IsRunning;

        private bool _isSpawned = false;
        public override void Spawned()
        {
            _isSpawned = true;
        }

        public bool TryToFinishMatch(PlayerBehaviour levelRunner)
        {
            if (_winner != PlayerRef.None)
                return false;

            if (!levelRunner.IsEnoghtCoin(GameSettings.Instace.CoinsForFinish))
                return false;

            Win(levelRunner);

            return true;
        }

        private void Win(PlayerBehaviour levelRunner)
        {
            _winner = levelRunner.Object.StateAuthority;
            _matchReloadTimer = TickTimer.CreateFromSeconds(Runner, GameSettings.Instace.GameOverTimeout);

            RPC_ShowMatchFinishForAllClients(levelRunner.GetNickname());
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_ShowMatchFinishForAllClients(string name)
        {
            _levelController.PlayMatchFinishLevelAnimation();
            ServiceLocator.Instance.Get<IWinnerDisplay>().ShowWinner(true, name);
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
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_ResetMatchForAllClients()
        {
            _levelController.ResetLevelForNewMatch();

            ServiceLocator.Instance.Get<IWinnerDisplay>().ShowWinner(false);
            ServiceLocator.Instance.Get<IRunnerRespawner>().RespawnLevelRunnerWithProgressReset();
        }
    }
}
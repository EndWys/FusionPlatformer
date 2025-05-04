using Assets._Project.Scripts.EventBus;
using Fusion;

namespace Assets._Project.Scripts.Player.PlayerComponents
{
    public class PlayerWallet : NetworkBehaviour, IWalletHolder
    {
        [Networked] private int _collectedCoins { get; set; }

        public void AddCoinsToWallet(int addedCount)
        {
            _collectedCoins += addedCount;
            Bus<CoinsCountChangeEvent>.Raise(new() { Count = _collectedCoins });
        }

        public void SetCoinCount(int count)
        {
            _collectedCoins = count;
            Bus<CoinsCountChangeEvent>.Raise(new() { Count = _collectedCoins });
        }

        public bool IsEnoghtCoin(int requiredCoin)
        {
            return _collectedCoins >= requiredCoin;
        }
    }
}
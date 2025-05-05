using Assets._Project.Scripts.ServiceLocatorSystem;
using Assets._Project.Scripts.UI;
using Fusion;

namespace Assets._Project.Scripts.Player.PlayerComponents
{
    public class PlayerWallet : NetworkBehaviour, IWalletHolder
    {
        [Networked] private int _collectedCoins { get; set; }

        private ICoinDisplay _coinDisplay;

        public override void Spawned()
        {
            _coinDisplay = ServiceLocator.Instance.Get<ICoinDisplay>();
        }

        public void AddCoinsToWallet(int addedCount)
        {
            _collectedCoins += addedCount;
            _coinDisplay.ChangeDisplayedCollectedCoinCount(_collectedCoins);
        }

        public void SetCoinCount(int count)
        {
            _collectedCoins = count;
            _coinDisplay.ChangeDisplayedCollectedCoinCount(_collectedCoins);
        }

        public bool IsEnoghtCoin(int requiredCoin)
        {
            return _collectedCoins >= requiredCoin;
        }
    }
}
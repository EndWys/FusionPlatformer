using Assets._Project.Scripts.Player.PlayerComponents;
using Assets._Project.Scripts.UI;
using Fusion;
using UnityEngine;

namespace Assets._Project.Scripts.Player
{
    public class PlayerBehaviour : NetworkBehaviour, IJumppadActor, IWalletHolder, INicknameHolder
    {
        [SerializeField] private PlayerMovement _movement;
        [SerializeField] private PlayerWallet _wallet;
        [SerializeField] private PlayerNameplate _nameplate;
        [SerializeField] private PlayerNameHolder _nameHolder;

        public void Respawn(Vector3 position, bool resetCoins)
        {
            if (resetCoins)
            {
                SetCoinCount(0);
            } 

            _movement.Respawn(position);
        }

        public void AddCoinsToWallet(int addedCount)
        {
            _wallet.AddCoinsToWallet(addedCount);
        }
        public void SetCoinCount(int count)
        {
            _wallet.SetCoinCount(count);
        }

        public bool IsEnoghtCoin(int requiredCoin)
        {
            return _wallet.IsEnoghtCoin(requiredCoin);
        }

        public void BounceFromJumppad(float impulsePower)
        {
            _movement.BounceFromJumppad(impulsePower);
        }

        public string GetNickname()
        {
            return _nameHolder.GetNickname();
        }
    }
}
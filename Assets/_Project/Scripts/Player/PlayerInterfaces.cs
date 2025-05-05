namespace Assets._Project.Scripts.Player
{
    public interface IPlayerComponent { }

    public interface IJumppadActor : IPlayerComponent
    {
        public void BounceFromJumppad(float impulsePower);
    }

    public interface IWalletHolder : IPlayerComponent
    {
        public void AddCoinsToWallet(int addedCount);
        public void SetCoinCount(int count);

        public bool IsEnoghtCoin(int requiredCoin);
    }

    public interface INicknameHolder : IPlayerComponent
    {
        public string GetNickname();
    }
}
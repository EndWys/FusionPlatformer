using Assets._Project.Scripts.EventBus;
using Assets._Project.Scripts.Gameplay;
using Assets._Project.Scripts.ServiceLocatorSystem;
using TMPro;
using UnityEngine;

namespace Assets._Project.Scripts.UI
{
    public interface ICoinDisplay : IService
    {
        public void ChangeDisplayedCollectedCoinCount(int count);
    }

    public interface IWinnerDisplay : IService
    {
        public void ShowWinner(bool show, string name = "");
    }

    public class GameLevelUIController : MonoBehaviour, ICoinDisplay, IWinnerDisplay
    {
        [SerializeField] private CanvasGroup _root;
        [SerializeField] private TextMeshProUGUI _tipText;
        [SerializeField] private TextMeshProUGUI _coinsText;
        [SerializeField] private TextMeshProUGUI _winnerText;

        public void Init()
        {
            ChangeDisplayedCollectedCoinCount(0);

            ShowWinner(false);

            _root.alpha = 1f;
        }

        public void ChangeDisplayedCollectedCoinCount(int count)
        {
            int targetCoinsAmount = GameSettings.Instace.CoinsForFinish;

            if (count < targetCoinsAmount)
                _tipText.text = $"Collect {targetCoinsAmount} coins!";
            else
                _tipText.text = "Run to the FINISH!";

            _coinsText.text = $"\u00d7{count}";
        }

        public void ShowWinner(bool show, string name = "")
        {
            _winnerText.gameObject.SetActive(show);
            _winnerText.text = $"We have a winner!\n{name}";
        }
    }
}
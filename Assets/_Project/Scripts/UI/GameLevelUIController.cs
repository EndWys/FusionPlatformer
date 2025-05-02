using Assets._Project.Scripts.EventBus;
using Assets._Project.Scripts.Gameplay;
using TMPro;
using UnityEngine;

namespace Assets._Project.Scripts.UI
{
    public class GameLevelUIController : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _root;
        [SerializeField] private TextMeshProUGUI _tipText;
        [SerializeField] private TextMeshProUGUI _coinsText;
        [SerializeField] private TextMeshProUGUI _winnerText;

        public void Init()
        {
            Bus<CoinsCountChangeEvent>.OnEvent += OnCollectedCoinsChanged;
            Bus<SomeoneWinEvent>.OnEvent += OnSomeoneWin;
            Bus<MatchReloadEvent>.OnEvent += OnMatchReload;

            OnCollectedCoinsChanged(0);

            ShowWinner(false);

            _root.alpha = 1f;
        }

        private void OnCollectedCoinsChanged(CoinsCountChangeEvent evnt)
        {
            OnCollectedCoinsChanged(evnt.Count);
        }

        private void OnCollectedCoinsChanged(int count)
        {
            int targetCoinsAmount = GameSettings.Instace.CoinsForFinish;

            if (count < targetCoinsAmount)
                _tipText.text = $"Collect {targetCoinsAmount} coins!";
            else
                _tipText.text = "Run to the FINISH!";

            _coinsText.text = $"\u00d7{count}";
        }

        private void OnSomeoneWin(SomeoneWinEvent evnt)
        {
            ShowWinner(true, evnt.WinnerName);
        }

        private void OnMatchReload(MatchReloadEvent evnt)
        {
            ShowWinner(false);
        }

        private void ShowWinner(bool show, string name = "")
        {
            _winnerText.gameObject.SetActive(show);
            _winnerText.text = $"We have a winner!\n{name}";
        }

        private void OnDisable()
        {
            Bus<CoinsCountChangeEvent>.OnEvent -= OnCollectedCoinsChanged;
            Bus<SomeoneWinEvent>.OnEvent -= OnSomeoneWin;
            Bus<MatchReloadEvent>.OnEvent -= OnMatchReload;
        }
    }
}
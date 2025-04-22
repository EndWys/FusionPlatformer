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

        private int _targetCoinsAmount;

        public void Init(int targetAmount)
        {
            _targetCoinsAmount = targetAmount;
            OnCollectedCoinsChanged(0);

            ShowWinner(false);

            _root.alpha = 1f;
        }

        public void ShowWinner(bool show, string name = "")
        {
            _winnerText.gameObject.SetActive(show);
            _winnerText.text = $"We have a winner!\n{name}";
        }

        public void OnCollectedCoinsChanged(int amount)
        {
            if(amount < _targetCoinsAmount)
                _tipText.text = $"Collect {_targetCoinsAmount} coins!";
            else
                _tipText.text = "Run to the FINISH!";

            _coinsText.text = $"\u00d7{amount}";
        }
    }
}
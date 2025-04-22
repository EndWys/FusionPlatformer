using Assets._Project.Scripts.Player;
using Fusion;
using TMPro;
using UnityEngine;

namespace Assets._Project.Scripts.UI
{
    public class GameLevelUIController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _winnerText;

        public void ShowWinner(bool show, string name = "")
        {
            _winnerText.gameObject.SetActive(show);
            _winnerText.text = $"We have a winner!\n{name}";
        }
    }
}
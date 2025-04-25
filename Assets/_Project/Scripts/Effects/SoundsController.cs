using Assets._Project.Scripts.EventBus;
using UnityEngine;

namespace Assets._Project.Scripts.Effects
{
    public class SoundsController : MonoBehaviour
    {
        [SerializeField] private SoundsData _soundsData;
        private void OnEnable()
        {
            Bus<CoinDisapearEvent>.OnEvent += PlayCoinSound;
        }
        private void OnDisable()
        {
            Bus<CoinDisapearEvent>.OnEvent -= PlayCoinSound;
        }

        private void PlayCoinSound(CoinDisapearEvent evnt)
        {
            AudioSource.PlayClipAtPoint(_soundsData.CoinCollect, evnt.Posiotion, 1f);
        }
    }
}
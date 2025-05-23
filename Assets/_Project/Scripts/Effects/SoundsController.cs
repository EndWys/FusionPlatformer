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
            Bus<CloudDisapearEvent>.OnEvent += PlayCloudSound;
            Bus<PlatformFallEvent>.OnEvent += PlayPlatformFallSound;
            Bus<CheckpointReachEvent>.OnEvent += PlayCheckpointSound;
            Bus<CrownReachEvent>.OnEvent += PlayCrowntSound;
            Bus<LevelRunnerFalloutEvent>.OnEvent += PlayPlayerFall;
        }
        private void OnDisable()
        {
            Bus<CoinDisapearEvent>.OnEvent -= PlayCoinSound;
            Bus<CloudDisapearEvent>.OnEvent -= PlayCloudSound;
            Bus<PlatformFallEvent>.OnEvent -= PlayPlatformFallSound;
            Bus<CheckpointReachEvent>.OnEvent -= PlayCheckpointSound;
            Bus<CrownReachEvent>.OnEvent -= PlayCrowntSound;
            Bus<LevelRunnerFalloutEvent>.OnEvent -= PlayPlayerFall;
        }

        private void PlayCoinSound(CoinDisapearEvent evnt)
        {
            AudioSource.PlayClipAtPoint(_soundsData.CoinCollect, evnt.Posiotion, 1f);
        }

        private void PlayCloudSound(CloudDisapearEvent evnt)
        {
            AudioSource.PlayClipAtPoint(_soundsData.FallSound, evnt.Posiotion, 1f);
        }

        private void PlayPlatformFallSound(PlatformFallEvent evnt)
        {
            AudioSource.PlayClipAtPoint(_soundsData.FallSound, evnt.Posiotion, 1f);
        }

        private void PlayCheckpointSound(CheckpointReachEvent evnt)
        {
            AudioSource.PlayClipAtPoint(_soundsData.CoinCollect, evnt.Posiotion, 1f);
        }

        private void PlayCrowntSound(CrownReachEvent evnt)
        {
            AudioSource.PlayClipAtPoint(_soundsData.CoinCollect, evnt.Posiotion, 1f);
        }

        private void PlayPlayerFall(LevelRunnerFalloutEvent evnt)
        {
            AudioSource.PlayClipAtPoint(_soundsData.FallSound, evnt.Posiotion, 1f);
        }
    }
}
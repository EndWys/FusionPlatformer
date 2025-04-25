using Assets._Project.Scripts.EventBus;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Scripts.Effects
{
    public class VFXController : MonoBehaviour
    {
        [SerializeField] private VFXData _vfxData;

        private Dictionary<string, ParticleSystem> _chacedVFX = new();

        private void OnEnable()
        {
            Bus<CoinDisapearEvent>.OnEvent += PlayCoinEffect;
        }

        private void OnDisable()
        {
            Bus<CoinDisapearEvent>.OnEvent -= PlayCoinEffect;
        }


        private void PlayCoinEffect(CoinDisapearEvent evnt)
        {
            Play(_vfxData.CoinParticles, evnt.Posiotion, 10);
        }

        private void Play(ParticleSystem prefab, Vector3 position, int emitCount)
        {
            string name = prefab.name;
            ParticleSystem currentParticles = null;

            if (_chacedVFX.TryGetValue(name, out ParticleSystem particles))
            {
                currentParticles = particles;
                currentParticles.transform.position = position;
            }
            else
            {
                currentParticles = Instantiate(_vfxData.CoinParticles, position, Quaternion.identity);
                _chacedVFX.Add(name, currentParticles);
            }

            if (currentParticles == null)
                return;

            currentParticles.Emit(emitCount);
        }
    }
}
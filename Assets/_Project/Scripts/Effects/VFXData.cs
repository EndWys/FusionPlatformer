using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Scripts.Effects
{
    [CreateAssetMenu(fileName = "VFXData", menuName = "Effects/VFXData")]
    public class VFXData : ScriptableObject
    {
        public ParticleSystem CoinParticles;
    }
}
using UnityEngine;

namespace Assets._Project.Scripts.Effects
{
    [CreateAssetMenu(fileName = "SoundsData", menuName = "Effects/SoundsData")]
    public class SoundsData : ScriptableObject
    {
        public AudioClip CoinCollect; 
        public AudioClip FallSound;
    }
}
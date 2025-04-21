using Fusion;
using UnityEngine;

namespace Assets._Project.Scripts.NetworkConnction
{
    public class NetworkPlayerSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _playerPrefab;

        public void SpawnPlayer(NetworkRunner runner, PlayerRef player)
        {
            runner.Spawn(_playerPrefab, new Vector3(0, 1, 0), Quaternion.identity, player);
        }
    }
}
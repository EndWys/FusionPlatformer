using Assets._Project.Scripts.UI;
using Fusion;
using UnityEngine;

namespace Assets._Project.Scripts.Player.PlayerComponents
{
    public class PlayerNameHolder : NetworkBehaviour, INicknameHolder
    {
        [SerializeField] private PlayerNameplate _nameplate;

        [Networked, HideInInspector, Capacity(24), OnChangedRender(nameof(OnNicknameChanged))]
        private string _nickname { get; set; }

        public override void Spawned()
        {
            if (HasStateAuthority)
            {
                // Set player nickname that is saved in UIGameMenu
                _nickname = PlayerPrefs.GetString("PlayerName");
            }

            // In case the nickname is already changed,
            // we need to trigger the change manually
            OnNicknameChanged();
        }

        public string GetNickname()
        {
            return _nickname;
        }

        private void OnNicknameChanged()
        {
            if (HasStateAuthority)
                return; // Do not show nickname for local player

            _nameplate.SetNickname(_nickname);
        }
    }
}
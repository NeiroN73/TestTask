using Mirror;
using TMPro;
using UnityEngine;
using System;

namespace Game.Components
{
    public class ChangeNameComponent : CreatureComponent
    {
        [SerializeField] private TMP_Text _nameText;
        
        [SyncVar(hook = nameof(OnUsernameChanged))]
        private string _userName;

        public string UserName => _userName;
        
        public override void OnStartClient()
        {
            if (!string.IsNullOrEmpty(_userName))
            {
                UpdateNameView(_userName);
            }
        }

        public override void OnStartServer()
        {
            if (string.IsNullOrEmpty(_userName))
            {
                SetUsername(null);
            }
        }
        
        [ClientRpc]
        public void Init(string username)
        {
            if (isServer)
            {
                SetUsername(username);
            }
            else if (authority)
            {
                CmdSetUsername(username);
            }
        }
        
        [Server]
        private void SetUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                username = GenerateRandomName();
            }
            
            _userName = username;
        }
        
        [Command]
        private void CmdSetUsername(string newUsername)
        {
            SetUsername(newUsername);
        }
        
        private string GenerateRandomName()
        {
            var guidPart = Guid.NewGuid();
            return $"Player_{guidPart}";
        }

        private void OnUsernameChanged(string oldValue, string newValue)
        {
            UpdateNameView(newValue);
        }
        
        private void UpdateNameView(string name)
        {
            if (_nameText != null)
            {
                _nameText.text = name;
            }
        }
    }
}
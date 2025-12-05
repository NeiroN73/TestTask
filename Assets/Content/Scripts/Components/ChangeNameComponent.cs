using TMPro;
using UnityEngine;
using System;
using FishNet.Object;
using FishNet.Object.Synchronizing;

namespace Game.Components
{
    public class ChangeNameComponent : EntityComponent
    {
        [SerializeField] private TMP_Text _nameText;
        
        //[SyncVar(OnChange = nameof(OnUsernameChanged))]
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
        
        [ObserversRpc]
        public void Init(string username)
        {
            if (IsServerInitialized)
            {
                SetUsername(username);
            }
            else if (IsController)
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
        
        [ServerRpc]
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
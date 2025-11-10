using Mirror;
using R3;
using UnityEngine;

namespace Game.Components
{
    public class SendDebugComponent : CreatureComponent
    {
        private ChangeNameComponent _changeNameComponent;

        public void Init()
        {
            var controllerComponent = Creature.GetCreatureComponentByType<ControllerComponent>();
            controllerComponent.DebugPerformed.Subscribe(SendChatMessage).AddTo(Disposable);
            
            _changeNameComponent = Creature.GetCreatureComponentByType<ChangeNameComponent>();;
        }
        
        private void SendChatMessage()
        {
            if (isServer)
            {
                RpcReceiveChatMessage(_changeNameComponent.UserName);
            }
            else
            {
                CmdSendChatMessage(_changeNameComponent.UserName);
            }
        }
        
        [Command]
        private void CmdSendChatMessage(string sender)
        {
            RpcReceiveChatMessage(sender);
        }
        
        [ClientRpc]
        private void RpcReceiveChatMessage(string sender)
        {
            Debug.Log($"Привет от {sender}");
        }
    }
}
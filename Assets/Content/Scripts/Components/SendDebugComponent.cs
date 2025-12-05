
using FishNet.Object;
using UnityEngine;

namespace Game.Components
{
    public class SendDebugComponent : EntityComponent
    {
        private ChangeNameComponent _changeNameComponent;

        public void Init()
        {
            //var controllerComponent = Entity.GetEntityComponentByType<ControllerComponentParent>();
            //controllerComponent.DebugPerformed.Subscribe(SendChatMessage).AddTo(Disposable);
            
            _changeNameComponent = Entity.GetEntityComponentByType<ChangeNameComponent>();;
        }
        
        private void SendChatMessage()
        {
            if (IsServerInitialized)
            {
                RpcReceiveChatMessage(_changeNameComponent.UserName);
            }
            else
            {
                CmdSendChatMessage(_changeNameComponent.UserName);
            }
        }
        
        [ServerRpc]
        private void CmdSendChatMessage(string sender)
        {
            RpcReceiveChatMessage(sender);
        }
        
        [ObserversRpc]
        private void RpcReceiveChatMessage(string sender)
        {
            Debug.Log($"Привет от {sender}");
        }
    }
}
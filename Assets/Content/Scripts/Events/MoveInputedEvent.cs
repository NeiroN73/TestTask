using FishNet.Broadcast;
using FishNet.Connection;
using UnityEngine;

namespace Game.Events
{
    public struct MoveInputedEvent : IBroadcast
    {
        public NetworkConnection NetworkConnection;
        public Vector3 Direction;
    }
}
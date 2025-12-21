using FishNet.Broadcast;
using UnityEngine;

namespace Game.Events
{
    public struct MovedOnServerEvent : IBroadcast
    {
        public bool IsMoved;
        public Vector3 Position;
        public Quaternion Rotation;
    }
}
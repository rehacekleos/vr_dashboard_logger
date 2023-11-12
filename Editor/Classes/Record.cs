using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Editor.Classes
{
    [Serializable]
    public class Record
    {
        public DateTime timestamp;
        public int tick;
        public string environment;
        [CanBeNull] public PositionAndRotation head;
        [CanBeNull] public PositionAndRotation left_hand;
        [CanBeNull] public PositionAndRotation right_hand;
        [CanBeNull] public string custom_data;
        [CanBeNull] public string[] events;

        public Record(DateTime timestamp, int tick, string environment, [CanBeNull] PositionAndRotation head, [CanBeNull] PositionAndRotation leftHand, [CanBeNull] PositionAndRotation rightHand, [CanBeNull] string customData, [CanBeNull] string[] events)
        {
            this.timestamp = timestamp;
            this.tick = tick;
            this.environment = environment;
            this.head = head;
            left_hand = leftHand;
            right_hand = rightHand;
            custom_data = customData;
            this.events = events;
        }
    }
}

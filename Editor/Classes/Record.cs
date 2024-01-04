using System;
using JetBrains.Annotations;
using UnityEngine;
using Object = System.Object;

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
        [CanBeNull] public Object custom_data;
        [CanBeNull] public string[] events;

        public Record(DateTime timestamp, int tick, string environment, [CanBeNull] PositionAndRotation head, [CanBeNull] PositionAndRotation leftHand, [CanBeNull] PositionAndRotation rightHand, [CanBeNull] Object customData, [CanBeNull] string[] events)
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
        
        public override string ToString()
        {
            return string.Format(
                "[Record: timestamp={0}, tick={1}, environment={2}, head={3}, left_hand={4}, right_hand={5}, custom_data={6}, events={7}]",
                timestamp, tick, environment, head, left_hand, right_hand, custom_data ?? "null", (object)events ?? "null");
        }
    }
}

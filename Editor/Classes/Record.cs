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
        [CanBeNull] public PositionAndRotation left_eye;
        [CanBeNull] public PositionAndRotation right_eye;
        [CanBeNull] public Object custom_data;
        [CanBeNull] public string[] events;

        public Record(DateTime timestamp, int tick, string environment, [CanBeNull] PositionAndRotation head,
            [CanBeNull] PositionAndRotation leftHand, [CanBeNull] PositionAndRotation rightHand,
            [CanBeNull] PositionAndRotation leftEye, [CanBeNull] PositionAndRotation rightEye,
            [CanBeNull] Object customData, [CanBeNull] string[] events)
        {
            this.timestamp = timestamp;
            this.tick = tick;
            this.environment = environment;
            this.head = head;
            left_hand = leftHand;
            right_hand = rightHand;
            left_eye = leftEye;
            right_eye = rightEye;
            custom_data = customData;
            this.events = events;
        }

        public override string ToString()
        {
            return string.Format(
                "[Record: timestamp={0}, tick={1}, environment={2}, head={3}, left_hand={4}, right_hand={5}, left_eye={6}, right_eye={7}, custom_data={8}, events={9}]",
                timestamp, tick, environment, head, (object)left_hand ?? "null", (object)right_hand ?? "null",
                (object)left_eye ?? "null", (object)right_eye ?? "null", custom_data ?? "null",
                (object)events ?? "null");
        }
    }
}
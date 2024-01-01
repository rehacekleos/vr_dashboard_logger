using System;
using UnityEngine;

namespace Editor.Classes
{
    [Serializable]
    public class PositionAndRotation
    {
        public Axis position;
        public Axis rotation;

        public PositionAndRotation(Axis position, Axis rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }
        
        public static PositionAndRotation GetPositionAndRotation(GameObject obj)
        {
             var position = obj.transform.position;
             var positionAxis = new Axis(position.x, position.y, position.z);

             var rotation = obj.transform.eulerAngles;
             var rotationAxis = new Axis(rotation.x, rotation.y, rotation.z);

             return new PositionAndRotation(positionAxis, rotationAxis);
        }
        
        public static PositionAndRotation GetLocalPositionAndRotation(GameObject obj)
        {
            var position = obj.transform.localPosition;
            var positionAxis = new Axis(position.x, position.y, position.z);

            var rotation = obj.transform.localEulerAngles;
            var rotationAxis = new Axis(rotation.x, rotation.y, rotation.z);

            return new PositionAndRotation(positionAxis, rotationAxis);
        }
        
        public override string ToString()
        {
            return string.Format("[PositionAndRotation: position={0}, rotation={1}]", (object)position ?? "null", (object)rotation ?? "null");
        }
    }
}

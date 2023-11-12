using System;
using UnityEngine;

namespace Editor.Classes
{
    [Serializable]
    public class Axis
    {
        public float x;
        public float y;
        public float z;

        public Axis(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}

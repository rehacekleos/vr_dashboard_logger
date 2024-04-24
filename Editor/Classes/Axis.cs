using System;

namespace VrDashboardLogger.Editor.Classes
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
        
        public override string ToString()
        {
            return string.Format("[Axis: x={0}, y={1}, z={2}]", x, y, z);
        }
    }
}

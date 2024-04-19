using UnityEngine;

namespace Editor
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                return instance;
            }
        }

        
        private void Awake()
        {
            if (instance == null)
            {
                instance = (T)this;
            }
        }
    }
}
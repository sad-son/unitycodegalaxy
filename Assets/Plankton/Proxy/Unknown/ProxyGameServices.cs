#if !UNITY_IOS && !UNITY_ANDROID
using UnityEngine;

namespace Plankton.Proxy
{
    public class ProxyGameServices : Proxy
    {
        private const string logName = "[Plankton] [PlayServices]";

        public void SignIn(bool forceInteractive = false, string serverClientId = "")
        {
            Debug.Log($"{logName} calling SignIn forceInteractive:{forceInteractive} serverClientId:{serverClientId}");
        }
        
        public void SignOut()
        {
            Debug.Log($"{logName} calling SignOut");
        }

        public void Load(string filename)
        {
            Debug.Log($"{logName} calling Load filename:{filename}");
        }

        public void Save(string filename, string data, string description)
        {
            Debug.Log($"{logName} calling Save filename:{filename} data:{data} description:{description}");
        }
    }
}
#endif
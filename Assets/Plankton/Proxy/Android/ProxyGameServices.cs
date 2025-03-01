#if UNITY_ANDROID
using UnityEngine;

namespace Plankton.Proxy
{
    public class ProxyGameServices : Proxy
    {
        private const string logName = "[Plankton] [PlayServices]";

        public void SignIn(bool forceInteractive = false, string serverClientId = "")
        {
            Debug.Log($"{logName} calling SignIn forceInteractive:{forceInteractive} serverClientId:{serverClientId}");
            FreeVersion.NotAvailable(logName);
        }
        
        public void SignOut()
        {
            Debug.Log($"{logName} calling SignOut");
            FreeVersion.NotAvailable(logName);
        }

        public void Load(string filename)
        {
            Debug.Log($"{logName} calling Load filename:{filename}");
            FreeVersion.NotAvailable(logName);
        }

        public void Save(string filename, string data, string description)
        {
            Debug.Log($"{logName} calling Save filename:{filename} data:{data} description:{description}");
            FreeVersion.NotAvailable(logName);
        }
    }
}
#endif
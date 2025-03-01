#if UNITY_IOS
using UnityEngine;

namespace Plankton.Proxy
{
    public class ProxyGameServices : Proxy
    {
        private const string logName = "[Plankton] [GameCenter]";

        public void SignIn(bool androidForceInteractive = false, string androidServerClientId = "")
        {
            Debug.Log($"{logName} SignIn");
            FreeVersion.NotAvailable();
        }

        public void SignOut()
        {
            // Apple's Game Center doesn't provide a way to sign-out programmatically
        }

        public void Load(string fileName)
        {
            Debug.Log($"{logName} Load fileName:{fileName}");
            FreeVersion.NotAvailable();
        }

        public void Save(string fileName, string data, string description)
        {
            Debug.Log($"{logName} Save fileName:{fileName} data:{data}");
            FreeVersion.NotAvailable();
        }
    }
}

#endif
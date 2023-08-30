using UnityEngine;

namespace SilverTau.Sample
{
    public class OpenURL : MonoBehaviour
    {
        private void Start()
        {
        
        }

        public void OpenLink(string url)
        {
            if(string.IsNullOrEmpty(url)) return;
            Application.OpenURL(url);
        }
    }
}
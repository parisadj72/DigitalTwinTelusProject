using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SilverTau.Sample
{
    public class ScanCell : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI txtTitle;
        [SerializeField] private TextMeshProUGUI txtDate;
        public Button button;

        private Scan _scan;
        
        private void Start()
        {
        }

        public void Init(Scan inputScan)
        {
            txtTitle.text = inputScan.name;
            txtDate.text = inputScan.creationTime;
            _scan = inputScan;
        }
    }
}

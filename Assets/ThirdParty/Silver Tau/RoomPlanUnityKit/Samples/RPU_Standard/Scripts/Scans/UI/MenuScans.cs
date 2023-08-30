using System.Collections.Generic;
using UnityEngine;

namespace SilverTau.Sample
{
    public class MenuScans : MonoBehaviour
    {
        [SerializeField] private MyScans myScans;
        [SerializeField] private ModelViewer modelViewer;
        [SerializeField] private GameObject menuEmpty;
        [SerializeField] private Transform container;
        [SerializeField] private ScanCell prefabScanCell;
        
        private List<ScanCell> _cells = new List<ScanCell>();
        private List<Scan> _scans = new List<Scan>();
        
        private void Start()
        {
        }

        private void OnEnable()
        {
            _scans.Clear();
            _scans = myScans.Scans;

            if (_scans.Count == 0)
            {
                if(menuEmpty) menuEmpty.SetActive(true);
                return;
            }
            
            if(menuEmpty) menuEmpty.SetActive(false);
            CreateCells();
        }

        private void OnDisable()
        {
            Dispose();
        }

        private void CreateCells()
        {
            _cells.Clear();
            
            foreach (var scan in _scans)
            {
                var cell = Instantiate(prefabScanCell, container);
                cell.Init(scan);
                cell.button.onClick.AddListener(() =>
                {
                    modelViewer.Open(scan);
                });
                _cells.Add(cell);
            }
        }
        
        private void Dispose()
        {
            foreach (var cell in _cells)
            {
                if(cell == null) continue;
                Destroy(cell.gameObject);
            }
            
            _cells.Clear();
        }
    }
}

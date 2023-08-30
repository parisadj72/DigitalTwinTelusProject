using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;

namespace SilverTau.Sample
{
    /// <summary>
    /// The Scan class with parameters.
    /// </summary>
    [Serializable]
    public class Scan
    {
        public string id;
        public string name;
        public string creationTime;
        public string modifyTime;
        public string directoryPath;
        public string jsonPath;
        public string usdzPath;
        public string snapshot;
    }
    
    /// <summary>
    /// Sorting type of the scan list.
    /// </summary>
    [Serializable]
    public enum SortType
    {
        None = 0,
        Name = 1,
        CreationTime = 2,
        ModifyTime = 4
    }
    
    public class MyScans : MonoBehaviour
    {
        [SerializeField] private string nameScanDirectory = "model-viewer";
        [SerializeField] private SortType sortType = SortType.None;
        [SerializeField] private bool reverseList = false;
        
        public List<Scan> Scans => GetScans();
        
        [Header("Debug")]
        [Space(10)]
        [SerializeField] private bool debug;
        [SerializeField] private bool onlyEditor;
        
        private List<Scan> _scans = new List<Scan>();
        
        private void Start()
        {
            if (debug)
            {
                if (onlyEditor)
                {
                    if (Application.isEditor)
                    {
                        DebugScans();
                    }
                }
                else
                {
                    DebugScans();
                }
            }
            
            var folderPath = Path.Combine(Application.persistentDataPath, nameScanDirectory);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }
        
        /// <summary>
        /// A method that uploads scans for testing.
        /// </summary>
        private void DebugScans()
        {
            var folderPath = Path.Combine(Application.persistentDataPath, nameScanDirectory);
            var debugPath = Path.Combine(Application.streamingAssetsPath, nameScanDirectory);

            if (!Directory.Exists(debugPath)) return;
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            foreach (var directory in Directory.GetDirectories(debugPath))
            {
                if(string.IsNullOrEmpty(directory)) continue;
                if(Path.GetExtension(directory) == ".meta") continue;
                if(Path.GetExtension(directory) == ".DS_Store") continue;
                        
                var cloneDirectoryPath = Path.Combine(folderPath, Path.GetFileName(directory));
                if(Directory.Exists(cloneDirectoryPath)) continue;
                    
                Directory.CreateDirectory(cloneDirectoryPath);

                foreach (var file in Directory.GetFiles(directory))
                {
                    if(string.IsNullOrEmpty(file)) continue;
                    if(Path.GetExtension(file) == ".meta") continue;
                    if(Path.GetExtension(file) == ".DS_Store") continue;
                    var filePath = Path.Combine(cloneDirectoryPath, Path.GetFileName(file));
                    if(File.Exists(filePath)) continue;
                        
                    File.Copy(file, filePath);
                }
            }
        }
        
        /// <summary>
        /// Get a list of scans.
        /// </summary>
        /// <returns></returns>
        private List<Scan> GetScans()
        {
            ReInitScans();
            return _scans;
        }
        
        /// <summary>
        /// A method that initializes and checks existing scans.
        /// </summary>
        private void ReInitScans()
        {
            var loadScans = new List<Scan>();

            var folderPath = Path.Combine(Application.persistentDataPath, nameScanDirectory);
            
            if (!Directory.Exists(folderPath))
            {
                Debug.Log("The directory does not exist.");
                return;
            }

            var directorys = Directory.GetDirectories(folderPath);
            
            if (!directorys.Any())
            {
                Debug.Log("The directory is empty.");
                return;
            }
            
            var removeDirectories = new List<string> {"__MACOSX"};
            
            foreach (var directory in directorys)
            {
                var directoryName = Path.GetFileName(directory);
                if(!removeDirectories.Contains(directoryName)) continue;
                try
                {
                    Directory.Delete(Path.Combine(folderPath, directoryName), true);
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
            }

            var datePatt = @"d-M-yyyy | hh:mm tt";

            foreach (var directory in directorys)
            {
                if(!Directory.Exists(directory)) continue;
                var scanId = Path.GetFileNameWithoutExtension(directory);

                var jsonPath = Path.Combine(directory, "scan.json");
                var usdzPath = Path.Combine(directory, "scan.usdz");

                var scan = new Scan {id = scanId, name = scanId, directoryPath = directory};

                var creationTime = Directory.GetCreationTime(directory);
                var creationTimeUTC = creationTime.ToUniversalTime().ToString(datePatt, CultureInfo.InvariantCulture).ToLower();
                scan.creationTime = creationTimeUTC;
                    
                var modifyTime = Directory.GetLastWriteTime(directory);
                var modifyTimeUTC = modifyTime.ToUniversalTime().ToString(datePatt, CultureInfo.InvariantCulture).ToLower();
                scan.modifyTime = modifyTimeUTC;
                
                if (File.Exists(jsonPath))
                {
                    scan.jsonPath = jsonPath;
                    scan.snapshot = File.ReadAllText(jsonPath);
                }
                
                if (File.Exists(usdzPath))
                {
                    scan.usdzPath = usdzPath;
                }
                
                loadScans.Add(scan);
            }
            
            _scans.Clear();
            
            switch (sortType)
            {
                case SortType.None:
                    _scans.AddRange(loadScans);
                    break;
                case SortType.Name:
                    List<Scan> sortedListByName;
                    sortedListByName = loadScans.OrderBy(s=> s.name).ToList();
                    _scans.AddRange(sortedListByName);
                    break;
                case SortType.CreationTime:
                    List<Scan> sortedListByCreationTime;
                    sortedListByCreationTime = loadScans.OrderBy(s=> DateTime.ParseExact(s.creationTime, datePatt, CultureInfo.InvariantCulture)).ToList();
                    _scans.AddRange(sortedListByCreationTime);
                    break;
                case SortType.ModifyTime:
                    List<Scan> sortedListByModifyTime;
                    sortedListByModifyTime = loadScans.OrderBy(s=> DateTime.ParseExact(s.modifyTime, datePatt, CultureInfo.InvariantCulture)).ToList();
                    _scans.AddRange(sortedListByModifyTime);
                    break;
                default:
                    _scans.AddRange(loadScans);
                    break;
            }

            if (reverseList)
            {
                _scans.Reverse();
            }
        }
    }
}

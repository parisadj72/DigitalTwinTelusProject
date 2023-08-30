using System;
using System.Collections;
using System.IO;
using System.Linq;
using SilverTau.RoomPlanUnity;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace SilverTau.Sample
{
    public class ARViewer : MonoBehaviour
    {
        [SerializeField] private RoomPlanUnityKit roomPlanUnityKit;
        [SerializeField] private CapturedRoomSnapshot capturedRoomSnapshot;
        [SerializeField] private PlaceObjectAR placeObjectAR;
        
        [Space(10)]
        [Header("Common")]
        [SerializeField] private string nameScanDirectory = "model-viewer";
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Camera aRCamera;
        [SerializeField] private GameObject envObjects;
        
        [Space(10)]
        [Header("UI")]
        [SerializeField] private MenuUIManager uIManager;
        [SerializeField] private GameObject loader;
        
        [Space(10)]
        [Header("Pop-ups")]
        [SerializeField] private GameObject popupHasScanName;
        [SerializeField] private GameObject popupInputScanName;
        [SerializeField] private TMP_InputField inputFieldScanName;

        public UnityAction<int> onStartNewScan;
        
        private void Start()
        {
        }
        
        /// <summary>
        /// A method that allows you to change the RoomPlanObject prefab when loading an AR.
        /// </summary>
        /// <param name="roomPlanObject">Target prefab RoomPlanObject.</param>
        public void ChangeCapturedRoomObjectPrefab(RoomPlanObject roomPlanObject)
        {
            if(capturedRoomSnapshot == null) return;
            capturedRoomSnapshot.SetCapturedRoomObjectPrefab(roomPlanObject);
        }
        
        /// <summary>
        /// A method that launches AR with a specific parameter.
        /// </summary>
        /// <param name="value">AR stage parameter.</param>
        public void StartNewScan(int value)
        {
            switch (value)
            {
                case 0:
                    RoomPlanUnityKit.CurrentRoomPlanUnityKitSettings.isDefaultScanEnabled = true;
                    break;
                case 1:
                    RoomPlanUnityKit.CurrentRoomPlanUnityKitSettings.isDefaultScanEnabled = false;
                    break;
                default:
                    break;
            }
            
            onStartNewScan?.Invoke(value);
            
            roomPlanUnityKit.UpdateRPUKitSettings();
            OpenAR();
        }
        
        /// <summary>
        /// A method that launches AR.
        /// </summary>
        public void OpenAR()
        {
            Resources.UnloadUnusedAssets();
            
            loader.gameObject.SetActive(true);
            
            if (aRCamera) aRCamera.gameObject.SetActive(true);
            if (envObjects) envObjects.gameObject.SetActive(true);
            if (mainCamera) mainCamera.gameObject.SetActive(false);
            if (placeObjectAR) placeObjectAR.gameObject.SetActive(true);
            
            capturedRoomSnapshot.Dispose();
            roomPlanUnityKit.Initialize();
            
#if UNITY_EDITOR
            if (RoomPlanUnityKit.CurrentRoomPlanUnityKitSettings.editorAR)
            {
                uIManager.OpenMenu("ar-viewer");
                var editorArScan = Resources.Load<TextAsset>("EditorAR/scan");
                capturedRoomSnapshot.EditorRoomSnapshot(editorArScan.text);
                loader.gameObject.SetActive(false);
                return;
            }
#endif

            if (!RoomPlanUnityKit.RPUSupport)
            {
                loader.gameObject.SetActive(false);
                return;
            }
            
            uIManager.OpenMenu("ar-viewer");
            roomPlanUnityKit.StartCaptureSession();
            loader.gameObject.SetActive(false);
        }
        
        /// <summary>
        /// A method that closes AR.
        /// </summary>
        public void CloseAR()
        {
            Resources.UnloadUnusedAssets();
            if(mainCamera) mainCamera.gameObject.SetActive(true);
            if(aRCamera) aRCamera.gameObject.SetActive(false);
            if(envObjects) envObjects.gameObject.SetActive(false);
            if(placeObjectAR) placeObjectAR.gameObject.SetActive(false);
            
            uIManager.OpenMenu("scans");
            roomPlanUnityKit.StopCaptureSession();
            roomPlanUnityKit.Dispose();
            capturedRoomSnapshot.Dispose();
        }

        public void PauseResumeCaptureSessionScanning()
        {
            if (RoomPlanUnityKit.CurrentCaptureStatus == CapturedRoom.CaptureStatus.Paused)
            {
                roomPlanUnityKit.ResumeCaptureSessionScanning();
                return;
            }
            
            roomPlanUnityKit.PauseCaptureSessionScanning();
        }
        
        /// <summary>
        /// A method that controls the pop-up for entering a scan name.
        /// </summary>
        /// <param name="status">Popup status parameter.</param>
        public void CallPopupInputScanName(bool status)
        {
            if(popupInputScanName == null) return;
            popupInputScanName.SetActive(status);
        }
        
        /// <summary>
        /// A method that saves a scan from a pop-up window.
        /// </summary>
        public void SaveExperienceFromPopup()
        {
            if(inputFieldScanName == null) return;
            if(string.IsNullOrEmpty(inputFieldScanName.text)) return;
            SaveExperience(inputFieldScanName.text);
        }
        
        /// <summary>
        /// A method that stores scanned experience, scan.
        /// </summary>
        /// <param name="value">Scan name.</param>
        private void SaveExperience(string value)
        {
            try
            {
                var pathToScans = Path.Combine(Application.persistentDataPath, nameScanDirectory);

                if (!Directory.Exists(pathToScans))
                {
                    Directory.CreateDirectory(pathToScans);
                }
                
                if (Directory.Exists(pathToScans))
                {
                    var dirs = Directory.GetDirectories(pathToScans);

                    if (dirs.Any())
                    {
                        foreach (var dir in dirs)
                        {
                            if(string.IsNullOrEmpty(dir)) continue;
                            var dirName = Path.GetFileName(dir);
                            if (dirName != value) continue;
                            popupHasScanName.SetActive(true);
                            Debug.Log("A scan with this name already exists.");
                            return;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log("An error occurred while saving a scan.");
                Debug.Log(e);
                return;
            }
            
            StartCoroutine(IESaveExperience(value));
        }
        
        /// <summary>
        /// A Coroutine that helps to save a scan correctly.
        /// </summary>
        /// <param name="value">Scan name.</param>
        /// <returns></returns>
        private IEnumerator IESaveExperience(string value)
        {
            loader.gameObject.SetActive(true);
            
            roomPlanUnityKit.SaveRoomPlanExperience(value, nameScanDirectory);
            
            yield return new WaitForSeconds(0.5f);
            
            roomPlanUnityKit.StopCaptureSession();
            yield return new WaitForSeconds(0.2f);
            roomPlanUnityKit.Dispose();
            yield return new WaitForSeconds(0.2f);
            capturedRoomSnapshot.Dispose(() =>
            {
                if(mainCamera) mainCamera.gameObject.SetActive(true);
                if(aRCamera) aRCamera.gameObject.SetActive(false);
                if(envObjects) envObjects.gameObject.SetActive(false);
                if(placeObjectAR) placeObjectAR.gameObject.SetActive(false);

                CallPopupInputScanName(false);
                uIManager.OpenMenu("scans");
                loader.gameObject.SetActive(false);
            });
            
            yield break;
        }
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace SilverTau.RoomPlanUnity
{
    public class RPUARFinder : MonoBehaviour
    {
        [SerializeField] private GameObject finder;
        [SerializeField] private GameObject bodyDefault;
        [SerializeField] private GameObject bodyCustom;
        [SerializeField] private UnityEvent onEnable;
        [SerializeField] private UnityEvent onCompleted;

        private CanvasGroup _finderCanvasGroup;

        private void Awake()
        {
            if (finder == null)
            {
                return;
            }

            finder.SetActive(false);
        }

        private void Start()
        {
        }

        private void OnEnable()
        {
            if (finder == null)
            {
                return;
            }
            
            _finderCanvasGroup = finder.GetComponent<CanvasGroup>();
                
            if (_finderCanvasGroup == null)
            {
                _finderCanvasGroup = finder.AddComponent<CanvasGroup>();
            }

#if UNITY_EDITOR
            if (RoomPlanUnityKit.CurrentRoomPlanUnityKitSettings.editorAR)
            {
                RoomPlanUnityKit.CurrentCaptureStatus = CapturedRoom.CaptureStatus.Scanning;
                CaptureStatus(RoomPlanUnityKit.CurrentCaptureStatus);
                return;
            }
#endif
            
            RoomPlanUnityKit.captureStatus += CaptureStatus;
            CaptureStatus(RoomPlanUnityKit.CurrentCaptureStatus);
        }
        
        private void OnDisable()
        {
            if (finder == null)
            {
                return;
            }
            
            if (bodyDefault) bodyDefault.SetActive(false);
            if (bodyCustom) bodyCustom.SetActive(false);
            
            RoomPlanUnityKit.captureStatus -= CaptureStatus;
        }
        
        private void CaptureStatus(CapturedRoom.CaptureStatus captureStatus)
        {
            switch (captureStatus)
            {
                case CapturedRoom.CaptureStatus.None:
                case CapturedRoom.CaptureStatus.Processing:
                    onEnable?.Invoke();

                    finder.SetActive(!RoomPlanUnityKit.CurrentRoomPlanUnityKitSettings.isDefaultScanEnabled);
                    
                    if (bodyDefault) bodyDefault.SetActive(false);
                    if (bodyCustom) bodyCustom.SetActive(false);
                    
                    break;
                case CapturedRoom.CaptureStatus.Scanning:
                    finder.SetActive(false);

                    if (RoomPlanUnityKit.CurrentRoomPlanUnityKitSettings.isDefaultScanEnabled)
                    {
                        if (bodyCustom) bodyCustom.SetActive(false);
                        if (bodyDefault) bodyDefault.SetActive(true);
                    }
                    else
                    {
                        if (bodyDefault) bodyDefault.SetActive(false);
                        if (bodyCustom) bodyCustom.SetActive(true);
                    }
                        
                    onCompleted?.Invoke();
                    break;
                default:
                    break;
            }
        }
    }
}
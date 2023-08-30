using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Threading;
using AOT;
using UnityEngine;
using UnityEngine.Events;

namespace SilverTau.RoomPlanUnity
{
    public class RoomPlanUnityKit : MonoBehaviour
    {
        #region RPU main

        [DllImport("__Internal")]
        private static extern void initializeRPU();
        
        [DllImport("__Internal")]
        private static extern void disposeRPU();
        
        #endregion

        #region RPU RoomPlan
        
        [DllImport("__Internal")]
        private static extern void startCaptureSession();
        
        [DllImport("__Internal")]
        private static extern void stopCaptureSession();
        
        [DllImport("__Internal")]
        private static extern void pauseCaptureSessionScanning();
        
        [DllImport("__Internal")]
        private static extern void resumeCaptureSessionScanning();
        
        [DllImport("__Internal")]
        private static extern void saveRoomPlanExperience(String directoryName, String scanName);
        
        [DllImport("__Internal")]
        private static extern bool trySaveRoomPlanExperience(String directoryName, String scanName);
        
        [DllImport("__Internal")]
        private static extern void isRPUDefaultScanEnabled(bool value);
        
        [DllImport("__Internal")]
        private static extern void isRPUDefaultMiniatureModelEnabled(bool value);
        
        [DllImport("__Internal")]
        private static extern void isRPUDefaultScanDataTransferEnabled(bool value);

        [DllImport("__Internal")]
        private static extern void isRPUCoachingEnabled(bool value);
        
        [DllImport("__Internal")]
        private static extern void isRPUInstructionEnabled(bool value);

        #endregion

        #region Screenshot

        [DllImport("__Internal")]
        private static extern void screenshotRPU();
        
        [DllImport("__Internal")]
        private static extern void isNativeScreenshot(bool value);
        
        [DllImport("__Internal")]
        private static extern void isNativeScreenshotPreview(bool value);

        [DllImport("__Internal")]
        private static extern string getRPUNativeScreenshotOutputURL();
        
        [DllImport("__Internal")]
        private static extern void screenshotShareRPU();

        #endregion

        #region Video Recorder

        [DllImport("__Internal")]
        private static extern void startScreenRecorderRPU();
        
        [DllImport("__Internal")]
        private static extern void stopScreenRecorderRPU();
        
        [DllImport("__Internal")]
        private static extern void isNativeScreenRecorder(bool value);
        
        [DllImport("__Internal")]
        private static extern void isNativeScreenRecorderPreview(bool value);

        [DllImport("__Internal")]
        private static extern string getRPUNativeScreenRecorderOutputURL();
        
        #endregion

        #region RPU Get

        [DllImport("__Internal")]
        private static extern string getRoomPlanUnityKitVersion();

        [DllImport("__Internal")]
        private static extern bool RoomPlanUnityKitSupport();
        
        [DllImport("__Internal")]
        private static extern string getRPUARCameraMatrixRuntime();
        
        [DllImport("__Internal")]
        private static extern string getRPUARCameraProjectionMatrixRuntime();

        #endregion
        
        #region Delegates

        public delegate void RPUDidStartDelegate();
        [DllImport("__Internal")]
        private static extern void setRPUDidStart(RPUDidStartDelegate callBack);

        public delegate void RPUDidEndDelegate();
        [DllImport("__Internal")]
        private static extern void setRPUDidEnd(RPUDidEndDelegate callBack);
        
        public delegate void RPUCaptureSessionDidStartDelegate();
        [DllImport("__Internal")]
        private static extern void setRPUCaptureSessionDidStart(RPUCaptureSessionDidStartDelegate callBack);
        
        public delegate void RPUCaptureSessionDidEndDelegate();
        [DllImport("__Internal")]
        private static extern void setRPUCaptureSessionDidEnd(RPUCaptureSessionDidEndDelegate callBack);
        
        public delegate void RPUCaptureSessionDidAddDelegate();
        [DllImport("__Internal")]
        private static extern void setRPUCaptureSessionDidAdd(RPUCaptureSessionDidAddDelegate callBack);
        
        public delegate void RPUCaptureSessionDidUpdateDelegate();
        [DllImport("__Internal")]
        private static extern void setRPUCaptureSessionDidUpdate(RPUCaptureSessionDidUpdateDelegate callBack);
        
        public delegate void RPUCaptureSessionDidChangeDelegate();
        [DllImport("__Internal")]
        private static extern void setRPUCaptureSessionDidChange(RPUCaptureSessionDidChangeDelegate callBack);
        
        public delegate void RPUCaptureSessionDidRemoveDelegate();
        [DllImport("__Internal")]
        private static extern void setRPUCaptureSessionDidRemove(RPUCaptureSessionDidRemoveDelegate callBack);
        
        public delegate void RPUCaptureSessionInstructionDelegate(String value);
        [DllImport("__Internal")]
        private static extern void setRPUCaptureSessionInstruction(RPUCaptureSessionInstructionDelegate callBack);
        
        public delegate void RPURoomSnapshotDelegate(String value);
        [DllImport("__Internal")]
        private static extern void setRPURoomSnapshot(RPURoomSnapshotDelegate callBack);
        
        #endregion

        #region UnityActions
        
        /// <summary>
        /// Called when the framework is initialized.
        /// </summary>
        public static UnityAction didStart;
        
        /// <summary>
        /// Called when the framework is disposed of.
        /// </summary>
        public static UnityAction didEnd;
        
        /// <summary>
        /// Called when the scanning session starts.
        /// </summary>
        public static UnityAction captureSessionDidStart;
        
        /// <summary>
        /// Called when the scan session ends.
        /// </summary>
        public static UnityAction captureSessionDidEnd;
        
        public static UnityAction captureSessionDidAdd;
        public static UnityAction captureSessionDidUpdate;
        public static UnityAction captureSessionDidChange;
        public static UnityAction captureSessionDidRemove;
        
        /// <summary>
        /// Provides scan session instructions for the user.
        /// </summary>
        public static UnityAction<CapturedRoom.SessionInstruction> captureSessionInstruction;
        
        /// <summary>
        /// Called when a session submits a snapshot to the Unity Engine.
        /// </summary>
        public static UnityAction<String> roomSnapshot;

        /// <summary>
        /// Called when the session status is changed.
        /// </summary>
        public static UnityAction<CapturedRoom.CaptureStatus> captureStatus;
        #endregion
        
        [SerializeField] private RPUSessionCamera sessionCamera;
        [SerializeField] private RoomPlanUnityKitSettings kitSettings;

        /// <summary>
        /// An option that lets you set or get the current session camera.
        /// </summary>
        public static RPUSessionCamera CurrentSessionCamera  { get; set; }
        
        /// <summary>
        /// An option that allows you to set or get the current settings of the framework.
        /// </summary>
        public static RoomPlanUnityKitSettings CurrentRoomPlanUnityKitSettings { get; set; }

        private static CapturedRoom.CaptureStatus _captureStatus = CapturedRoom.CaptureStatus.None;
        
        /// <summary>
        /// A parameter that informs about the current status of the session.
        /// </summary>
        public static CapturedRoom.CaptureStatus CurrentCaptureStatus
        {
            get => _captureStatus;
            set => OnChangedCaptureStatus(value);
        }
        
        public static bool debug = false;
        
        [Space(10)]
        [Header("Initialization")]
        [Tooltip("If the parameter is enabled, initialization will occur only when you initialize the framework manually.")]
        [SerializeField] private bool delayInitialization = false;
        
        [Space(10)]
        [Header("Match Frame Rate")]
        [Tooltip("This helps to optimize the performance of the application and the framework, as well as to set the desired frame rate.")]
        [SerializeField] private bool matchFrameRate = true;
        [SerializeField] private bool useWaitForNextFrame = false;
        [SerializeField] private int targetFrameRate = 60;

        /// <summary>
        /// Value that sets the target frame rate in real time. If you use useWaitForNextFrame!
        /// </summary>
        public int ApplicationTargetFrameRate
        {
            get => targetFrameRate;
            set => SetTargetFrameRate(value);
        }
        
        private float currentFrameTime;
        
        private static float[] aRCameraMatrix;

        /// <summary>
        /// Values to check for RoomPlan support.
        /// </summary>
        public static bool RPUSupport => CheckRPUSupport();
        
        private static bool CheckRPUSupport()
        {
#if !UNITY_EDITOR && PLATFORM_IOS
            var result = RoomPlanUnityKitSupport();
            if (debug) Debug.Log($"RoomPlan Unity Kit Support = {result}.");
            return result;
#elif !UNITY_EDITOR && PLATFORM_ANDROID //FOR THE TEST APPLICATION ONLY
            if (debug) Debug.Log("RoomPlan Unity Kit Support = PLATFORM ANDROID (FOR THE TEST APPLICATION ONLY).");
            return true;
#else
            if (debug) Debug.Log("RoomPlan Unity Kit = Unity Editor.");
            return false;
#endif
        }

        /// <summary>
        /// Values to get Screen Recorder Output URL.
        /// </summary>
        public static string RPUNativeScreenRecorderOutputURL => CheckRPUNativeScreenRecorderOutputURL();
        
        private static string CheckRPUNativeScreenRecorderOutputURL()
        {
#if !UNITY_EDITOR && PLATFORM_IOS
            var result = getRPUNativeScreenRecorderOutputURL();
            if (debug) Debug.Log($"RoomPlan Unity Kit Native Screen Recorder Output URL = {result}.");
            return result;
#else
            if (debug) Debug.Log("RoomPlan Unity Kit = OTHER PLATFORM.");
            return string.Empty;
#endif
        }

        /// <summary>
        /// An option that allows you to set or get the current status of the session, whether it is active.
        /// </summary>
        public static bool RPUActive { get; private set; } = false;
        
        /// <summary>
        /// An option that allows you to set or get the current scanning status of the session, whether it is active.
        /// </summary>
        public static bool RPUCaptureSessionActive { get; private set; } = false;
        
        private void Awake()
        {
            if (matchFrameRate)
            {
                QualitySettings.vSyncCount = 0;
                
                if (useWaitForNextFrame)
                {
                    Application.targetFrameRate = (int)targetFrameRate;
                    currentFrameTime = Time.realtimeSinceStartup;
                    StartCoroutine(WaitForNextFrame());
                }
                else
                {
                    Application.targetFrameRate = (int)targetFrameRate;
                }
            }
            
            CurrentRoomPlanUnityKitSettings = kitSettings;
            CurrentSessionCamera = sessionCamera;
        }
        
        private void Start()
        {
            if(delayInitialization) return;
            Initialize();
        }
        
        /// <summary>
        /// A method that sets the target frame rate.
        /// </summary>
        /// <param name="value">Frame rate value.</param>
        private void SetTargetFrameRate(float value)
        {
            QualitySettings.vSyncCount = 0;
            
            if (useWaitForNextFrame)
            {
                targetFrameRate = (int)value;
            }
            
            Application.targetFrameRate = (int)value;
        }

        private IEnumerator WaitForNextFrame()
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();
                currentFrameTime += 1.0f / targetFrameRate;
                var t = Time.realtimeSinceStartup;
                var sleepTime = currentFrameTime - t - 0.01f;
                if (sleepTime > 0)
                    Thread.Sleep((int)(sleepTime * 1000));
                while (t < currentFrameTime)
                    t = Time.realtimeSinceStartup;
            }
        }

        /// <summary>
        /// A method that initializes RoomPlan for Unity.
        /// </summary>
        public void Initialize()
        {
            if (RPUActive) return;
#if UNITY_EDITOR
            
            CurrentCaptureStatus = CapturedRoom.CaptureStatus.None;
            RPUActive = true;
            
#elif !UNITY_EDITOR && PLATFORM_IOS
            isRPUDefaultScanEnabled(CurrentRoomPlanUnityKitSettings.isDefaultScanEnabled);
            isRPUDefaultMiniatureModelEnabled(CurrentRoomPlanUnityKitSettings.isDefaultMiniatureModelEnabled);
            isRPUDefaultScanDataTransferEnabled(CurrentRoomPlanUnityKitSettings.isDefaultScanDataTransferEnabled);
            isNativeScreenshot(CurrentRoomPlanUnityKitSettings.isNativeScreenshot);
            isNativeScreenshotPreview(CurrentRoomPlanUnityKitSettings.isNativeScreenshotPreview);
            isNativeScreenRecorder(CurrentRoomPlanUnityKitSettings.isNativeScreenRecorder);
            isNativeScreenRecorderPreview(CurrentRoomPlanUnityKitSettings.isNativeScreenRecorderPreview);

            initializeRPU();

            if (!RPUSupport)
            {
                if (debug)
                {
                    Debug.Log("The camera is disabled because RoomPlan Unity Kit does not support your current platform.");
                }
                
                CurrentCaptureStatus = CapturedRoom.CaptureStatus.None;
                RPUActive = false;
                return;
            }
            
            setRPUDidStart(RPUDidStart);
            setRPUDidEnd(RPUDidEnd);

            setRPUCaptureSessionDidStart(RPUCaptureSessionDidStart);
            setRPUCaptureSessionDidEnd(RPUCaptureSessionDidEnd);
            setRPUCaptureSessionDidAdd(RPUCaptureSessionDidAdd);
            setRPUCaptureSessionDidUpdate(RPUCaptureSessionDidUpdate);
            setRPUCaptureSessionDidChange(RPUCaptureSessionDidChange);
            setRPUCaptureSessionDidRemove(RPUCaptureSessionDidRemove);
            setRPUCaptureSessionInstruction(RPUCaptureSessionInstruction);
            setRPURoomSnapshot(RPURoomSnapshot);
            
            CurrentCaptureStatus = CapturedRoom.CaptureStatus.None;
            RPUActive = true;
#else
            if (debug) Debug.Log("RoomPlan Unity Kit = OTHER PLATFORM. Not supported!");
#endif
        }
        
        /// <summary>
        /// A method that updates the main RoomPlan settings in real time.
        /// </summary>
        public void UpdateRPUKitSettings()
        {
#if !UNITY_EDITOR
            isRPUDefaultScanEnabled(CurrentRoomPlanUnityKitSettings.isDefaultScanEnabled);
            isRPUDefaultMiniatureModelEnabled(CurrentRoomPlanUnityKitSettings.isDefaultMiniatureModelEnabled);
            isRPUDefaultScanDataTransferEnabled(CurrentRoomPlanUnityKitSettings.isDefaultScanDataTransferEnabled);
            isNativeScreenshot(CurrentRoomPlanUnityKitSettings.isNativeScreenshot);
            isNativeScreenshotPreview(CurrentRoomPlanUnityKitSettings.isNativeScreenshotPreview);
            isNativeScreenRecorder(CurrentRoomPlanUnityKitSettings.isNativeScreenRecorder);
            isNativeScreenRecorderPreview(CurrentRoomPlanUnityKitSettings.isNativeScreenRecorderPreview);
#endif
        }

        /// <summary>
        /// A method that starts a RoomPlan scanning session.
        /// </summary>
        public void StartCaptureSession()
        {
            if (!RPUActive)
            {
                return;
            }
            
            if (!RPUSupport)
            {
                return;
            }
            
#if !UNITY_EDITOR
            isRPUCoachingEnabled(CurrentRoomPlanUnityKitSettings.isCoachingEnabled);
            isRPUInstructionEnabled(CurrentRoomPlanUnityKitSettings.isInstructionEnabled);

            startCaptureSession();
#endif
            
            CurrentCaptureStatus = CapturedRoom.CaptureStatus.Processing;
            RPUCaptureSessionActive = true;
        }
        
        /// <summary>
        /// A method that stops a RoomPlan scanning session.
        /// </summary>
        public void StopCaptureSession()
        {
            if (!RPUActive)
            {
                return;
            }
            
#if !UNITY_EDITOR
            stopCaptureSession();
#endif
            CurrentCaptureStatus = CapturedRoom.CaptureStatus.None;
            RPUCaptureSessionActive = false;
        }
        
        /// <summary>
        /// A method that paused a RoomPlan scanning session.
        /// </summary>
        public void PauseCaptureSessionScanning()
        {
            if (!RPUActive)
            {
                return;
            }
            
            if (!RPUCaptureSessionActive)
            {
                return;
            }
            
#if !UNITY_EDITOR
            pauseCaptureSessionScanning();
#endif
            CurrentCaptureStatus = CapturedRoom.CaptureStatus.Paused;
        }
        
        /// <summary>
        /// A method that resumed a RoomPlan scanning session.
        /// </summary>
        public void ResumeCaptureSessionScanning()
        {
            if (!RPUActive)
            {
                return;
            }
            
#if !UNITY_EDITOR
            resumeCaptureSessionScanning();
#endif
            CurrentCaptureStatus = CapturedRoom.CaptureStatus.Scanning;
        }
        
        /// <summary>
        /// A method that disposes of a RoomPlan.
        /// </summary>
        public void Dispose()
        {
            if (!RPUActive)
            {
                return;
            }
            
#if !UNITY_EDITOR
            disposeRPU();
#endif
            
            CurrentCaptureStatus = CapturedRoom.CaptureStatus.None;
            RPUActive = false;
        }
        
        /// <summary>
        /// A method that allows you to take a screenshot of the screen.
        /// </summary>
        public static void Screenshot()
        {
            if (!RPUActive)
            {
                return;
            }
            
#if !UNITY_EDITOR
            screenshotRPU();
#endif
        }
        
        /// <summary>
        /// A method that allows you to share a screenshot.
        /// </summary>
        public static void ScreenshotShare()
        {
            if (!RPUActive)
            {
                return;
            }
            
#if !UNITY_EDITOR
            screenshotShareRPU();
#endif
        }
        
        /// <summary>
        /// A method that allows you to start recording a video screen.
        /// </summary>
        public static void StartScreenRecorder()
        {
            if (!RPUActive)
            {
                return;
            }
            
#if !UNITY_EDITOR
            startScreenRecorderRPU();
#endif
        }
        
        /// <summary>
        /// A method that allows you to stop recording a video screen.
        /// </summary>
        public static void StopScreenRecorder()
        {
            if (!RPUActive)
            {
                return;
            }
            
#if !UNITY_EDITOR
            stopScreenRecorderRPU();
#endif
        }
        
        /// <summary>
        /// A method that allows you to preserve the scanning experience.
        /// </summary>
        /// <param name="scanName">Scan name.</param>
        /// <param name="directoryName">The name of the catalog. The main directory for saving files.</param>
        public void SaveRoomPlanExperience(string scanName = "My new scan", string directoryName = "model-viewer")
        {
            if (string.IsNullOrEmpty(directoryName))
            {
                directoryName = "model-viewer";
            }
            
            if (string.IsNullOrEmpty(scanName))
            {
                scanName = "My new scan";
            }
            
#if !UNITY_EDITOR
            saveRoomPlanExperience(directoryName, scanName);
#endif
        }
        
        /// <summary>
        /// A method that allows you to preserve the scanning experience.
        /// </summary>
        /// <param name="scanName">Scan name.</param>
        /// <param name="directoryName">Directory name.</param>
        public bool TrySaveRoomPlanExperience(string scanName = "My new scan", string directoryName = "model-viewer")
        {
            if (string.IsNullOrEmpty(directoryName))
            {
                directoryName = "model-viewer";
            }
            
            if (string.IsNullOrEmpty(scanName))
            {
                scanName = "My new scan";
            }
            
#if !UNITY_EDITOR
            return trySaveRoomPlanExperience(directoryName, scanName);
#endif
            return false;
        }
        
        /// <summary>
        /// A method that processes the CVMatrix from the camera.
        /// </summary>
        /// <returns></returns>
        public static string GetCameraTransformDataRuntime()
        {
#if !UNITY_EDITOR
            return getRPUARCameraMatrixRuntime();
#endif
            return null;
        }
        
        /// <summary>
        /// A method that processes the CVProjection from the camera.
        /// </summary>
        /// <returns></returns>
        public static string GetCameraProjectionDataRuntime()
        {
#if !UNITY_EDITOR
            return getRPUARCameraProjectionMatrixRuntime();
#endif
            return null;
        }

        private static void OnChangedCaptureStatus(CapturedRoom.CaptureStatus cs)
        {
            _captureStatus = cs;
            captureStatus?.Invoke(cs);
        }

        #region Handle Delegates

        [MonoPInvokeCallback(typeof(RPUDidStartDelegate))]
        public static void RPUDidStart()
        {
            if (debug) Debug.Log("RoomPlan Unity Kit - Did start");
            didStart?.Invoke();
        }

        [MonoPInvokeCallback(typeof(RPUDidEndDelegate))]
        public static void RPUDidEnd()
        {
            if (debug) Debug.Log("RoomPlan Unity Kit - Did end");
            didEnd?.Invoke();
        }
        
        [MonoPInvokeCallback(typeof(RPUCaptureSessionDidStartDelegate))]
        public static void RPUCaptureSessionDidStart()
        {
            if (debug) Debug.Log("RoomPlan Unity Kit - Did Capture Session Start");
            captureSessionDidStart?.Invoke();
        }
        
        [MonoPInvokeCallback(typeof(RPUCaptureSessionDidEndDelegate))]
        public static void RPUCaptureSessionDidEnd()
        {
            if (debug) Debug.Log("RoomPlan Unity Kit - Did Capture Session End");
            captureSessionDidEnd?.Invoke();
        }

        [MonoPInvokeCallback(typeof(RPUCaptureSessionDidAddDelegate))]
        public static void RPUCaptureSessionDidAdd()
        {
            if (debug) Debug.Log("RoomPlan Unity Kit - Did Capture Session Add");
            captureSessionDidAdd?.Invoke();
        }

        [MonoPInvokeCallback(typeof(RPUCaptureSessionDidUpdateDelegate))]
        public static void RPUCaptureSessionDidUpdate()
        {
            if (debug) Debug.Log("RoomPlan Unity Kit - Did Capture Session Update");
            captureSessionDidUpdate?.Invoke();
        }

        [MonoPInvokeCallback(typeof(RPUCaptureSessionDidChangeDelegate))]
        public static void RPUCaptureSessionDidChange()
        {
            if (debug) Debug.Log("RoomPlan Unity Kit - Did Capture Session Change");
            captureSessionDidChange?.Invoke();
        }

        [MonoPInvokeCallback(typeof(RPUCaptureSessionDidRemoveDelegate))]
        public static void RPUCaptureSessionDidRemove()
        {
            if (debug) Debug.Log("RoomPlan Unity Kit - Did Capture Session Remove");
            captureSessionDidRemove?.Invoke();
        }
        
        [MonoPInvokeCallback(typeof(RPUCaptureSessionInstructionDelegate))]
        public static void RPUCaptureSessionInstruction(String value)
        {
            if (debug) Debug.Log("RoomPlan Unity Kit - Capture Session Instruction: " + value);

            if (Enum.TryParse(value, true, out CapturedRoom.SessionInstruction instruction))
            {
                captureSessionInstruction?.Invoke(instruction);
            }
        }
        
        [MonoPInvokeCallback(typeof(RPURoomSnapshotDelegate))]
        public static void RPURoomSnapshot(String value)
        {
            if (debug) Debug.Log("RoomPlan Unity Kit - Room Snapshot");

            if (CurrentCaptureStatus != CapturedRoom.CaptureStatus.Scanning)
            {
                CurrentCaptureStatus = CapturedRoom.CaptureStatus.Scanning;
            }

            if (CurrentRoomPlanUnityKitSettings.isDefaultScanEnabled)
            {
                if(!CurrentRoomPlanUnityKitSettings.isDefaultScanDataTransferEnabled) return;
            }
            
            roomSnapshot?.Invoke(value);
        }
        
        #endregion
    }
}
using SilverTau.RoomPlanUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScanner : MonoBehaviour
{

    #region Fields

    [Header("Room Plan Kit")]
    [SerializeField] private RoomPlanUnityKit _roomPlanUnityKit;
    [SerializeField] private CapturedRoomSnapshot _capturedRoomSnapshot;
    [SerializeField] private Camera _arCamera;

    [Header("UI")]
    [SerializeField] private GameObject _canvas;

    #endregion

    #region Initialization

    /// <summary>
    /// Initializes and starts the AR scanning.
    /// </summary>
    private void Start()
    {
        // Set to default style
        RoomPlanUnityKit.CurrentRoomPlanUnityKitSettings.isDefaultScanEnabled = true;

        // Apply the settings
        _roomPlanUnityKit.UpdateRPUKitSettings();

        // Clear resources to avoid memory leaks
        Resources.UnloadUnusedAssets();
        _capturedRoomSnapshot.Dispose();

        // Initialize the room plan kit
        _roomPlanUnityKit.Initialize();

        // Enable UI
        _canvas.SetActive(true);

        // Start AR capturing
#if UNITY_EDITOR
        if (RoomPlanUnityKit.CurrentRoomPlanUnityKitSettings.editorAR)
        {
            var editorArScan = Resources.Load<TextAsset>("EditorAR/scan");
            _capturedRoomSnapshot.EditorRoomSnapshot(editorArScan.text);
            return;
        }
#endif

        if (!RoomPlanUnityKit.RPUSupport)
            return;

        _roomPlanUnityKit.StartCaptureSession();
    }

    #endregion

}

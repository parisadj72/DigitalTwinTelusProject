using UnityEngine;

namespace SilverTau.RoomPlanUnity
{
    public class RPUSessionCamera : SessionCamera
    {
        [Tooltip("The current session camera.")]
        [SerializeField] private Camera ARCamera;
        
        /// <summary>
        /// Customize the current session camera.
        /// </summary>
        public Camera CurrentARCamera
        {
            get { return ARCamera; }
            set { SetNewCamera(value); }
        }

        private void OnEnable()
        {
#if UNITY_EDITOR
            if (RoomPlanUnityKit.CurrentRoomPlanUnityKitSettings.editorAR)
            {
                var editorArCamera = CurrentARCamera.gameObject.AddComponent<EditorARCamera>();
                editorArCamera.cameraAR = CurrentARCamera;
                editorArCamera.canMove = true;
                editorArCamera.canRotate = true;
            }
#endif
            UpdateProjection(CurrentARCamera);
        }

        /// <summary>
        /// An additional method of installing a new camera.
        /// </summary>
        /// <param name="newCamera">Target camera.</param>
        public void SetNewCamera(Camera newCamera)
        {
            if(ARCamera == newCamera) return;
            ARCamera = newCamera;
            UpdateProjection(newCamera);
        }
        
        private void Update()
        {
            if (CurrentARCamera == null) return;
            
            if(!RoomPlanUnityKit.RPUCaptureSessionActive) return;
            UpdateCameraTRS(CurrentARCamera, RoomPlanUnityKit.GetCameraTransformDataRuntime(), RoomPlanUnityKit.GetCameraProjectionDataRuntime());
        }
    }
}

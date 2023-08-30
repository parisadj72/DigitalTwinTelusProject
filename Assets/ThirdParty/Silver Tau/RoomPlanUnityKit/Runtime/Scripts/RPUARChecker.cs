using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace SilverTau.RoomPlanUnity
{
    public class RPUARChecker : MonoBehaviour
    {
        [SerializeField] private float delayTime = 15.0f;
        [Space]
        [SerializeField] private UnityEvent onDidNotScanning;
        [SerializeField] private UnityEvent onDidLowLight;

        private bool _isCaptureSessionDidUpdate = false;
        
        private void Awake()
        {
        }

        private void Start()
        {
        }

        private void OnEnable()
        {
#if UNITY_EDITOR
            if (RoomPlanUnityKit.CurrentRoomPlanUnityKitSettings.editorAR)
            {
                return;
            }
#endif
            
            RoomPlanUnityKit.captureSessionDidUpdate += CaptureSessionDidUpdate;
            StartCoroutine(CheckLIDAR());
        }

        private void OnDisable()
        {
            RoomPlanUnityKit.captureSessionDidUpdate -= CaptureSessionDidUpdate;
            _isCaptureSessionDidUpdate = false;
        }
        
        private void CaptureSessionDidUpdate()
        {
            if (_isCaptureSessionDidUpdate) return;
            StopAllCoroutines();
            _isCaptureSessionDidUpdate = true;
        }
        
        private void CaptureSessionInstruction(CapturedRoom.SessionInstruction sessionInstruction)
        {
            if(!_isCaptureSessionDidUpdate) return;
            
            switch (sessionInstruction)
            {
                case CapturedRoom.SessionInstruction.None:
                    break;
                case CapturedRoom.SessionInstruction.MoveCloseToWall:
                    break;
                case CapturedRoom.SessionInstruction.MoveAwayFromWall:
                    break;
                case CapturedRoom.SessionInstruction.SlowDown:
                    break;
                case CapturedRoom.SessionInstruction.TurnOnLight:
                    onDidLowLight?.Invoke();
                    break;
                case CapturedRoom.SessionInstruction.Normal:
                    break;
                case CapturedRoom.SessionInstruction.LowTexture:
                    break;
                default:
                    break;
            }
        }
        
        private IEnumerator CheckLIDAR()
        {
            yield return new WaitForSeconds(delayTime);
            
            if(_isCaptureSessionDidUpdate) yield break;
            
            Debug.Log("It seems some issues with scanning.\nPlease, check your LiDAR sensor or the lighting in the room.");
            onDidNotScanning?.Invoke();
            yield break;
        }
    }
}
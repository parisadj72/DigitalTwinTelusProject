using UnityEngine;
using UnityEngine.Events;

namespace SilverTau.RoomPlanUnity
{
    public class RPUDelegateEvents : MonoBehaviour
    {
        public UnityEvent didStart;
        public UnityEvent didEnd;
        public UnityEvent captureSessionDidStart;
        public UnityEvent captureSessionDidEnd;
        
        [Header("AR Runtime")]
        [Tooltip("")]
        [Space(10)]
        public UnityEvent captureSessionDidAdd;
        public UnityEvent captureSessionDidUpdate;
        public UnityEvent captureSessionDidChange;
        public UnityEvent captureSessionDidRemove;
        
        private void OnEnable()
        {
            RoomPlanUnityKit.didStart += DidStart;
            RoomPlanUnityKit.didEnd += DidEnd;
            RoomPlanUnityKit.captureSessionDidStart += CaptureSessionDidStart;
            RoomPlanUnityKit.captureSessionDidEnd += CaptureSessionDidEnd;
            RoomPlanUnityKit.captureSessionDidAdd += CaptureSessionDidAdd;
            RoomPlanUnityKit.captureSessionDidUpdate += CaptureSessionDidUpdate;
            RoomPlanUnityKit.captureSessionDidChange += CaptureSessionDidChange;
            RoomPlanUnityKit.captureSessionDidRemove += CaptureSessionDidRemove;
        }
        
        private void DidStart()
        {
            didStart?.Invoke();
        }
        
        private void DidEnd()
        {
            didEnd?.Invoke();
        }
        
        private void CaptureSessionDidStart()
        {
            captureSessionDidStart?.Invoke();
        }
        
        private void CaptureSessionDidEnd()
        {
            captureSessionDidEnd?.Invoke();
        }
        
        private void CaptureSessionDidAdd()
        {
            captureSessionDidAdd?.Invoke();
        }
        
        private void CaptureSessionDidUpdate()
        {
            captureSessionDidUpdate?.Invoke();
        }
        
        private void CaptureSessionDidChange()
        {
            captureSessionDidChange?.Invoke();
        }
        
        private void CaptureSessionDidRemove()
        {
            captureSessionDidRemove?.Invoke();
        }
    }
}
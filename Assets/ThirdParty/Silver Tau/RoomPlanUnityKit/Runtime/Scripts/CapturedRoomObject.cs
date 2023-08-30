using UnityEngine;
using UnityEngine.Events;

namespace SilverTau.RoomPlanUnity
{
    public class CapturedRoomObject : RoomPlanObject
    {
        [Space(10)]
        [Header("Captured Room")]
        public bool autoSetTRS = true;

        public UnityAction initObject;
        public UnityAction updateObject;
        public UnityAction removeObject;

        public Vector3 CurrentPosition => _currentPosition;
        public Quaternion CurrentRotation => _currentRotation;
        public Vector3 CurrentScale => _currentScale;

        private Vector3 _currentPosition;
        private Quaternion _currentRotation;
        private Vector3 _currentScale;
        
        private void Start()
        {
        }
        
        public override void InitObject(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            _currentPosition = position;
            _currentRotation = rotation;
            _currentScale = scale;

            if (autoSetTRS)
            {
                transform.localPosition = _currentPosition;
                transform.localRotation = _currentRotation;
                transform.localScale = _currentScale;
            }
            
            gameObject.name = category.ToString();
            
            initObject?.Invoke();
        }
        
        public override void UpdateObject(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            _currentPosition = position;
            _currentRotation = rotation;
            _currentScale = scale;
            
            if (autoSetTRS)
            {
                transform.localPosition = _currentPosition;
                transform.localRotation = _currentRotation;
                transform.localScale = _currentScale;
            }
            
            updateObject?.Invoke();
        }
        
        public override void RemoveObject()
        {
            removeObject?.Invoke();
            Destroy(gameObject);
        }
    }
}
using SilverTau.RoomPlanUnity;
using UnityEngine;

namespace SilverTau.Sample
{
    [RequireComponent(typeof(CapturedRoomObject))]
    public abstract class CROVisualizer : MonoBehaviour
    {
        /// <summary>
        /// A parameter that returns the main component of the CapturedRoomObject.
        /// </summary>
        public CapturedRoomObject CurrentCapturedRoomObject => _capturedRoomObject;
        private CapturedRoomObject _capturedRoomObject;

        public virtual void Awake()
        {
            _capturedRoomObject = GetComponent<CapturedRoomObject>();
        }

        public virtual void Start()
        {
        }

        public virtual void OnEnable()
        {
            if(_capturedRoomObject == null) return;
            _capturedRoomObject.initObject += OnInit;
            _capturedRoomObject.updateObject += OnUpdate;
        }

        public virtual void OnDisable()
        {
            if(_capturedRoomObject == null) return;
            _capturedRoomObject.initObject -= OnInit;
            _capturedRoomObject.updateObject -= OnUpdate;
        }

        /// <summary>
        /// An abstract method that signals the creation of an object.
        /// </summary>
        public abstract void OnInit();
        
        /// <summary>
        /// An abstract method that signals updates to the object's characteristics.
        /// </summary>
        public abstract void OnUpdate();
    }
}
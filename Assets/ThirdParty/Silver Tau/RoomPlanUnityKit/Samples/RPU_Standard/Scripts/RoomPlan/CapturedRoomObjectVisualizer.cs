using SilverTau.RoomPlanUnity;
using UnityEngine;

namespace SilverTau.Sample
{
    [RequireComponent(typeof(CapturedRoomObject))]
    public class CapturedRoomObjectVisualizer : CROVisualizer
    {
        [SerializeField] private bool autoSetTRS = false;
        [SerializeField] private float slerpSmoothValue = 1.0f;
        [SerializeField] private bool animatedPosition = false;
        [SerializeField] private bool animatedRotation = false;
        [SerializeField] private bool animatedScale = true;
        [SerializeField] private bool changeCategoryColor = true;
        [SerializeField] private MeshRenderer meshRenderer;
        
        private Vector3 _currentPosition;
        private Quaternion _currentRotation;
        private Vector3 _currentScale;
        private bool _isUpdateTransform;

        public override void Awake()
        {
            base.Awake();
        }

        public override void Start()
        {
            base.Start();
            CurrentCapturedRoomObject.autoSetTRS = autoSetTRS;
        }

        private void Update()
        {
            if(!_isUpdateTransform) return;

            if (animatedPosition)
            {
                if (transform.localPosition != _currentPosition)
                {
                    transform.localPosition = Vector3.Slerp(transform.localPosition, _currentPosition, Time.smoothDeltaTime * slerpSmoothValue * 2);
                }
            }

            if (animatedRotation)
            {
                if (transform.localRotation != _currentRotation)
                {
                    transform.localRotation = Quaternion.Slerp(transform.localRotation, _currentRotation, Time.smoothDeltaTime * slerpSmoothValue * 2);
                }
            }

            if (animatedScale)
            {
                if (transform.localScale != _currentScale)
                {
                    transform.localScale = Vector3.Slerp(transform.localScale, _currentScale, Time.smoothDeltaTime * slerpSmoothValue * 2);
                }
            }

            if (transform.localScale == _currentScale &&
                transform.localRotation == _currentRotation &&
                transform.localPosition == _currentPosition)
            {
                _isUpdateTransform = false;
            }
        }
        
        private void UpdateTransform()
        {
            _currentPosition = CurrentCapturedRoomObject.CurrentPosition;
            if (!animatedPosition) transform.localPosition = _currentPosition;
            
            _currentRotation = CurrentCapturedRoomObject.CurrentRotation;
            if (!animatedRotation) transform.localRotation = _currentRotation;
            
            _currentScale = CurrentCapturedRoomObject.CurrentScale;
            if (!animatedScale) transform.localScale = _currentScale;
            
            _isUpdateTransform = true;
        }

        public override void OnInit()
        {
            UpdateTransform();
            
            if(CurrentCapturedRoomObject == null) return;
            if(meshRenderer == null) return;
            
            if (changeCategoryColor) ChangeCategoryColor();
        }

        public override void OnUpdate()
        {
            UpdateTransform();
            if (changeCategoryColor) ChangeCategoryColor();
        }
        
        private void ChangeCategoryColor()
        {
            var materal = meshRenderer.material;
            
            switch (CurrentCapturedRoomObject.category)
            {
                case CapturedRoom.Category.unknown:
                    materal.color = Color.black;
                    break;
                case CapturedRoom.Category.bathtub:
                    materal.color = Color.blue;
                    break;
                case CapturedRoom.Category.bed:
                    materal.color = Color.cyan;
                    break;
                case CapturedRoom.Category.chair:
                    materal.color = new Color(0.267f, 0.339f, 0.522f);
                    break;
                case CapturedRoom.Category.dishwasher:
                    materal.color = Color.green;
                    break;
                case CapturedRoom.Category.fireplace:
                    materal.color = Color.gray;
                    break;
                case CapturedRoom.Category.oven:
                    materal.color = Color.red;
                    break;
                case CapturedRoom.Category.refrigerator:
                    materal.color = Color.yellow;
                    break;
                case CapturedRoom.Category.sink:
                    materal.color = new Color(0.722f, 0.078f, 0.078f);
                    break;
                case CapturedRoom.Category.sofa:
                    materal.color = new Color(0.878f, 0.482f, 0.094f);
                    break;
                case CapturedRoom.Category.stairs:
                    materal.color = new Color(0.686f, 0.831f, 0.106f);
                    break;
                case CapturedRoom.Category.storage:
                    materal.color = new Color(0.098f, 0.541f, 0.067f);
                    break;
                case CapturedRoom.Category.stove:
                    materal.color = new Color(0.118f, 0.749f, 0.475f);
                    break;
                case CapturedRoom.Category.table:
                    materal.color = new Color(0.059f, 0.6f, 0.518f);
                    break;
                case CapturedRoom.Category.television:
                    materal.color = new Color(0.106f, 0.525f, 0.761f);
                    break;
                case CapturedRoom.Category.toilet:
                    materal.color = new Color(0.314f, 0.106f, 0.761f);
                    break;
                case CapturedRoom.Category.washerDryer:
                    materal.color = new Color(0.667f, 0.439f, 0.922f);
                    break;
                case CapturedRoom.Category.window:
                    materal.color = new Color(0.749f, 0.522f, 0.655f);
                    break;
                case CapturedRoom.Category.wall:
                    materal.color = Color.white;
                    break;
                case CapturedRoom.Category.opening:
                    materal.color = new Color(0.561f, 0.678f, 0.831f);
                    break;
                case CapturedRoom.Category.door:
                    materal.color = new Color(0.655f, 0.851f, 0.659f);
                    break;
                case CapturedRoom.Category.doorOpen:
                    materal.color = new Color(0.655f, 0.851f, 0.659f);
                    break;
                case CapturedRoom.Category.doorClose:
                    materal.color = new Color(0.655f, 0.851f, 0.659f);
                    break;
                case CapturedRoom.Category.floor:
                    materal.color = Color.white;
                    break;
                default:
                    materal.color = Color.black;
                    break;
            }
        }
    }
}
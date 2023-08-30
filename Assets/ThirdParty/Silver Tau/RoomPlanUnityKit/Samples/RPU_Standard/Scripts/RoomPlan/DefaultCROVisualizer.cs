using SilverTau.RoomPlanUnity;
using UnityEngine;

namespace SilverTau.Sample
{
    [RequireComponent(typeof(CapturedRoomObject))]
    public class DefaultCROVisualizer : CROVisualizer
    {
        [SerializeField] private bool showCategoryText = true;
        [SerializeField] private bool showObjectRealSize = true;
        [SerializeField] private TextMesh textMesh;
        [SerializeField] private float textMeshScale = 0.5f;
        
        private Vector3 _currentPosition;
        private Quaternion _currentRotation;
        private Vector3 _currentScale;

        private string _resultText;
        
        public override void Awake()
        {
            base.Awake();
        }

        public override void Start()
        {
            base.Start();
        }

        private void UpdateTransform()
        {
            _currentPosition = CurrentCapturedRoomObject.CurrentPosition;
            transform.localPosition = _currentPosition;
            
            _currentRotation = CurrentCapturedRoomObject.CurrentRotation;
            transform.localRotation = _currentRotation;
            
            _currentScale = CurrentCapturedRoomObject.CurrentScale;
            transform.localScale = _currentScale;

            if (textMesh == null) return;
            textMesh.transform.localScale = new Vector3(textMeshScale / _currentScale.x, textMeshScale / _currentScale.y, textMeshScale / _currentScale.z);
        }

        public override void OnInit()
        {
            UpdateTransform();
            
            if(CurrentCapturedRoomObject == null) return;
            
            if (showCategoryText) ChangeCategoryText();
        }

        public override void OnUpdate()
        {
            UpdateTransform();
            if (showCategoryText) ChangeCategoryText();
        }
        
        private void ChangeCategoryText()
        {
            if(textMesh == null) return;
            
            _resultText = string.Empty;
            
            //This is done to remove the name of the floor component, it's more convenient for us.
            //Because it is generated using Triangulation and its perpendicular object is always shown in the middle of the scanned plane.
            //You can remove this exception.
            if (CurrentCapturedRoomObject.category == CapturedRoom.Category.floor)
            {
                textMesh.text = _resultText;
                return;
            }

            _resultText = CurrentCapturedRoomObject.category.ToString();
            
            if (showObjectRealSize)
            {
                _resultText += "\n"
                               + CurrentCapturedRoomObject.CurrentScale.x.ToString("F") + "x"
                               + CurrentCapturedRoomObject.CurrentScale.y.ToString("F") + "x";
                
                _resultText += CurrentCapturedRoomObject.CurrentScale.z <= 0.01f ? "0.20" : CurrentCapturedRoomObject.CurrentScale.z.ToString("F");
            }
            
            textMesh.text = _resultText;
        }
    }
}
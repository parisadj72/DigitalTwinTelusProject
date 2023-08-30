using UnityEngine;

namespace SilverTau.RoomPlanUnity
{
    public class EditorARCamera : MonoBehaviour
    {
#if UNITY_EDITOR
        public Camera cameraAR;
    
        [Space(10)]
        public bool canRotate;
        [SerializeField] [Range(-2.0f, 2.0f)] private float xRotationSpeed = 0.8f;
        [SerializeField] [Range(-2.0f, 2.0f)] private float yRotationSpeed = 0.8f;
        public bool canMove;
        [SerializeField] private float translateSpeed = 2;

        private void Start()
        {
            if (cameraAR) return;
            if (Camera.main)
            {
                cameraAR = Camera.main;
                return;
            }

            var mainCamera = GameObject.FindWithTag("Main Camera");
            if (!mainCamera) return;
            if (!mainCamera.TryGetComponent<Camera>(out var resultCamera)) return;
            cameraAR = resultCamera;
            return;
        }

        private void Update()
        {
            if (canMove)
            {
                MoveCamera();
            }
        
            if (canRotate)
            {
                RotateCamera();
            }
        }

        private void MoveCamera()
        {
            var xAxisValue = Input.GetAxis("Horizontal") * translateSpeed * Time.deltaTime;
            var zAxisValue = Input.GetAxis("Vertical") * translateSpeed * Time.deltaTime;
            if(cameraAR == null) return;
            cameraAR.transform.Translate(new Vector3(xAxisValue, 0.0f, zAxisValue));
        }
    
        private void RotateCamera()
        {
            if (!Input.GetMouseButton(1)) return;
            var xDeg = Input.GetAxis("Mouse X") * xRotationSpeed * 100 * -1 * Time.deltaTime;
            var yDeg = Input.GetAxis("Mouse Y") * yRotationSpeed * 100 * -1 * Time.deltaTime;
        
            if(cameraAR == null) return;
            cameraAR.transform.Rotate(new Vector3(yDeg, -xDeg,0));
            var z = cameraAR.transform.eulerAngles.z;
            cameraAR.transform.Rotate(0, 0, -z);
        }
    
#endif
    }
}
using SilverTau.RoomPlanUnity;
using TMPro;
using UnityEngine;

namespace SilverTau.Sample
{
    public class ModelViewer : MonoBehaviour
    {
        [SerializeField] private MenuUIManager uIManager;
        
        [Space(10)]
        [Header("Common")]
        [SerializeField] private Camera mainCamera;
        [SerializeField] private GameObject modelViewer;
        [SerializeField] private Transform modelViewerContainer;
        [SerializeField] private RoomPlanObject prefabRoomPlanObject;
        
        [Space(10)]
        [Header("UI")]
        [SerializeField] private GameObject loader;
        [SerializeField] private TextMeshProUGUI textTitle;

        private RoomBuilder _roomBuilder;
        
        private void Start()
        {
        }
        
        /// <summary>
        /// A method that opens a model viewer (3В) with a parameter.
        /// </summary>
        /// <param name="scan">Scan option.</param>
        public void Open(Scan scan)
        {
            loader.SetActive(true);
            
            _roomBuilder = new RoomBuilder
            {
                container = modelViewerContainer,
                prefabRoomPlanObject = prefabRoomPlanObject,
                createFloor = RoomPlanUnityKit.CurrentRoomPlanUnityKitSettings.createFloorToRoomBuilder
            };
            
            _roomBuilder.CreateRoomFromSnapshot(scan.snapshot, () =>
            {
                textTitle.text = scan.name;
                uIManager.OpenMenu("model-viewer");
                modelViewer.SetActive(true);
                mainCamera.gameObject.SetActive(false);
                loader.SetActive(false);
            });
        }
        
        /// <summary>
        /// A method that closes the model viewer with a parameter.
        /// </summary>
        public void Close()
        {
            loader.SetActive(true);
            
            _roomBuilder.Dispose(() =>
            {
                uIManager.OpenMenu("scans");
                modelViewer.SetActive(false);
                mainCamera.gameObject.SetActive(true);
                
                loader.SetActive(false);
                _roomBuilder = null;
            });
        }
    }
}

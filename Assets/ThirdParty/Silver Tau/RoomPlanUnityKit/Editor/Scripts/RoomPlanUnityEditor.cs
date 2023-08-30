using UnityEditor;
using UnityEngine;

namespace SilverTau.RoomPlanUnity
{
    [CustomEditor(typeof(RoomPlanUnityKit))]
    public class RoomPlanUnityEditor : RoomPlanUnityPackageEditor
    {
        private RoomPlanUnityKit _target;
        
        private void OnEnable()
        {
            if (target) _target = (RoomPlanUnityKit)target;
        }
        
        public override void OnInspectorGUI()
        {
            BoxLogo(_target, " <b><color=#ffffff>Solution</color></b>");
            
            base.OnInspectorGUI();
        }
        private static GameObject CreateElementRoot(string name)
        {
	        var child = new GameObject(name);
	        Undo.RegisterCreatedObjectUndo(child, "Create " + name);
	        Selection.activeGameObject = child;
	        return child;
        }

        static GameObject CreateObject(string name, GameObject parent)
        {
	        var go = new GameObject(name);
	        GameObjectUtility.SetParentAndAlign(go, parent);
	        return go;
        }
        
        [MenuItem("GameObject/Silver Tau/RoomPlan/RPU Kit", false)]
		static public void AddRoomPlanUnityKit()
		{
			var rPUKit = CreateElementRoot("RPU Kit");
			
			var roomPlanUnityKitChild = CreateObject("RoomPlan Unity Kit", rPUKit);
			roomPlanUnityKitChild.AddComponent<RoomPlanUnityKit>();
			
			var capturedRoomSnapshotChild = CreateObject("Captured Room Snapshot", rPUKit);
			capturedRoomSnapshotChild.AddComponent<CapturedRoomSnapshot>();
			
			var sessionOriginChild = CreateObject("Session Origin", rPUKit);
			var rPUSessionCamera = sessionOriginChild.AddComponent<RPUSessionCamera>();
			
			var arCameraChild = CreateObject("AR Camera", sessionOriginChild);
			var arCamera = arCameraChild.AddComponent<Camera>();
			arCamera.depth = 3;
			arCamera.backgroundColor = new Color(0, 0, 0, 0);
			arCamera.clearFlags = CameraClearFlags.Color;
			
			rPUSessionCamera.CurrentARCamera = arCamera;
		}
    }
}
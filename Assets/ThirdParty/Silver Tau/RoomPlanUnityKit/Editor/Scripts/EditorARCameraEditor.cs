using UnityEditor;

namespace SilverTau.RoomPlanUnity
{
    [CustomEditor(typeof(EditorARCamera))]
    public class EditorARCameraEditor : RoomPlanUnityPackageEditor
    {
        private EditorARCamera _target;
        
        private void OnEnable()
        {
            if (target) _target = (EditorARCamera)target;
        }
        
        public override void OnInspectorGUI()
        {
            BoxLogo(_target, " <b><color=#ffffff>Editor AR Camera</color></b>");
            
            base.OnInspectorGUI();
        }
    }
}
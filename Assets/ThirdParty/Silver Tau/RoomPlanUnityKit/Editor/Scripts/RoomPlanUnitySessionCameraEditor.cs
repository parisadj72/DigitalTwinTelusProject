using UnityEditor;

namespace SilverTau.RoomPlanUnity
{
    [CustomEditor(typeof(RPUSessionCamera))]
    public class RoomPlanUnitySessionCameraEditor : RoomPlanUnityPackageEditor
    {
        private RPUSessionCamera _target;
        
        private void OnEnable()
        {
            if (target) _target = (RPUSessionCamera)target;
        }
        
        public override void OnInspectorGUI()
        {
            BoxLogo(_target, " <b><color=#ffffff>Session Camera</color></b>");
            
            base.OnInspectorGUI();
        }
    }
}
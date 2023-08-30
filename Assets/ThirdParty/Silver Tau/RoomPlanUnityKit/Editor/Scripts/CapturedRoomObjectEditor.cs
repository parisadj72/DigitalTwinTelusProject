using UnityEditor;

namespace SilverTau.RoomPlanUnity
{
    [CustomEditor(typeof(CapturedRoomObject))]
    public class CapturedRoomObjectEditor : RoomPlanUnityPackageEditor
    {
        private CapturedRoomObject _target;
        
        private void OnEnable()
        {
            if (target) _target = (CapturedRoomObject)target;
        }
        
        public override void OnInspectorGUI()
        {
            BoxLogo(_target, " <b><color=#ffffff>Captured Room Object</color></b>");
            
            base.OnInspectorGUI();
        }
    }
}
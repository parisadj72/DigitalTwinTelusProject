using UnityEditor;

namespace SilverTau.RoomPlanUnity
{
    [CustomEditor(typeof(CapturedRoomSnapshot))]
    public class CapturedRoomSnapshotEditor : RoomPlanUnityPackageEditor
    {
        private CapturedRoomSnapshot _target;
        
        private void OnEnable()
        {
            if (target) _target = (CapturedRoomSnapshot)target;
        }
        
        public override void OnInspectorGUI()
        {
            BoxLogo(_target, " <b><color=#ffffff>Captured Room Snapshot</color></b>");
            
            base.OnInspectorGUI();
        }
    }
}
using UnityEditor;

namespace SilverTau.RoomPlanUnity
{
    [CustomEditor(typeof(RPUDelegateEvents))]
    public class RoomPlanUnityDelegateEventsEditor : RoomPlanUnityPackageEditor
    {
        private RPUDelegateEvents _target;
        
        private void OnEnable()
        {
            if (target) _target = (RPUDelegateEvents)target;
        }
        
        public override void OnInspectorGUI()
        {
            BoxLogo(_target, " <b><color=#ffffff>Delegate Events</color></b>");
            
            base.OnInspectorGUI();
        }
    }
}
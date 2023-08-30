using System;

namespace SilverTau.RoomPlanUnity
{
    public class CapturedRoomSnapshot : RoomPlanCapturedSnapshot
    {
        public override void OnEnable()
        {
            base.OnEnable();
            Dispose();
            RoomPlanUnityKit.roomSnapshot += RoomSnapshot;
        }
        
        public override void Start()
        {
            base.Start();
            createFloor = RoomPlanUnityKit.CurrentRoomPlanUnityKitSettings.createFloorToAR;
        }

        public override void Dispose(Action callback = null)
        {
            base.Dispose(callback);
        }

        public override void EditorRoomSnapshot(string snapshot)
        {
            base.EditorRoomSnapshot(snapshot);
        }

        /// <summary>
        /// A method that allows you to change the prefab RoomPlanObject at any time.
        /// </summary>
        /// <param name="roomPlanObject">Target prefab RoomPlanObject.</param>
        public void SetCapturedRoomObjectPrefab(RoomPlanObject roomPlanObject)
        {
            CapturedRoomObjectPrefab = roomPlanObject;
        }
    }
}
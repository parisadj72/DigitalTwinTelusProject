using UnityEngine;

namespace Crosstales.OnlineCheck.Tool.SpeedTestNET
{
   /// <summary>Setup the project to use SpeedTestNET.</summary>
#if UNITY_EDITOR
   [UnityEditor.InitializeOnLoadAttribute]
#endif
   public class SetupProject
   {
      #region Constructor

      static SetupProject()
      {
         setup();
      }

      #endregion


      #region Public methods

      [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
      private static void setup()
      {
         Crosstales.Common.Util.Singleton<SpeedTestNET>.PrefabPath = "Prefabs/SpeedTestNET";
         Crosstales.Common.Util.Singleton<SpeedTestNET>.GameObjectName = "SpeedTestNET";
      }

      #endregion
   }
}
// © 2020-2023 crosstales LLC (https://www.crosstales.com)
using UnityEngine;

namespace Crosstales.OnlineCheck.Tool.SpeedTest
{
   /// <summary>Setup the project to use SpeedTest.</summary>
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
         Crosstales.Common.Util.Singleton<SpeedTest>.PrefabPath = "Prefabs/SpeedTest";
         Crosstales.Common.Util.Singleton<SpeedTest>.GameObjectName = "SpeedTest";
      }

      #endregion
   }
}
// © 2022-2023 crosstales LLC (https://www.crosstales.com)
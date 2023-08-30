#if UNITY_EDITOR
using UnityEditor;
using Crosstales.OnlineCheck.EditorUtil;
using Crosstales.OnlineCheck.Util;

namespace Crosstales.OnlineCheck.EditorIntegration
{
   /// <summary>Editor component for the "Tools"-menu.</summary>
   public static class SpeedTestNETMenu
   {
      [MenuItem("Tools/" + Constants.ASSET_NAME + "/Prefabs/" + Constants.SPEEDTESTNET_SCENE_OBJECT_NAME, false, EditorHelper.MENU_ID + 90)]
      private static void AddSpeedTestNET()
      {
         EditorHelper.InstantiatePrefab(Constants.SPEEDTESTNET_SCENE_OBJECT_NAME, $"{EditorConfig.ASSET_PATH}Extras/SpeedTestNET/Resources/Prefabs/");
      }

      [MenuItem("Tools/" + Constants.ASSET_NAME + "/Prefabs/" + Constants.SPEEDTESTNET_SCENE_OBJECT_NAME, true)]
      private static bool AddSpeedTestNETValidator()
      {
         return !EditorHelper.isSpeedTestNETInScene;
      }
   }
}
#endif
// © 2020-2023 crosstales LLC (https://www.crosstales.com)
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using Crosstales.OnlineCheck.EditorUtil;

namespace Crosstales.OnlineCheck.EditorTask
{
   /// <summary>Automatically adds the necessary TrueRandom-prefabs to the current scene.</summary>
   [InitializeOnLoad]
   public class AutoInitialize
   {
      #region Variables

      private static Scene currentScene;

      #endregion


      #region Constructor

      static AutoInitialize()
      {
         EditorApplication.hierarchyChanged += hierarchyWindowChanged;
      }

      #endregion


      #region Private static methods

      private static void hierarchyWindowChanged()
      {
         if (currentScene != EditorSceneManager.GetActiveScene())
         {
            if (EditorConfig.PREFAB_AUTOLOAD)
            {
               if (!EditorHelper.isOnlineCheckInScene)
                  EditorHelper.InstantiatePrefab(Crosstales.OnlineCheck.Util.Constants.ONLINECHECK_SCENE_OBJECT_NAME);
            }

            currentScene = EditorSceneManager.GetActiveScene();
         }
      }

      #endregion
   }
}
#endif
// © 2017-2023 crosstales LLC (https://www.crosstales.com)
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Crosstales.OnlineCheck.EditorIntegration.SpeedTest
{
   /// <summary>Installs the Demos-package.</summary>
   [InitializeOnLoad]
   public abstract class ZInstaller : Crosstales.Common.EditorTask.BaseInstaller
   {
      #region Constructor

      static ZInstaller()
      {
#if !CT_OC_DEMO && !CT_DEVELOP
         string path = $"{Application.dataPath}{Crosstales.OnlineCheck.EditorUtil.EditorConfig.ASSET_PATH}";

         installPackage(path, "Demos.unitypackage", "CT_OC_DEMO");
#endif
      }

      #endregion
   }
}
#endif
// © 2022-2023 crosstales LLC (https://www.crosstales.com)
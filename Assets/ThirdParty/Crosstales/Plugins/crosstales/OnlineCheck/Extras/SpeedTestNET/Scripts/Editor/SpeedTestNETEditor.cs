#if UNITY_EDITOR && (NET_4_6 || NET_STANDARD_2_0)
using UnityEngine;
using UnityEditor;
using Crosstales.OnlineCheck.Tool.SpeedTestNET;

namespace Crosstales.OnlineCheck.EditorExtension
{
   /// <summary>Custom editor for the 'SpeedTestNET'-class.</summary>
   //[InitializeOnLoad]
   [CustomEditor(typeof(SpeedTestNET))]
   public class SpeedTestNETEditor : Editor
   {
      #region Variables

      private SpeedTestNET script;

      private static bool showTD;

      #endregion


      #region Editor methods

      private void OnEnable()
      {
         script = (SpeedTestNET)target;
         EditorApplication.update += onUpdate;
      }

      private void OnDisable()
      {
         EditorApplication.update -= onUpdate;
      }

/*
        public override bool RequiresConstantRepaint()
        {
            return true;
        }
*/
      public override void OnInspectorGUI()
      {
         DrawDefaultInspector();

         Crosstales.OnlineCheck.EditorUtil.EditorHelper.SeparatorUI();

         if (script.isActiveAndEnabled)
         {
            EditorStyles.foldout.fontStyle = FontStyle.Bold;
            showTD = EditorGUILayout.Foldout(showTD, "Speed Test Status");
            EditorStyles.foldout.fontStyle = FontStyle.Normal;

            if (showTD)
            {
               EditorGUI.indentLevel++;

               if (script.LastDuration > 0)
               {
                  GUILayout.Label("Server details:", EditorStyles.boldLabel);
                  GUILayout.Label(script.LastServer == null ? $"Server:{Crosstales.OnlineCheck.Util.Constants.TAB}{Crosstales.OnlineCheck.Util.Constants.TAB}unknown\n" : script.LastServer?.ToString());
                  GUILayout.Label($"Download speed:\t{script.LastDownloadSpeedMBps:N3} MBps ({script.LastDownloadSpeed / 1000000:N3} Mbps)");
                  GUILayout.Label($"Upload speed:{Crosstales.OnlineCheck.Util.Constants.TAB}{script.LastUploadSpeedMBps:N3} MBps ({script.LastUploadSpeed / 1000000:N3} Mbps)");
                  GUILayout.Label($"Duration:\t\t{script.LastDuration:N3} seconds");
               }
               else
               {
                  EditorGUILayout.HelpBox(script.isBusy ? "Testing the download speed, please wait..." : "Speed not tested.", MessageType.Info);
               }

               if (Crosstales.OnlineCheck.Util.Helper.isEditorMode)
               {
                  GUI.enabled = !script.isBusy;

                  if (GUILayout.Button(new GUIContent(" Refresh", Crosstales.OnlineCheck.EditorUtil.EditorHelper.Icon_Reset, "Restart the speed test.")))
                  {
                     script.Test();
                  }

                  GUI.enabled = true;
               }

               EditorGUI.indentLevel--;
            }
         }
         else
         {
            EditorGUILayout.HelpBox("Script is disabled!", MessageType.Info);
         }
      }

      #endregion


      #region Private methods

      private void onUpdate()
      {
         Repaint();
      }

      #endregion
   }
}
#endif
// © 2020-2023 crosstales LLC (https://www.crosstales.com)
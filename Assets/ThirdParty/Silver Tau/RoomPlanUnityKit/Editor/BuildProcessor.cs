using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace SilverTau.RoomPlanUnity
{
    public class BuildProcessor : IPreprocessBuildWithReport
    {
        public int callbackOrder { get { return 0; } }
        public void OnPreprocessBuild(BuildReport report)
        {
            PlayerSettings.preserveFramebufferAlpha = true;
        }
    }
}

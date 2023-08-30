using UnityEngine;
using UnityEngine.UI;
using Crosstales.OnlineCheck.Tool.SpeedTestNET;

namespace Crosstales.OnlineCheck.Demo
{
   /// <summary>GUI component for SpeedTestNET.</summary>
   public class GUISpeedNET : MonoBehaviour
   {
      #region Variables

      public Text Result;
      public Button CheckButton;

      #endregion

#if NET_4_6 || NET_STANDARD_2_0

      #region MonoBehaviour methods

      private void Start()
      {
         SpeedTestNET.Instance.OnTestCompleted += onTestCompleted;

         if (SpeedTestNET.Instance.LastServer != null)
            onTestCompleted(SpeedTestNET.Instance.LastServer, SpeedTestNET.Instance.LastDuration, SpeedTestNET.Instance.LastDownloadSpeed, SpeedTestNET.Instance.LastDownloadSpeed);
      }

      private void OnDestroy()
      {
         if (SpeedTestNET.Instance != null)
            SpeedTestNET.Instance.OnTestCompleted -= onTestCompleted;
      }

      #endregion


      #region Public methods

      public void Test()
      {
         if (SpeedTestNET.Instance.isPlatformSupported)
         {
            Result.text = "<i>Please wait...</i>";
            SpeedTestNET.Instance.Test();
         }
         else
         {
            Result.text = "<color=red>Not supported under WSA and WebGL!</color>";
         }
      }

      #endregion


      #region Private methods

      private void onTestCompleted(Crosstales.OnlineCheck.Tool.SpeedTestNET.Model.Server server, double duration, double downloadspeed, double uploadspeed)
      {
         Result.text = $"<b>Server</b>{System.Environment.NewLine}{server}{System.Environment.NewLine}Download speed: <b>{SpeedTestNET.Instance.LastDownloadSpeedMBps:N3} MBps</b> ({downloadspeed / 1000000:N3} Mbps){System.Environment.NewLine}Upload speed: <b>{SpeedTestNET.Instance.LastUploadSpeedMBps:N3} MBps</b> ({uploadspeed / 1000000:N3} Mbps){System.Environment.NewLine}Duration: <b>{duration:N3} seconds</b>";
      }

      #endregion

#endif
   }
}
// © 2020-2023 crosstales LLC (https://www.crosstales.com)
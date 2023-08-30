using UnityEngine;
using UnityEngine.UI;
using Crosstales.OnlineCheck.Tool.SpeedTest;
using Crosstales.OnlineCheck.Tool.SpeedTest.Model.Enum;

namespace Crosstales.OnlineCheck.Demo
{
   /// <summary>GUI component for SpeedTest.</summary>
   public class GUISpeed : MonoBehaviour
   {
      #region Variables

      public Text Result;
      public Button CheckButton;

      private TestSize size = TestSize.MEDIUM;

      #endregion


      #region MonoBehaviour methods

      private void Start()
      {
         SpeedTest.Instance.OnTestCompleted += onTestCompleted;

         if (!string.IsNullOrEmpty(SpeedTest.Instance.LastURL))
            onTestCompleted(SpeedTest.Instance.LastURL, SpeedTest.Instance.LastDataSize, SpeedTest.Instance.LastDuration, SpeedTest.Instance.LastSpeed);
      }

      private void OnDestroy()
      {
         if (SpeedTest.Instance != null)
            SpeedTest.Instance.OnTestCompleted -= onTestCompleted;
      }

      #endregion


      #region Public methods

      public void Test()
      {
         if (SpeedTest.Instance.isPlatformSupported)
         {
            Result.text = "<i>Please wait...</i>";
            SpeedTest.Instance.Test(size);
         }
         else
         {
            Result.text = "<color=red>Not supported under WSA and WebGL!</color>";
         }
      }

      public void SetSize(int value)
      {
         switch (value)
         {
            case 0:
               size = TestSize.SMALL;
               break;
            case 1:
               size = TestSize.MEDIUM;
               break;
            default:
               size = TestSize.LARGE;
               break;
         }
      }

      #endregion


      #region Private methods

      private void onTestCompleted(string url, long datasize, double duration, double speed)
      {
         Result.text = $"{url}{System.Environment.NewLine}Speed: <b>{SpeedTest.Instance.LastSpeedMBps:N3} MBps</b> ({speed / 1000000:N3} Mbps){System.Environment.NewLine}Duration: <b>{duration:N3} seconds</b>{System.Environment.NewLine}Data size: <b>{SpeedTest.Instance.LastDataSizeMB:N2} MB</b>";
      }

      #endregion
   }
}
// © 2020-2023 crosstales LLC (https://www.crosstales.com)
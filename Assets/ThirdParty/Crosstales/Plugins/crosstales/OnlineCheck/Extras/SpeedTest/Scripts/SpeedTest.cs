using System.Collections;
using UnityEngine;
using Crosstales.OnlineCheck.Util;
using Crosstales.OnlineCheck.Tool.SpeedTest.Model.Enum;

namespace Crosstales.OnlineCheck.Tool.SpeedTest
{
   [System.Serializable]
   public class SpeedTestCompleteEvent : UnityEngine.Events.UnityEvent<double, double>
   {
   }

   /// <summary>Test the download speed of the Internet connection.</summary>
   [ExecuteInEditMode]
   [DisallowMultipleComponent]
   [HelpURL("https://www.crosstales.com/media/data/assets/OnlineCheck/api/class_crosstales_1_1_online_check_1_1_tool_1_1_speed_test.html")] //TODO update?
   public class SpeedTest : Crosstales.Common.Util.Singleton<SpeedTest>
   {
      #region Variables

      [UnityEngine.Serialization.FormerlySerializedAsAttribute("DataSize")] [Header("General Settings"), Tooltip("Data size for the speed test (default: SMALL)."), SerializeField]
      private TestSize dataSize = TestSize.MEDIUM;

      [UnityEngine.Serialization.FormerlySerializedAsAttribute("SmallURL")] [Tooltip("URL for the small data speed test (default: https://dl.google.com/googletalk/googletalk-setup.exe (1.5MB))."), SerializeField]
      private string smallURL = "https://dl.google.com/googletalk/googletalk-setup.exe"; //1.5MB

      [UnityEngine.Serialization.FormerlySerializedAsAttribute("MediumURL")] [Tooltip("URL for the medium data speed test (default: https://dl.google.com/dl/earth/client/advanced/current/googleearthprowin-7.3.2-x64.exe (62MB))."), SerializeField]
      private string mediumURL =
         "https://dl.google.com/dl/earth/client/advanced/current/googleearthprowin-7.3.2-x64.exe"; //62MB

      [UnityEngine.Serialization.FormerlySerializedAsAttribute("LargeURL")] [Tooltip("URL for the large data speed test (default: https://dl.google.com/drive-file-stream/GoogleDriveFSSetup.exe (195MB))."), SerializeField]
      private string largeURL = "https://dl.google.com/drive-file-stream/GoogleDriveFSSetup.exe"; // 195MB

      //url = "https://speed.hetzner.de/100MB.bin";
      //url = "https://speed.hetzner.de/1GB.bin";
      //url = "https://speed.hetzner.de/10GB.bin";

      [UnityEngine.Serialization.FormerlySerializedAsAttribute("RunOnStart")] [Header("Behaviour Settings"), Tooltip("Start at runtime (default: false)."), SerializeField]
      private bool runOnStart;

#if (!UNITY_WSA && !UNITY_XBOXONE) || UNITY_EDITOR
      private System.Threading.Thread worker;
#endif

      #endregion


      #region Events

      [Header("Events")] public SpeedTestCompleteEvent OnSpeedTestComplete;

      /// <summary>Callback to determine whether the speed test has completed.</summary>
      public delegate void TestCompleted(string url, long dataSize, double duration, double speed);

      /// <summary>An event triggered whenever the speed test has completed.</summary>
      public event TestCompleted OnTestCompleted;

      #endregion


      #region Properties

      /// <summary>Data size for the speed test.</summary>
      public TestSize DataSize
      {
         get => dataSize;
         set => dataSize = value;
      }

      /// <summary>URL for the small data speed test.</summary>
      public string SmallUrl
      {
         get => smallURL;
         set => smallURL = value;
      }

      /// <summary>URL for the medium data speed test.</summary>
      public string MediumUrl
      {
         get => mediumURL;
         set => mediumURL = value;
      }

      /// <summary>URL for the large data speed test.</summary>
      public string LargeUrl
      {
         get => largeURL;
         set => largeURL = value;
      }

      /// <summary>Start at runtime.</summary>
      public bool RunOnStart
      {
         get => runOnStart;
         set => runOnStart = value;
      }

      /// <summary>Returns the last URL.</summary>
      /// <returns>Last URL.</returns>
      public string LastURL { get; private set; }

      /// <summary>Returns the last data size in bits.</summary>
      /// <returns>Last data size in bits.</returns>
      public long LastDataSize { get; private set; }

      /// <summary>Returns the last data size in mega bytes (MB).</summary>
      /// <returns>Last data size in mega bytes (MB).</returns>
      public double LastDataSizeMB => (double)LastDataSize / 8 / 1024 / 1024;

      /// <summary>Returns the last test duration size seconds.</summary>
      /// <returns>Last test duration size seconds.</returns>
      public double LastDuration { get; private set; }

      /// <summary>Returns the last test speed in bits-per-second (bps).</summary>
      /// <returns>Last test speed in bits-per-second (bps).</returns>
      public double LastSpeed { get; private set; }

      /// <summary>Returns the last test speed in mega bytes-per-second (MBps).</summary>
      /// <returns>Last test speed in mega bytes-per-second (MBps).</returns>
      public double LastSpeedMBps => LastSpeed / 8 / 1024 / 1024;

      /// <summary>Returns true if SpeedTest is busy.</summary>
      /// <returns>True if if SpeedTest is busy.</returns>
      public bool isBusy { get; private set; }

      /// <summary>Indicates if SpeedTest is supporting the current platform.</summary>
      /// <returns>True if SpeedTest supports current platform.</returns>
      public bool isPlatformSupported => !Helper.isWebPlatform && !Helper.isWSABasedPlatform;

      #endregion


      #region MonoBehaviour methods

      protected override void OnApplicationQuit()
      {
#if (!UNITY_WSA && !UNITY_WEBGL && !UNITY_XBOXONE) || UNITY_EDITOR
         worker.CTAbort();
#endif
         base.OnApplicationQuit();
      }

      private void Start()
      {
         if (runOnStart && !Helper.isEditorMode)
            Test();
      }

      #endregion


      #region Public methods

      /// <summary>Checks the Internet download speed with the 'DataSize'-variable.</summary>
      public void Test()
      {
         Test(dataSize);
      }

      /// <summary>Checks the Internet download speed with a given data size.</summary>
      /// <param name="size">Data size for the test</param>
      public void Test(TestSize size)
      {
         if (this != null && !isActiveAndEnabled)
            return;

         if (!isBusy)
            StartCoroutine(test(size, string.Empty));
      }

      /// <summary>Checks the Internet download speed with a given url.</summary>
      /// <param name="url">URL for the test</param>
      public void Test(string url)
      {
         if (this != null && !isActiveAndEnabled)
            return;

         if (!isBusy)
            StartCoroutine(test(TestSize.SMALL, url));
      }

      #endregion


      #region Private methods

      private IEnumerator test(TestSize size, string url)
      {
#if (!UNITY_WEBGL && !UNITY_WSA && !UNITY_XBOXONE) || UNITY_EDITOR
         if (worker?.IsAlive != true)
         {
            worker = new System.Threading.Thread(() => speedTest(size, url));
            worker.Start();

            do
            {
               yield return null;
            } while (worker.IsAlive);

            onTestCompleted(LastURL, LastDataSize, LastDuration, LastSpeed);
         }
#else
			Debug.LogWarning("'SpeedTest' is not supported under the current platform!", this);
			yield return null;
#endif
      }

      private void speedTest(TestSize size, string url)
      {
#if (!UNITY_WSA && !UNITY_XBOXONE) || UNITY_EDITOR
         try
         {
            isBusy = true;

            LastDuration = 0;
            LastSpeed = 0;
            LastDataSize = 0;

            System.Net.ServicePointManager.ServerCertificateValidationCallback = Crosstales.Common.Util.NetworkHelper.RemoteCertificateValidationCallback;

            string _url;

            if (string.IsNullOrEmpty(url))
            {
               switch (size)
               {
                  case TestSize.SMALL:
                     _url = smallURL;
                     break;
                  case TestSize.MEDIUM:
                     _url = mediumURL;
                     break;
                  default:
                     _url = largeURL;
                     break;
               }
            }
            else
            {
               _url = url;
            }

            if (string.IsNullOrEmpty(_url))
            {
               Debug.LogWarning($"Url is empty for {size} - can't execute SpeedTest!", this);
            }
            else
            {
               LastURL = _url;

               using (System.Net.WebClient client = new CTWebClientNotCached())
               {
                  System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
                  byte[] data = client.DownloadData(_url);
                  sw.Stop();

                  LastDuration = sw.Elapsed.TotalSeconds;
                  LastDataSize = data.LongLength * 8;
                  LastSpeed = LastDataSize / LastDuration;
               }
            }
         }
         catch (System.Exception ex)
         {
            Debug.LogWarning($"Couldn't execute SpeedTest: {ex}", this);
         }

         isBusy = false;
#endif
      }

      #endregion


      #region Event-trigger methods

      private void onTestCompleted(string url, long filesize, double duration, double speed)
      {
         if (Config.DEBUG)
            Debug.Log($"onTestCompleted: {url} - {filesize} - {duration} - {speed}", this);

         if (!Helper.isEditorMode)
            OnSpeedTestComplete?.Invoke(duration, speed);

         OnTestCompleted?.Invoke(url, filesize, duration, speed);
      }

      #endregion
   }
}
// © 2020-2023 crosstales LLC (https://www.crosstales.com)
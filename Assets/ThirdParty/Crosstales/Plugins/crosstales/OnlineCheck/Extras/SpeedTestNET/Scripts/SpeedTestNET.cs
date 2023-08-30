using System.Collections;
using System.Linq;
using UnityEngine;
using Crosstales.OnlineCheck.Util;

namespace Crosstales.OnlineCheck.Tool.SpeedTestNET
{
   [System.Serializable]
   public class SpeedTestNETCompleteEvent : UnityEngine.Events.UnityEvent<double, double, double>
   {
   }

   /// <summary>Test the down- and upload speed of the Internet connection.</summary>
   [ExecuteInEditMode]
   [DisallowMultipleComponent]
   [HelpURL("https://www.crosstales.com/media/data/assets/OnlineCheck/api/class_crosstales_1_1_online_check_1_1_tool_1_1_speed_test_n_e_t_1_1_speed_test_n_e_t.html")]
   public class SpeedTestNET : Crosstales.Common.Util.Singleton<SpeedTestNET>
   {
#if NET_4_6 || NET_STANDARD_2_0

      #region Variables

      [UnityEngine.Serialization.FormerlySerializedAsAttribute("TestDownload")] [Header("General Settings"), Tooltip("Test the download speed."), SerializeField]
      private bool testDownload = true;

      [UnityEngine.Serialization.FormerlySerializedAsAttribute("TestUpload")] [Tooltip("Test the upload speed."), SerializeField]
      private bool testUpload = true;


      [UnityEngine.Serialization.FormerlySerializedAsAttribute("RunOnStart")] [Header("Behaviour Settings"), Tooltip("Start at runtime (default: false)."), SerializeField]
      private bool runOnStart;

      private static Crosstales.OnlineCheck.Tool.SpeedTestNET.SpeedTestClient client;
      private static Crosstales.OnlineCheck.Tool.SpeedTestNET.Model.Settings settings;

#if (!UNITY_WSA && !UNITY_XBOXONE) || UNITY_EDITOR
      private System.Threading.Thread worker;
#endif

      #endregion


      #region Events

      [Header("Events")] public SpeedTestNETCompleteEvent OnSpeedTestComplete;

      /// <summary>Callback to determine whether the speed test has completed.</summary>
      public delegate void TestCompleted(Crosstales.OnlineCheck.Tool.SpeedTestNET.Model.Server server, double duration, double downloadspeed, double uploadspeed);

      /// <summary>An event triggered whenever the speed test has completed.</summary>
      public event TestCompleted OnTestCompleted;

      #endregion


      #region Properties

      /// <summary>Test the download speed.</summary>
      public bool TestDownload
      {
         get => testDownload;
         set => testDownload = value;
      }

      /// <summary>Test the upload speed.</summary>
      public bool TestUpload
      {
         get => testUpload;
         set => testUpload = value;
      }

      /// <summary>Start at runtime.</summary>
      public bool RunOnStart
      {
         get => runOnStart;
         set => runOnStart = value;
      }

      /// <summary>Returns the last used server.</summary>
      /// <returns>Last used server.</returns>
      public Crosstales.OnlineCheck.Tool.SpeedTestNET.Model.Server LastServer { get; private set; }

      /// <summary>Returns the last test duration size seconds.</summary>
      /// <returns>Last test duration size seconds.</returns>
      public double LastDuration { get; private set; }

      /// <summary>Returns the last download speed in bits-per-second (bps).</summary>
      /// <returns>Last download speed in bits-per-second (bps).</returns>
      public double LastDownloadSpeed { get; private set; }

      /// <summary>Returns the last download speed in mega bytes-per-second (MBps).</summary>
      /// <returns>Last test download in mega bytes-per-second (MBps).</returns>
      public double LastDownloadSpeedMBps => LastDownloadSpeed / 8 / 1024 / 1024;

      /// <summary>Returns the last upload speed in bits-per-second (bps).</summary>
      /// <returns>Last upload speed in bits-per-second (bps).</returns>
      public double LastUploadSpeed { get; private set; }

      /// <summary>Returns the last upload speed in mega bytes-per-second (MBps).</summary>
      /// <returns>Last test upload in mega bytes-per-second (MBps).</returns>
      public double LastUploadSpeedMBps => LastUploadSpeed / 8 / 1024 / 1024;

      /// <summary>Returns true if SpeedTest is busy.</summary>
      /// <returns>True if if SpeedTest is busy.</returns>
      public bool isBusy { get; private set; }

      /// <summary>Indicates if SpeedTestNET is supporting the current platform.</summary>
      /// <returns>True if SpeedTestNET supports current platform.</returns>
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

      /// <summary>Checks the Internet down- and upload speed.</summary>
      public void Test()
      {
         if (this != null && !isActiveAndEnabled)
            return;

         if (!isBusy)
            StartCoroutine(test());
      }

      #endregion


      #region Private methods

      private IEnumerator test()
      {
#if (!UNITY_WEBGL && !UNITY_WSA && !UNITY_XBOXONE) || UNITY_EDITOR
         if (worker?.IsAlive != true)
         {
            worker = new System.Threading.Thread(speedTest);
            worker.Start();

            do
            {
               yield return null;
            } while (worker.IsAlive);

            onTestCompleted(LastServer, LastDuration, LastDownloadSpeed, LastUploadSpeed);
         }
#else
			Debug.LogWarning("'SpeedTestNET' is not supported under the current platform!", this);
			yield return null;
#endif
      }

      private void speedTest()
      {
         try
         {
            isBusy = true;

            System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();

            if (Config.DEBUG)
               Debug.Log("Getting speedtest.net settings and server list...", this);

            client = new Crosstales.OnlineCheck.Tool.SpeedTestNET.SpeedTestClient();
            settings = client.GetSettings();

            System.Collections.Generic.IEnumerable<Crosstales.OnlineCheck.Tool.SpeedTestNET.Model.Server> servers = selectServers();
            LastServer = servers.OrderBy(x => x.Latency).First();

            if (Config.DEBUG)
               Debug.Log("Testing speed...", this);

            if (testDownload)
               LastDownloadSpeed = client.TestDownloadSpeed(LastServer, settings.Download.ThreadsPerUrl) * 1024;

            if (testUpload)
               LastUploadSpeed = client.TestUploadSpeed(LastServer, settings.Upload.ThreadsPerUrl) * 1024;

            LastDuration = sw.Elapsed.TotalSeconds;
         }
         catch (System.Exception ex)
         {
            Debug.LogWarning($"Couldn't execute SpeedTestNET: {ex}", this);
         }

         isBusy = false;
      }

      private static System.Collections.Generic.IEnumerable<Crosstales.OnlineCheck.Tool.SpeedTestNET.Model.Server> selectServers()
      {
         //UnityEngine.Debug.Log("Selecting best server by distance...");
         System.Collections.Generic.List<Crosstales.OnlineCheck.Tool.SpeedTestNET.Model.Server> servers = settings.Servers.Take(10).ToList();

         foreach (Crosstales.OnlineCheck.Tool.SpeedTestNET.Model.Server server in servers)
         {
            server.Latency = client.TestServerLatency(server);
         }

         return servers;
      }

      #endregion


      #region Event-trigger methods

      private void onTestCompleted(Crosstales.OnlineCheck.Tool.SpeedTestNET.Model.Server server, double duration, double downloadspeed, double uploadspeed)
      {
         if (Config.DEBUG)
            Debug.Log($"onTestCompleted: {server} - {duration} - {downloadspeed} - {uploadspeed}", this);

         if (!Helper.isEditorMode)
            OnSpeedTestComplete?.Invoke(duration, downloadspeed, uploadspeed);

         OnTestCompleted?.Invoke(server, duration, downloadspeed, uploadspeed);
      }

      #endregion

#else
      public void Start()
      {
         Debug.LogError("'SpeedTestNET' is only supported in .NET4.x!", this);
      }
#endif
   }
}
// © 2020-2023 crosstales LLC (https://www.crosstales.com)
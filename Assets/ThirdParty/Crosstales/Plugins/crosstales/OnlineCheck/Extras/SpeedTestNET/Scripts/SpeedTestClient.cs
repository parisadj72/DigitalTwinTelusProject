#if NET_4_6 || NET_STANDARD_2_0
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Crosstales.OnlineCheck.Tool.SpeedTestNET.Model;

namespace Crosstales.OnlineCheck.Tool.SpeedTestNET
{
   /// <summary>Implementation of a SpeedTestClient.</summary>
   public class SpeedTestClient : ISpeedTestClient
   {
      private const string ConfigUrl = "https://www.speedtest.net/speedtest-config.php";

      private static readonly string[] ServersUrls =
      {
         "https://www.speedtest.net/speedtest-servers-static.php",
         "https://c.speedtest.net/speedtest-servers-static.php",
         "https://www.speedtest.net/speedtest-servers.php",
         "https://c.speedtest.net/speedtest-servers.php"
      };

      private static readonly int[] DownloadSizes = { 350, 750, 1500, 3000 };
      private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
      private const int MaxUploadSize = 4; // 400 KB

      #region ISpeedTestClient

      /// <inheritdoc />
      /// <exception cref="InvalidOperationException"></exception>
      public Settings GetSettings()
      {
         using (SpeedTestHttpClient client = new SpeedTestHttpClient())
         {
            Settings settings = client.GetConfig<Settings>(ConfigUrl).GetAwaiter().GetResult();

            ServersList serversConfig = new ServersList();
            foreach (string serversUrl in ServersUrls)
            {
               try
               {
                  serversConfig = client.GetConfig<ServersList>(serversUrl).GetAwaiter().GetResult();
                  if (serversConfig.Servers.Count > 0) break;
               }
               catch (Exception ex)
               {
                  UnityEngine.Debug.LogWarning($"Couldn't get the settings: {ex}");
               }
            }

            if (serversConfig.Servers.Count <= 0)
            {
               throw new InvalidOperationException("SpeedTest does not return any server");
            }

            string[] ignoredIds = settings.ServerConfig.IgnoreIds.Split(new[] { "'" }, StringSplitOptions.RemoveEmptyEntries);
            serversConfig.CalculateDistances(settings.Client.GeoCoordinate);
            settings.Servers = serversConfig.Servers
               .Where(s => !ignoredIds.Contains(s.Id.ToString()))
               .OrderBy(s => s.Distance)
               .ToList();

            return settings;
         }
      }

      /// <inheritdoc />
      public int TestServerLatency(Server server, int retryCount = 3)
      {
         string latencyUri = CreateTestUrl(server, "latency.txt");
         Stopwatch timer = new Stopwatch();

         using (SpeedTestHttpClient client = new SpeedTestHttpClient())
         {
            for (int ii = 0; ii < retryCount; ii++)
            {
               string testString;
               try
               {
                  timer.Start();
                  testString = client.GetStringAsync(latencyUri).ConfigureAwait(false).GetAwaiter().GetResult();
               }
               catch (WebException)
               {
                  continue;
               }
               finally
               {
                  timer.Stop();
               }

               if (!testString.CTStartsWith("test=test"))
                  throw new InvalidOperationException("Server returned incorrect test string for latency.txt");
            }
         }

         return (int)timer.ElapsedMilliseconds / retryCount;
      }

      public double TestDownloadSpeed(Server server, int simultaneousDownloads = 2, int retryCount = 2)
      {
         IEnumerable<string> testData = GenerateDownloadUrls(server, retryCount);

         return TestSpeed(testData, async (client, url) =>
         {
            byte[] data = await client.GetByteArrayAsync(url).ConfigureAwait(false);
            return data.Length;
         }, simultaneousDownloads);
      }

      public double TestUploadSpeed(Server server, int simultaneousUploads = 2, int retryCount = 2)
      {
         IEnumerable<string> testData = GenerateUploadData(retryCount);
         return TestSpeed(testData, async (client, uploadData) =>
         {
            await client.PostAsync(server.Url, new StringContent(uploadData));
            return uploadData.Length;
         }, simultaneousUploads);
      }

      #endregion


      #region Helpers

      private static double TestSpeed<T>(IEnumerable<T> testData, Func<HttpClient, T, Task<int>> doWork, int concurrencyCount = 2)
      {
         Stopwatch timer = new Stopwatch();
         SemaphoreSlim throttler = new SemaphoreSlim(concurrencyCount);

         timer.Start();
         Task<int>[] downloadTasks = testData.Select(async data =>
         {
            await throttler.WaitAsync().ConfigureAwait(false);

            using (SpeedTestHttpClient client = new SpeedTestHttpClient())
            {
               try
               {
                  int size = await doWork(client, data).ConfigureAwait(false);
                  return size;
               }
               finally
               {
                  throttler.Release();
               }
            }
         }).ToArray();

         Task.WaitAll(downloadTasks);
         timer.Stop();

         double totalSize = downloadTasks.Sum(task => task.Result);
         return totalSize * 8 / 1024 / ((double)timer.ElapsedMilliseconds / 1000);
      }

      private static IEnumerable<string> GenerateUploadData(int retryCount)
      {
         Random random = new Random();
         List<string> result = new List<string>();

         for (int sizeCounter = 1; sizeCounter < MaxUploadSize + 1; sizeCounter++)
         {
            int size = sizeCounter * 200 * 1024;
            StringBuilder builder = new StringBuilder(size);

            builder.AppendFormat("content{0}=", sizeCounter);

            for (int ii = 0; ii < size; ++ii)
            {
               builder.Append(Chars[random.Next(Chars.Length)]);
            }

            for (int ii = 0; ii < retryCount; ii++)
            {
               result.Add(builder.ToString());
            }
         }

         return result;
      }

      private static string CreateTestUrl(Server server, string file)
      {
         return new Uri(new Uri(server.Url), ".").OriginalString + file;
      }

      private static IEnumerable<string> GenerateDownloadUrls(Server server, int retryCount)
      {
         string downloadUriBase = CreateTestUrl(server, "random{0}x{0}.jpg?r={1}");
         foreach (int downloadSize in DownloadSizes)
         {
            for (int ii = 0; ii < retryCount; ii++)
            {
               yield return string.Format(downloadUriBase, downloadSize, ii);
            }
         }
      }

      #endregion
   }
}
#endif
// © 2020-2023 crosstales LLC (https://www.crosstales.com)
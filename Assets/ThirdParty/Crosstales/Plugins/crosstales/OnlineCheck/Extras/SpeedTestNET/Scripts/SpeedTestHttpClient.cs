#if NET_4_6 || NET_STANDARD_2_0
using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;

namespace Crosstales.OnlineCheck.Tool.SpeedTestNET
{
   /// <summary>Specialized HttpClient.</summary>
   internal class SpeedTestHttpClient : HttpClient
   {
      public SpeedTestHttpClient()
      {
         DefaultRequestHeaders.Add("Accept", "text/html, application/xhtml+xml, */*");
         DefaultRequestHeaders.Add("User-Agent", string.Join(" ", "Mozilla/5.0", "(KHTML, like Gecko)", $"SpeedTest.Net/{typeof(ISpeedTestClient).Assembly.GetName().Version}"));
      }

      public async Task<T> GetConfig<T>(string url)
      {
         string data = await GetStringAsync(AddTimeStamp(new Uri(url)));
         XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
         using (StringReader reader = new StringReader(data))
         {
            return (T)xmlSerializer.Deserialize(reader);
         }
      }

      private static Uri AddTimeStamp(Uri address)
      {
         UriBuilder uriBuilder = new UriBuilder(address);
         System.Collections.Specialized.NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
         query["x"] = DateTime.Now.ToFileTime().ToString(CultureInfo.InvariantCulture);
         uriBuilder.Query = query.ToString();
         return uriBuilder.Uri;
      }
   }
}
#endif
// © 2020-2023 crosstales LLC (https://www.crosstales.com)
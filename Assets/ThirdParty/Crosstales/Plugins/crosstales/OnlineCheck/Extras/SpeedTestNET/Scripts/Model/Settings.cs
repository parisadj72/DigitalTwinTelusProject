#if NET_4_6 || NET_STANDARD_2_0
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Crosstales.OnlineCheck.Tool.SpeedTestNET.Model
{
   /// <summary>Model for settings.</summary>
   [XmlRoot("settings")]
   public class Settings
   {
      [XmlElement("client")] public Client Client { get; set; }

      [XmlElement("times")] public Times Times { get; set; }

      [XmlElement("download")] public Download Download { get; set; }

      [XmlElement("upload")] public Upload Upload { get; set; }

      [XmlElement("server-config")] public ServerConfig ServerConfig { get; set; }

      public List<Server> Servers { get; set; }

      public Settings()
      {
         Servers = new List<Server>();
      }
   }
}
#endif
// © 2020-2023 crosstales LLC (https://www.crosstales.com)
using System.Xml.Serialization;

namespace Crosstales.OnlineCheck.Tool.SpeedTestNET.Model
{
   /// <summary>Model for a server configuration.</summary>
   [XmlRoot("server-config")]
   public class ServerConfig
   {
      [XmlAttribute("ignoreids")] public string IgnoreIds { get; set; }
   }
}
// © 2020-2023 crosstales LLC (https://www.crosstales.com)
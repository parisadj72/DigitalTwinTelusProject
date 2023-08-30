#if NET_4_6 || NET_STANDARD_2_0
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Crosstales.OnlineCheck.Tool.SpeedTestNET.Model
{
   /// <summary>Model for a server-list.</summary>
   [XmlRoot("settings")]
   public class ServersList
   {
      [XmlArray("servers")] [XmlArrayItem("server")] public List<Server> Servers { get; set; }

      public ServersList()
      {
         Servers = new List<Server>();
      }

      public void CalculateDistances(Coordinate clientCoordinate)
      {
         foreach (Server server in Servers)
         {
            server.Distance = clientCoordinate.GetDistanceTo(server.GeoCoordinate);
         }
      }
   }
}
#endif
// © 2020-2023 crosstales LLC (https://www.crosstales.com)
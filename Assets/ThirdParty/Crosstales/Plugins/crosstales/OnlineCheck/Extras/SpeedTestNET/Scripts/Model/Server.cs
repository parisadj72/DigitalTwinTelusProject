#if NET_4_6 || NET_STANDARD_2_0
using System;
using System.Xml.Serialization;

namespace Crosstales.OnlineCheck.Tool.SpeedTestNET.Model
{
   /// <summary>Model for a server.</summary>
   [XmlRoot("server")]
   public class Server
   {
      [XmlAttribute("id")] public int Id { get; set; }

      [XmlAttribute("name")] public string Name { get; set; }

      [XmlAttribute("country")] public string Country { get; set; }

      [XmlAttribute("sponsor")] public string Sponsor { get; set; }

      [XmlAttribute("host")] public string Host { get; set; }

      [XmlAttribute("url")] public string Url { get; set; }

      [XmlAttribute("lat")] public double Latitude { get; set; }

      [XmlAttribute("lon")] public double Longitude { get; set; }

      public double Distance { get; set; }

      public int Latency { get; set; }

      private Lazy<Coordinate> geoCoordinate;

      public Coordinate GeoCoordinate => geoCoordinate.Value;

      public Server()
      {
         // note: geo coordinate will not be recalculated on Latitude or Longitude change
         geoCoordinate = new Lazy<Coordinate>(() => new Coordinate(Latitude, Longitude));
      }

      #region Overridden methods

      public override string ToString()
      {
         System.Text.StringBuilder result = new System.Text.StringBuilder();

         result.Append("Id: ");
         result.AppendLine(Id.ToString());

         result.Append("Name: ");
         result.AppendLine(Name);

         result.Append("Country: ");
         result.AppendLine(Country);

         result.Append("Sponsor: ");
         result.AppendLine(Sponsor);

         result.Append("Host: ");
         result.AppendLine(Host);

         result.Append("Url: ");
         result.AppendLine(Url);

         result.Append("Distance: ");
         result.Append((Distance / 1000).ToString("N2"));
         result.AppendLine("km");

         result.Append("Latency: ");
         result.Append(Latency.ToString());
         result.AppendLine(" ms");

         return result.ToString();
      }

      #endregion
   }
}
#endif
// © 2020-2023 crosstales LLC (https://www.crosstales.com)
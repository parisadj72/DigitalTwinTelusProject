#if NET_4_6 || NET_STANDARD_2_0
using System;
using System.Xml.Serialization;

namespace Crosstales.OnlineCheck.Tool.SpeedTestNET.Model
{
   /// <summary>Model for a client.</summary>
   [XmlRoot("client")]
   public class Client
   {
      [XmlAttribute("ip")] public string Ip { get; set; }

      [XmlAttribute("lat")] public double Latitude { get; set; }

      [XmlAttribute("lon")] public double Longitude { get; set; }

      [XmlAttribute("isp")] public string Isp { get; set; }

      [XmlAttribute("isprating")] public double IspRating { get; set; }

      [XmlAttribute("rating")] public double Rating { get; set; }

      [XmlAttribute("ispdlavg")] public int IspAvarageDownloadSpeed { get; set; }

      [XmlAttribute("ispulavg")] public int IspAvarageUploadSpeed { get; set; }

      private readonly Lazy<Coordinate> geoCoordinate;

      public Coordinate GeoCoordinate => geoCoordinate.Value;

      public Client()
      {
         // note: geo coordinate will not be recalculated on Latitude or Longitude change
         geoCoordinate = new Lazy<Coordinate>(() => new Coordinate(Latitude, Longitude));
      }
   }
}
#endif
// © 2020-2023 crosstales LLC (https://www.crosstales.com)
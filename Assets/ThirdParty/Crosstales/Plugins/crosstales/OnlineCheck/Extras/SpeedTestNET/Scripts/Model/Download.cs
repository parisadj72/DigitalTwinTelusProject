using System.Xml.Serialization;

namespace Crosstales.OnlineCheck.Tool.SpeedTestNET.Model
{
   /// <summary>Model for a download.</summary>
   [XmlRoot("download")]
   public class Download
   {
      [XmlAttribute("testlength")] public int TestLength { get; set; }

      [XmlAttribute("initialtest")] public string InitialTest { get; set; }

      [XmlAttribute("mintestsize")] public string MinTestSize { get; set; }

      [XmlAttribute("threadsperurl")] public int ThreadsPerUrl { get; set; }
   }
}
// © 2020-2023 crosstales LLC (https://www.crosstales.com)
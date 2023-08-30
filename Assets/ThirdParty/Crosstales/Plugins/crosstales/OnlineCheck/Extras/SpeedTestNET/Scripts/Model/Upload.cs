using System.Xml.Serialization;

namespace Crosstales.OnlineCheck.Tool.SpeedTestNET.Model
{
   /// <summary>Model for an upload.</summary>
   [XmlRoot("upload")]
   public class Upload
   {
      [XmlAttribute("testlength")] public int TestLength { get; set; }

      [XmlAttribute("ratio")] public int Ratio { get; set; }

      [XmlAttribute("initialtest")] public int InitialTest { get; set; }

      [XmlAttribute("mintestsize")] public string MinTestSize { get; set; }

      [XmlAttribute("threads")] public int Threads { get; set; }

      [XmlAttribute("maxchunksize")] public string MaxChunkSize { get; set; }

      [XmlAttribute("maxchunkcount")] public string MaxChunkCount { get; set; }

      [XmlAttribute("threadsperurl")] public int ThreadsPerUrl { get; set; }
   }
}
// © 2020-2023 crosstales LLC (https://www.crosstales.com)
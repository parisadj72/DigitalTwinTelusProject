using System;

namespace Crosstales.OnlineCheck.Tool.SpeedTestNET.Model
{
   /// <summary>Model for a geo coordinate.</summary>
   public class Coordinate
   {
      public double Latitude { get; private set; }
      public double Longitude { get; private set; }

      public Coordinate(double latitude, double longitude)
      {
         Latitude = latitude;
         Longitude = longitude;
      }

      public double GetDistanceTo(Coordinate other)
      {
         if (double.IsNaN(Latitude) || double.IsNaN(Longitude) || double.IsNaN(other.Latitude) ||
             double.IsNaN(other.Longitude))
         {
            throw new ArgumentException("Argument latitude or longitude is not a number");
         }

         double d1 = Latitude * (Math.PI / 180);
         double num1 = Longitude * (Math.PI / 180);
         double d2 = other.Latitude * (Math.PI / 180);
         double num2 = other.Longitude * (Math.PI / 180) - num1;
         double d3 = Math.Pow(Math.Sin((d2 - d1) / 2), 2) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2), 2);

         return 6376500 * (2 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1 - d3)));
      }
   }
}
// © 2020-2023 crosstales LLC (https://www.crosstales.com)
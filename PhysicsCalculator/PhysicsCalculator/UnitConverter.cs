using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsCalculator
{
    public static class UnitConverter
    {
        public static double ConvertToSI(double value, string unit)
        {
            string lowerUnit = unit.ToLower();

            switch (lowerUnit)
            {
                case "km":
                    return value * 1000;           // километры в метры
                case "km/h":
                    return value * 1000 / 3600;    // км/ч в м/с
                case "min":
                    return value * 60;             // минуты в секунды
                case "h":
                    return value * 3600;           // часы в секунды
                case "g":
                    return value * 0.001;          // граммы в килограммы
                default:
                    return value;                  // по умолчанию - значение в СИ
            }
        }

        public static string GetUnitName(string unitAbbreviation)
        {
            string lowerUnit = unitAbbreviation.ToLower();

            switch (lowerUnit)
            {
                case "m":
                    return "метры";
                case "s":
                    return "секунды";
                case "m/s":
                    return "метры в секунду";
                case "m/s2":
                    return "метры в секунду в квадрате";
                case "n":
                    return "ньютоны";
                case "kg":
                    return "килограммы";
                case "j":
                    return "джоули";
                default:
                    return unitAbbreviation;
            }
        }
    }
}

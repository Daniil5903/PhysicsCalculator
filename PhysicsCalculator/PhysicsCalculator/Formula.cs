using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsCalculator
{
    public class Formula
    {
        public string Name { get; set; }
        public string Expression { get; set; }
        public Dictionary<string, string> Variables { get; set; }

        public Formula(string name, string expression, string variablesStr)
        {
            Name = name;
            Expression = expression;
            Variables = ParseVariables(variablesStr);
        }

        private Dictionary<string, string> ParseVariables(string variablesStr)
        {
            var variables = new Dictionary<string, string>();
            var pairs = variablesStr.Split(',');

            foreach (var pair in pairs)
            {
                var parts = pair.Split('-');
                if (parts.Length == 2)
                {
                    variables[parts[0].Trim()] = parts[1].Trim();
                }
                else
                {
                    throw new FormatException($"Неверный формат переменных: {pair}");
                }
            }

            return variables;
        }

        public List<string> GetVariableNames()
        {
            return Variables.Keys.ToList();
        }

        public double Calculate(Dictionary<string, double> knownValues)
        {
            var allVariables = GetVariableNames();
            var unknownVariables = allVariables.Where(v => !knownValues.ContainsKey(v)).ToList();

            if (unknownVariables.Count != 1)
            {
                throw new InvalidOperationException(
                    $"Должен быть указан ровно один неизвестный параметр. " +
                    $"Найдено неизвестных: {unknownVariables.Count}");
            }

            string unknownVar = unknownVariables[0];
            double result;

            // Определение типа формулы и выполнение расчета
            if (Name.Contains("движение") || Name.Contains("Движение"))
            {
                result = CalculateMovement(knownValues, unknownVar);
            }
            else if (Name.Contains("Ньютона") || Name.Contains("ньютона"))
            {
                result = CalculateNewton(knownValues, unknownVar);
            }
            else if (Name.Contains("энергия") || Name.Contains("Энергия"))
            {
                result = CalculateEnergy(knownValues, unknownVar);
            }
            else
            {
                throw new InvalidOperationException($"Неизвестный тип формулы: {Name}");
            }

            // Проверка физической корректности результата
            ValidatePhysicalResult(result, unknownVar);

            return result;
        }

        private double CalculateMovement(Dictionary<string, double> knownValues, string unknownVar)
        {
            // Замена switch expression на обычный switch statement
            switch (unknownVar)
            {
                case "S":
                    return knownValues["V"] * knownValues["t"];
                case "V":
                    return knownValues["S"] / knownValues["t"];
                case "t":
                    return knownValues["S"] / knownValues["V"];
                default:
                    throw new InvalidOperationException($"Неизвестная переменная для движения: {unknownVar}");
            }
        }

        private double CalculateNewton(Dictionary<string, double> knownValues, string unknownVar)
        {
            // Замена switch expression на обычный switch statement
            switch (unknownVar)
            {
                case "F":
                    return knownValues["m"] * knownValues["a"];
                case "m":
                    return knownValues["F"] / knownValues["a"];
                case "a":
                    return knownValues["F"] / knownValues["m"];
                default:
                    throw new InvalidOperationException($"Неизвестная переменная для закона Ньютона: {unknownVar}");
            }
        }

        private double CalculateEnergy(Dictionary<string, double> knownValues, string unknownVar)
        {
            if (Name.Contains("Кинетическая") || Name.Contains("кинетическая"))
            {
                // Замена switch expression на обычный switch statement
                switch (unknownVar)
                {
                    case "Ek":
                        return knownValues["m"] * Math.Pow(knownValues["V"], 2) / 2;
                    case "m":
                        return 2 * knownValues["Ek"] / Math.Pow(knownValues["V"], 2);
                    case "V":
                        return Math.Sqrt(2 * knownValues["Ek"] / knownValues["m"]);
                    default:
                        throw new InvalidOperationException($"Неизвестная переменная для кинетической энергии: {unknownVar}");
                }
            }
            else if (Name.Contains("Потенциальная") || Name.Contains("потенциальная"))
            {
                // Замена switch expression на обычный switch statement
                switch (unknownVar)
                {
                    case "Ep":
                        return knownValues["m"] * knownValues["g"] * knownValues["h"];
                    case "m":
                        return knownValues["Ep"] / (knownValues["g"] * knownValues["h"]);
                    case "g":
                        return knownValues["Ep"] / (knownValues["m"] * knownValues["h"]);
                    case "h":
                        return knownValues["Ep"] / (knownValues["m"] * knownValues["g"]);
                    default:
                        throw new InvalidOperationException($"Неизвестная переменная для потенциальной энергии: {unknownVar}");
                }
            }
            else
            {
                throw new InvalidOperationException($"Неизвестный тип энергетической формулы: {Name}");
            }
        }

        private void ValidatePhysicalResult(double result, string variable)
        {
            // Проверка на NaN и Infinity
            if (double.IsNaN(result) || double.IsInfinity(result))
            {
                throw new InvalidOperationException($"Результат вычисления некорректен: {result}");
            }

            // Проверка физической осмысленности
            if (result < 0 && IsPhysicallyPositive(variable))
            {
                throw new InvalidOperationException(
                    $"Физически некорректный результат: {variable} не может быть отрицательным");
            }
        }

        private bool IsPhysicallyPositive(string variable)
        {
            var positiveVariables = new[] { "m", "t", "S", "V", "a", "F", "Ek", "Ep", "h" };
            return positiveVariables.Contains(variable);
        }
    }
}

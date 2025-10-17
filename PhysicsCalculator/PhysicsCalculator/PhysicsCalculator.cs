using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsCalculator
{
    public class PhysicsCalculator
    {
        private FormulaManager _formulaManager;
        private FileManager _fileManager;

        public PhysicsCalculator()
        {
            _formulaManager = new FormulaManager();
            _fileManager = new FileManager();
        }

        public void Run()
        {
            try
            {
                Console.WriteLine("=== КАЛЬКУЛЯТОР ФИЗИКА ===");
                Console.WriteLine("Загрузка формул...");

                // Загрузка формул из файла
                _formulaManager.LoadFormulas("formulas.txt");
                var formulas = _formulaManager.GetFormulas();

                while (true)
                {
                    try
                    {
                        // Выбор формулы
                        Console.WriteLine("\nДоступные формулы:");
                        for (int i = 0; i < formulas.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {formulas[i].Name} ({formulas[i].Expression})");
                        }

                        Console.WriteLine("0. Выход");
                        Console.Write("\nВыберите формулу (номер): ");
                        string input = Console.ReadLine();

                        if (input == "0")
                        {
                            break;
                        }

                        if (!int.TryParse(input, out int formulaIndex) || formulaIndex < 1 || formulaIndex > formulas.Count)
                        {
                            Console.WriteLine("Ошибка: введите корректный номер формулы");
                            continue;
                        }

                        var selectedFormula = _formulaManager.GetFormula(formulaIndex - 1);
                        PerformCalculation(selectedFormula);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка: {ex.Message}");
                    }
                }

                Console.WriteLine("\nСпасибо за использование калькулятора!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Критическая ошибка: {ex.Message}");
            }
        }

        private void PerformCalculation(Formula formula)
        {
            Console.WriteLine($"\n=== Расчет по формуле: {formula.Name} ===");
            Console.WriteLine($"Выражение: {formula.Expression}");

            var knownValues = new Dictionary<string, double>();
            var variables = formula.GetVariableNames();

            // Ввод известных значений
            Console.WriteLine("\nВведите известные значения (для неизвестного параметра оставьте пустым):");

            foreach (var variable in variables)
            {
                string unit = formula.Variables[variable];
                string unitName = UnitConverter.GetUnitName(unit);

                while (true)
                {
                    Console.Write($"{variable} ({unitName}): ");
                    string input = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(input))
                    {
                        // Пользователь оставил поле пустым - это неизвестный параметр
                        break;
                    }

                    if (double.TryParse(input, out double value))
                    {
                        // Проверка физической корректности
                        if (value < 0 && IsPhysicallyPositive(variable))
                        {
                            Console.WriteLine($"Ошибка: {variable} не может быть отрицательным");
                            continue;
                        }

                        // Конвертация в СИ при необходимости
                        value = UnitConverter.ConvertToSI(value, unit);
                        knownValues[variable] = value;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Ошибка: введите корректное числовое значение");
                    }
                }
            }

            // Выполнение расчета
            try
            {
                double result = formula.Calculate(knownValues);
                var unknownVar = variables.First(v => !knownValues.ContainsKey(v));
                string unit = formula.Variables[unknownVar];
                string unitName = UnitConverter.GetUnitName(unit);

                string resultText = $"{formula.Name}: {unknownVar} = {result:F4} {unitName}";

                // Вывод результата
                Console.WriteLine($"\n★ РЕЗУЛЬТАТ: {resultText}");

                // Вывод всех использованных параметров
                Console.WriteLine("\nИспользованные параметры:");
                foreach (var kvp in knownValues)
                {
                    string paramUnit = formula.Variables[kvp.Key];
                    string paramUnitName = UnitConverter.GetUnitName(paramUnit);
                    Console.WriteLine($"  {kvp.Key} = {kvp.Value:F4} {paramUnitName}");
                }

                // Сохранение результата
                SaveCalculationResult(resultText);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка расчета: {ex.Message}");
            }
        }

        private void SaveCalculationResult(string result)
        {
            Console.Write("\nСохранить результат? (y/n): ");
            string answer = Console.ReadLine();

            if (answer?.ToLower() == "y" || answer?.ToLower() == "д")
            {
                try
                {
                    string filePath = "calculations.txt";
                    _fileManager.SaveResult(filePath, result);
                    Console.WriteLine($"Результат сохранен в файл: {Path.GetFullPath(filePath)}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка сохранения: {ex.Message}");
                }
            }
        }

        private bool IsPhysicallyPositive(string variable)
        {
            var positiveVariables = new[] { "m", "t", "S", "V", "a", "F", "Ek", "Ep", "h" };
            return positiveVariables.Contains(variable);
        }
    }

}

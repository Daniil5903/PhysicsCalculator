using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsCalculator
{
    public class FormulaManager
    {
        private List<Formula> _formulas;

        public FormulaManager()
        {
            _formulas = new List<Formula>();
        }

        public void LoadFormulas(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Файл конфигурации не найден: {filePath}");
            }

            string[] lines;
            try
            {
                lines = File.ReadAllLines(filePath);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Не удалось прочитать файл конфигурации: {ex.Message}");
            }

            if (lines.Length == 0)
            {
                throw new InvalidOperationException("Файл конфигурации не содержит данных");
            }

            _formulas.Clear();
            int lineNumber = 0;

            foreach (var line in lines)
            {
                lineNumber++;
                string trimmedLine = line.Trim();

                // Пропуск пустых строк и комментариев
                if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith("#"))
                {
                    continue;
                }

                var parts = trimmedLine.Split(';');
                if (parts.Length != 3)
                {
                    Console.WriteLine($"Предупреждение: неверный формат строки {lineNumber}. Ожидается 3 части, найдено {parts.Length}");
                    continue;
                }

                try
                {
                    var formula = new Formula(parts[0].Trim(), parts[1].Trim(), parts[2].Trim());
                    _formulas.Add(formula);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Предупреждение: некорректная формула в строке {lineNumber}: {ex.Message}");
                }
            }

            if (_formulas.Count == 0)
            {
                throw new InvalidOperationException("Не найдено валидных формул в файле конфигурации");
            }
        }

        public List<Formula> GetFormulas()
        {
            return _formulas;
        }

        public Formula GetFormula(int index)
        {
            if (index < 0 || index >= _formulas.Count)
            {
                throw new ArgumentOutOfRangeException($"Неверный индекс формулы. Допустимый диапазон: 0-{_formulas.Count - 1}");
            }

            return _formulas[index];
        }
    }
}

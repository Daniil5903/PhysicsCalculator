using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsCalculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Создание файла с формулами, если он не существует
            CreateDefaultFormulasFile();

            var calculator = new PhysicsCalculator();
            calculator.Run();

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }

        static void CreateDefaultFormulasFile()
        {
            if (!File.Exists("formulas.txt"))
            {
                try
                {
                    string[] defaultFormulas = {
                        "# Физические формулы для калькулятора",
                        "# Формат: Название;Выражение;Переменные-единицы",
                        "",
                        "Равномерное движение;S=V*t;S-m,V-m/s,t-s",
                        "Второй закон Ньютона;F=m*a;F-N,m-kg,a-m/s2",
                        "Кинетическая энергия;Ek=m*V^2/2;Ek-J,m-kg,V-m/s",
                        "Потенциальная энергия;Ep=m*g*h;Ep-J,m-kg,g-m/s2,h-m"
                    };

                    File.WriteAllLines("formulas.txt", defaultFormulas);
                    Console.WriteLine("Создан файл formulas.txt с базовыми формулами");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка создания файла формул: {ex.Message}");
                }
            }
        }
    }
}

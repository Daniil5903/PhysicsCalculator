using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsCalculator
{
    public class FileManager
    {
        public void SaveResult(string filePath, string result)
        {
            try
            {
                string directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                File.AppendAllText(filePath, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}: {result}{Environment.NewLine}");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Ошибка сохранения результата: {ex.Message}");
            }
        }
    }
}

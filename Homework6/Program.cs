using System;
using System.IO;
using System.Text;

namespace Homework6
{
    class Program
    {
        static void Main()
        {
            string[] args = Environment.GetCommandLineArgs();
            //проверяем, корректно ли заданы аргументы
            if (args.Length <= 1 || !CheckFilesPaths(args))
            {
                Console.WriteLine("Неправильно задан путь к файлу! Использование:\n");
                Console.WriteLine(Path.GetFileName(args[0]) + " <имя_файла>");
                Console.WriteLine("\nЕсли файл по указонному пути не существует, он будет создан.");
                Environment.Exit(0);
            }
            //создаем файл, если не существует
            string fileName = Path.GetFullPath(args[1]);
            if (!File.Exists(fileName))
            {
                Console.WriteLine($"Создаем файл {args[1]}");
                File.CreateText(fileName);
            }

            string msg = "Вставить новую запись? (Выход - Escape)";
            while (true)
            {
                int printed = PrintWorkers2(fileName);
                Console.WriteLine(msg);
                //Выход из цикла по Esc
                if (Console.ReadKey().Key == ConsoleKey.Escape) break;
                AddWorker(fileName, printed + 1);
            }
        }

        /// <summary>
        /// Выводит в консоль форматированного текстового файла
        /// </summary>
        /// <param name="path"></param>
        /// <returns>Возвращает количество выведенных строк</returns>
        static int PrintWorkers1(string path)
        {
            Console.Clear();
            //будем считать выведенные строки
            int stringsCount = 0;
            using (var sr = File.OpenText(path))
            {
                string s = "";
                string[] fields;
                string header = $"{"ID",-5}{"Дата и время добавления",-25}{"Ф.И.О.",-35}{"Возраст",-10}{"Рост",-5}"
                                + $"{"Дата рождения",-15}{"Место рождения"}";
                int[] fieldPos = {
                    header.IndexOf("ID"),
                    header.IndexOf("Дата и вр"),
                    header.IndexOf("Ф.И.О."),
                    header.IndexOf("Возр"),
                    header.IndexOf("Рост"),
                    header.IndexOf("Дата рож"),
                    header.IndexOf("Место")};
                Console.WriteLine(header);
                while ((s = sr.ReadLine()) != null)
                {
                    stringsCount++;
                    fields = s.Split('#');
                    for (int i = 0; i < fields.Length; i++)
                    {
                        Console.CursorLeft = fieldPos[i];
                        Console.Write(i == fields.Length - 1 ? (fields[i] + '\n') : fields[i]);
                    }
                }
            }
            return stringsCount;
        }
        static int PrintWorkers2(string path)
        {
            Console.Clear();
            var formatLines = new StringBuilder();
            formatLines.AppendFormat($"{"ID",-5}{"Дата и время добавления",-25}{"Ф.И.О.",-35}{"Возраст",-10}{"Рост",-5}" +
                                    $"{"Дата рождения",-15}{"Место рождения"}\n");
            string[] fileLines = File.ReadAllLines(path);
            string[] fields;
            foreach (string v in fileLines)
            {
                fields = v.Split('#');
                formatLines.AppendFormat($"{fields[0],-5}{fields[1],-25}{fields[2],-35}{fields[3],-10}{fields[4],-5}" +
                                        $"{fields[5],-15}{fields[6]}\n");
            }
            Console.WriteLine(formatLines.ToString());

            return fileLines.Length;
        }
        /// <summary>
        /// Добавляет запись о работнике в форматированный текстовый файл
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <param name="id">Id сотрудника</param>
        static void AddWorker(string path, int id)
        {
            using (var sw = File.AppendText(path))
            {
                sw.Write(id + "#" + DateTime.Now.ToString() + "#");
                Console.Write("\nВведите Ф.И.О.: ");
                sw.Write(Console.ReadLine() + "#");
                Console.Write("\nВведите возраст: ");
                sw.Write(Console.ReadLine() + "#");
                Console.Write("\nВведите рост: ");
                sw.Write(Console.ReadLine() + "#");
                Console.Write("\nВведите дату рождения: ");
                sw.Write(Console.ReadLine() + "#");
                Console.Write("\nВведите место рождения: ");
                sw.Write(Console.ReadLine() + "\n");
            }
        }
        /// <summary>
        /// Проверяет пути файлов на неправильные символы
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        static bool CheckFilesPaths(string[] args)
        {
            char[] invalidChars = Path.GetInvalidPathChars();

            for (int i = 0; i < args.Length; i++)
            {
                for (int j = 0; j < invalidChars.Length; j++)
                {
                    if (args[i].Contains(invalidChars[j].ToString()))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}

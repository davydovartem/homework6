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
                Console.Clear();
                int printed = PrintWorkers2(fileName);
                Console.WriteLine(msg);
                //Выход из цикла по Esc
                if (Console.ReadKey().Key == ConsoleKey.Escape) break;
                AddWorker(fileName, printed + 1);
            }
        }

        /// <summary>
        /// Выводит в консоль строки форматированного текстового файла
        /// </summary>
        /// <param name="path"></param>
        /// <returns>Возвращает количество выведенных строк</returns>
        static int PrintWorkers1(string path)
        {
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
        /// <summary>
        /// Выводит в консоль строки форматированного текстового файла
        /// </summary>
        /// <param name="path"></param>
        /// <returns>Возвращает количество выведенных строк</returns>
        static int PrintWorkers2(string path)
        {
            var formatLines = new StringBuilder(FormatString("ID", "Дата и время добавления", "Ф.И.О.", "Возраст", "Рост", "Дата рождения", "Место рождения"));
            var fileLines = File.ReadAllLines(path);
            foreach (string v in fileLines)
            {
                formatLines.Append(FormatString(v.Split('#')));
            }
            Console.WriteLine(formatLines.ToString());

            return fileLines.Length;
        }
        /// <summary>
        /// Расставляет аргументы на нужные места для красивого вывода
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        static string FormatString(params string[] args)
        {
            return String.Format($"{args[0],-5}{args[1],-25}{args[2],-35}{args[3],-10}{args[4],-5}{args[5],-15}{args[6]}\n");
        }
        /// <summary>
        /// Добавляет запись о работнике в форматированный текстовый файл
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <param name="id">Id сотрудника</param>
        static void AddWorker(string path, int id)
        {
            var sb = new StringBuilder(id.ToString());
            sb.Append("#").Append(DateTime.Now.ToString()).Append("#");
            Console.Write("\nВведите Ф.И.О.: ");
            sb.Append(Console.ReadLine()).Append("#");
            Console.Write("\nВведите возраст: ");
            sb.Append(Console.ReadLine()).Append("#");
            Console.Write("\nВведите рост: ");
            sb.Append(Console.ReadLine()).Append("#");
            Console.Write("\nВведите дату рождения: ");
            sb.Append(Console.ReadLine()).Append("#");
            Console.Write("\nВведите место рождения: ");
            sb.Append(Console.ReadLine()).AppendLine();
            File.AppendAllText(path, sb.ToString());
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

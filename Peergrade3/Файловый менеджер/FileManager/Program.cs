using System;
using System.IO;
using System.Text;

namespace FileManager
{
    class Program
    {
        /// <summary>
        /// Вывод стартового меню.
        /// </summary>
        static void ShowMenu()
        {
            Console.WriteLine("Добро пожаловать в Файловый Менеджер");
            Console.WriteLine("Специально для Вас мы предоставляем следующие услуги:");
            Console.WriteLine("1 - просмотр списка дисков компьютера и выбор диска;");
            Console.WriteLine("2 - переход в другую директорию (выбор папки);");
            Console.WriteLine("3 - просмотр списка файлов в директории;");
            Console.WriteLine("4 - вывод содержимого текстового файла в консоль в кодировке UTF-8;");
            Console.WriteLine("5 - вывод содержимого текстового файла в консоль в выбранной пользователем кодировке(предоставляется не менее трех вариантов);");
            Console.WriteLine("6 - копирование файла;");
            Console.WriteLine("7 - перемещение файла в выбранную пользователем директорию;");
            Console.WriteLine("8 - удаление файла;");
            Console.WriteLine("9 - создание простого текстового файла в кодировке UTF-8;");
            Console.WriteLine("10 - создание простого текстового файла в выбранной пользователем кодировке(предоставляется не менее трех вариантов);");
            Console.WriteLine("11 - конкатенация содержимого двух или более текстовых файлов и вывод результата в консоль в кодировке UTF-8;");
            Console.WriteLine("12 - вывод всех файлов в текущей директории по заданной маске;");
            Console.WriteLine("13 - вывод всех файлов в текущей директории и всех её поддиректориях по заданной маске; ");
            Console.WriteLine("14 - копирование все файлы из директории и всех её поддиректорий по маске в другую директорию;");
            Console.WriteLine("Перед началом работы обязательно выберите диск и директорию");
        }

        /// <summary>
        /// Выводит список дисков и предлагает выбрать один из них.
        /// </summary>
        /// <returns>Имя выбранного диска</returns>
        static DriveInfo ShowDrives()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            int number;
            try
            {
                Console.WriteLine("На выбор предложены следующие диски:");
                for (int i = 0; i < allDrives.Length; ++i)
                {
                    Console.WriteLine("{0} - {1}", i + 1, allDrives[i]);
                }
                string inputNumber = Console.ReadLine();
                while (!int.TryParse(inputNumber, out number) || number < 1 || number > allDrives.Length)
                {
                    Console.WriteLine("Некорректный ввод, попробуйте снова:");
                    inputNumber = Console.ReadLine();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"В ходе программы было вызвано исключение. Его текст:\n  {e.Message}");
                return null;
            }
            Console.WriteLine("\n\nВыбор диска прошел успешно!\n");
            return allDrives[number - 1];
        }

        /// <summary>
        /// Предоставляет выбор директории.
        /// </summary>
        /// <param name="driveName">Имя диска, с которым работает пользователь</param>
        /// <returns>Имя выбранной директории</returns>
        static string ChooseDirectory(string driveName)
        {
            string directoryName;
            try
            {
                Console.WriteLine($"Введите путь к каталогу на диске {driveName}, не указывая диск");
                Console.WriteLine("Например если требуется выбрать директорию C:\\users\\MyName\\MyFiles, то следует ввести в консоль users\\MyName\\MyFiles");
                directoryName = Console.ReadLine();
                while (!Directory.Exists(Path.Combine(driveName, directoryName)))
                {
                    Console.WriteLine("Директории не существует, попробуйте снова:");
                    directoryName = Console.ReadLine();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"В ходе программы было вызвано исключение. Его текст:\n  {e.Message}");
                return null;
            }
            Console.WriteLine("\n\nВыбор каталога прошел успешно!\n");
            return directoryName;
        }

        /// <summary>
        /// Выводит список файлов в заданной директории.
        /// </summary>
        /// <param name="path">Путь к директории</param>
        static void ShowListOfFiles(string path)
        {
            try
            {
                Console.WriteLine($"Файлы в каталоге {path}:");
                string[] files = Directory.GetFiles(path);
                Console.WriteLine($"Количество найденных файлов - {files.Length}");
                foreach (string file in files)
                {
                    Console.WriteLine(file);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"В ходе программы было вызвано исключение. Его текст:\n  {e.Message}");
                return;
            }
            Console.WriteLine("\n\nНазвания файлов выведены успешно!\n");
        }

        /// <summary>
        /// Выводит содержимое файла в кодировке UTF-8.
        /// </summary>
        /// <param name="path">Путь к директории</param>
        static void OutputFileText(string path)
        {
            try
            {
                Console.WriteLine($"Введите имя файла в каталоге {path}:");
                string fileName = Console.ReadLine();
                while (!File.Exists(Path.Combine(path, fileName)))
                {
                    Console.WriteLine("Файла не существует, попробуйте снова:");
                    fileName = Console.ReadLine();
                }
                string text;
                using (var sr = new StreamReader(Path.Combine(path, fileName), Encoding.UTF8, false))
                {
                    text = sr.ReadToEnd();
                }
                Console.WriteLine(text);
            }
            catch (Exception e)
            {
                Console.WriteLine($"В ходе программы было вызвано исключение. Его текст:\n  {e.Message}");
                return;
            }
            Console.WriteLine("\n\nСодержимое файла выведено успешно!\n");
        }

        /// <summary>
        /// Вывод текса из файла в выбранной кодировке.
        /// </summary>
        /// <param name="path">Путь до файла</param>
        /// <param name="number">Номер кодировки</param>
        static void OutputText(string path, int number)
        {
            string text;
            switch (number)
            {
                case 1:
                    using (var sr = new StreamReader(path, Encoding.Unicode, false))
                    {
                        text = sr.ReadToEnd();
                    }
                    Console.WriteLine(text);
                    break;
                case 2:
                    using (var sr = new StreamReader(path, Encoding.UTF32, false))
                    {
                        text = sr.ReadToEnd();
                    }
                    Console.WriteLine(text);
                    break;
                case 3:
                    using (var sr = new StreamReader(path, Encoding.ASCII, false))
                    {
                        text = sr.ReadToEnd();
                    }
                    Console.WriteLine(text);
                    break;
            }
        }

        /// <summary>
        /// Выводит содержимое файла в выбранной пользователем кодировке.
        /// </summary>
        /// <param name="path">Путь к директории</param>
        static void OutputFileInChosenEncoding(string path)
        {
            try
            {
                Console.WriteLine($"Введите имя файла в директории {path}:");
                string fileName = Console.ReadLine();
                while (!File.Exists(Path.Combine(path, fileName)))
                {
                    Console.WriteLine("Файла не существует, попробуйте снова:");
                    fileName = Console.ReadLine();
                }
                Console.WriteLine("Кодировки на выбор:");
                Console.WriteLine("1 - Unicode(UTF-16)");
                Console.WriteLine("2 - UTF-32");
                Console.WriteLine("3 - ASCII");
                Console.WriteLine("Введите номер нужной кодировки:");
                string inputNumber = Console.ReadLine();
                int number;
                while (!int.TryParse(inputNumber, out number) || number < 1 || number > 3)
                {
                    Console.WriteLine("Некорректный ввод, попробуйте снова:");
                    inputNumber = Console.ReadLine();
                }
                OutputText(Path.Combine(path, fileName), number);
            }
            catch (Exception e)
            {
                Console.WriteLine($"В ходе программы было вызвано исключение. Его текст:\n  {e.Message}");
                return;
            }
            Console.WriteLine("\n\nСодержимое файла выведено успешно!\n");
        }

        /// <summary>
        /// Копирует файл.
        /// </summary>
        /// <param name="path">Путь к директории</param>
        static void CopyFile(string path)
        {
            try
            {
                Console.WriteLine($"Введите имя файла в директории {path}:");
                string fileName = Console.ReadLine();
                while (!File.Exists(Path.Combine(path, fileName)))
                {
                    Console.WriteLine("Файла не существует, попробуйте снова:");
                    fileName = Console.ReadLine();
                }
                Console.WriteLine("Введите имя для нового файла:");
                string newFileName = Console.ReadLine();
                while (fileName == newFileName)
                {
                    Console.WriteLine("Имя нового файла должно отличаться от старого, попробуйте снова:");
                    newFileName = Console.ReadLine();
                }
                File.Copy(Path.Combine(path, fileName), Path.Combine(path, newFileName));
            }
            catch (Exception e)
            {
                Console.WriteLine($"В ходе программы было вызвано исключение. Его текст:\n  {e.Message}");
                return;
            }
            Console.WriteLine("\n\nКопирование прошло успешно!\n");
        }

        /// <summary>
        /// Перемещает файл.
        /// </summary>
        /// <param name="path">Путь к директории</param>
        static void MoveFile(string path)
        {
            try
            {
                Console.WriteLine($"Введите имя файла в директории {path}:");
                string fileName = Console.ReadLine();
                while (!File.Exists(Path.Combine(path, fileName)))
                {
                    Console.WriteLine("Файла не существует, попробуйте снова:");
                    fileName = Console.ReadLine();
                }
                Console.WriteLine("Введите абсолютный путь до новой директории:");
                string newPath = Console.ReadLine();
                while (!Directory.Exists(newPath))
                {
                    Console.WriteLine("Такого пути не существует, попробуйте снова:");
                    newPath = Console.ReadLine();
                }
                File.Move(Path.Combine(path, fileName), Path.Combine(newPath, fileName));
            }
            catch (Exception e)
            {
                Console.WriteLine($"В ходе программы было вызвано исключение. Его текст:\n  {e.Message}");
                return;
            }
            Console.WriteLine("\n\nПеремещение прошло успешно!\n");
        }

        /// <summary>
        /// Удаляет файл.
        /// </summary>
        /// <param name="path">Путь к директории</param>
        static void DeleteFile(string path)
        {
            try
            {
                Console.WriteLine($"Введите имя файла в директории {path}:");
                string fileName = Console.ReadLine();
                while (!File.Exists(Path.Combine(path, fileName)))
                {
                    Console.WriteLine("Файла не существует, попробуйте снова:");
                    fileName = Console.ReadLine();
                }
                File.Delete(Path.Combine(path, fileName));
            }
            catch (Exception e)
            {
                Console.WriteLine($"В ходе программы было вызвано исключение. Его текст:\n  {e.Message}");
                return;
            }
            Console.WriteLine("\n\nУдаление прошло успешно!\n");
        }

        /// <summary>
        /// Создает файл в кодировке UTF-8.
        /// </summary>
        /// <param name="path">Путь к директории</param>
        static void CreateFile(string path)
        {
            string fileName;
            try
            {
                Console.WriteLine($"Введите имя файла для создания:");
                fileName = Console.ReadLine();
                while (File.Exists(Path.Combine(path, fileName)))
                {
                    Console.WriteLine("Такое имя файла уже существует, попробуйте снова:");
                    fileName = Console.ReadLine();
                }
                Console.WriteLine("Введите текст для нового файла (если не хотите вводть текст, просто нажмите Enter):");
                string text = Console.ReadLine();
                File.WriteAllText(Path.Combine(path, fileName), text, Encoding.UTF8);
            }
            catch (Exception e)
            {
                Console.WriteLine($"В ходе программы было вызвано исключение. Его текст:\n  {e.Message}");
                return;
            }
            Console.WriteLine($"\n\nФайл {Path.Combine(path, fileName)} создан успешно!\n");
        }

        /// <summary>
        /// Создает файл в выбранной кодировке.
        /// </summary>
        /// <param name="path">Путь к директории</param>
        static void CreateFileInChosenEncoding(string path)
        {
            string fileName;
            try
            {
                Console.WriteLine($"Введите имя файла для создания:");
                fileName = Console.ReadLine();
                while (File.Exists(Path.Combine(path, fileName)))
                {
                    Console.WriteLine("Такое имя файла уже существует, попробуйте снова:");
                    fileName = Console.ReadLine();
                }
                Console.WriteLine("Кодировки на выбор:");
                Console.WriteLine("1 - Unicode(UTF-16)");
                Console.WriteLine("2 - UTF-32");
                Console.WriteLine("3 - ASCII");
                Console.WriteLine("Введите номер нужной кодировки:");
                string inputNumber = Console.ReadLine();
                int number;
                while (!int.TryParse(inputNumber, out number) || number < 1 || number > 3)
                {
                    Console.WriteLine("Некорректный ввод, попробуйте снова:");
                    inputNumber = Console.ReadLine();
                }
                Console.WriteLine("Введите текст для нового файла (если не хотите вводть текст, просто нажмите Enter):");
                string text = Console.ReadLine();
                switch (number)
                {
                    case 1:
                        File.WriteAllText(Path.Combine(path, fileName), text, Encoding.Unicode);
                        break;
                    case 2:
                        File.WriteAllText(Path.Combine(path, fileName), text, Encoding.UTF32);
                        break;
                    case 3:
                        File.WriteAllText(Path.Combine(path, fileName), text, Encoding.ASCII);
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"В ходе программы было вызвано исключение. Его текст:\n  {e.Message}");
                return;
            }
            Console.WriteLine($"\n\nФайл {Path.Combine(path, fileName)} создан успешно!\n");
        }

        /// <summary>
        /// Конкатенирует содержимое нескольких файлов и выводит результат в консоль.
        /// </summary>
        static void ConcatenateFiles(string path)
        {
            try
            {
                var text = new StringBuilder();
                do
                {
                    Console.WriteLine("Введите имя файла:");
                    string fileName = Console.ReadLine();
                    while (!File.Exists(Path.Combine(path, fileName)))
                    {
                        Console.WriteLine("Такого файла не существует, попробуйте снова:");
                        path = Console.ReadLine();
                    }
                    var sr = new StreamReader(Path.Combine(path, fileName), false);
                    text.Append(sr.ReadToEnd());
                    Console.WriteLine("Чтобы использовать еще один файл, нажмите Enter, иначе - любую другую клавишу");
                } while (Console.ReadKey().Key == ConsoleKey.Enter);
                Console.WriteLine(text);
            }
            catch (Exception e)
            {
                Console.WriteLine($"В ходе программы было вызвано исключение. Его текст:\n  {e.Message}");
            }
            Console.WriteLine("\n\nКонкатенация файлов совершена успешно!\n");
        }

        /// <summary>
        /// Выводит все файлы по заданной маске в выбранной директории.
        /// </summary>
        /// <param name="path">Путь к директории</param>
        static void OutputMaskedFilesInThisDirectory(string path)
        {
            try
            {
                Console.WriteLine("Справка: Маска файла – это представление имени и расширения файла общими символами. Пример маски: *a?.txt");
                Console.WriteLine("Введите маску файла(маска должна содержать как минимум 1 символ):");
                string mask = Console.ReadLine();
                while(mask.Length == 0)
                {
                    Console.WriteLine("Маска должна содержать как минимум 1 символ, попробуйте снова:");
                    mask = Console.ReadLine();
                }
                string[] files = Directory.GetFiles(path, mask, SearchOption.TopDirectoryOnly);
                Console.WriteLine($"Количество найденных файлов - {files.Length}");
                foreach (string file in files)
                {
                    Console.WriteLine(file);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"В ходе программы было вызвано исключение. Его текст:\n  {e.Message}");
                return;
            }
            Console.WriteLine("\n\nСписок файлов выведен успешно!\n");
        }

        /// <summary>
        /// Выводит все файлы по заданной маске в выбранной директории и ее поддиректориях.
        /// </summary>
        /// <param name="path"></param>
        static void OutputMaskedFilesInAllDirectories(string path)
        {
            try
            {
                Console.WriteLine("Справка: Маска файла – это представление имени и расширения файла общими символами. Пример маски: *a?.txt");
                Console.WriteLine("Введите маску файла(маска должна содержать как минимум 1 символ):");
                string mask = Console.ReadLine();
                while (mask.Length == 0)
                {
                    Console.WriteLine("Маска должна содержать как минимум 1 символ, попробуйте снова:");
                    mask = Console.ReadLine();
                }
                string[] files = Directory.GetFiles(path, mask, SearchOption.AllDirectories);
                Console.WriteLine($"Количество найденных файлов - {files.Length}");
                foreach (string file in files)
                {
                    Console.WriteLine(file);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"В ходе программы было вызвано исключение. Его текст:\n  {e.Message}");
                return;
            }
            Console.WriteLine("\n\nСписок файлов выведен успешно!\n");
        }

        /// <summary>
        /// Сообщает о готовности диска и директории.
        /// </summary>
        /// <param name="driveName">Имя диска</param>
        /// <param name="directoryName">Имя директории</param>
        /// <returns>Информация о готовности диска и директории</returns>
        static string CheckPath(string driveName, string directoryName)
        {
            if (driveName == null)
            {
                return "Путь не выбран";
            }
            else
            {
                if (directoryName == null)
                {
                    return $"Вы работаете на диске {driveName}";
                }
                else
                {
                    return $"Вы работаете в каталоге {Path.Combine(driveName, directoryName)}";
                }
            }
        }

        /// <summary>
        /// Копирует все файлы по заданной маске из данной директории и ее поддиректорий
        /// в другую поддиректорию
        /// </summary>
        /// <param name="path"></param>
        static void CopyMaskedFiles(string path)
        {
            try
            {
                Console.WriteLine("Справка: Маска файла – это представление имени и расширения файла общими символами. Пример маски: *a?.txt");
                Console.WriteLine("Введите маску файла(маска должна содержать как минимум 1 символ):");
                string mask = Console.ReadLine();
                while (mask.Length == 0)
                {
                    Console.WriteLine("Маска должна содержать как минимум 1 символ, попробуйте снова:");
                    mask = Console.ReadLine();
                }
                string[] files = Directory.GetFiles(path, mask, SearchOption.AllDirectories);
                Console.WriteLine("Введите абсолютный путь директории для копирования(если ее не существует, она создастся автоматически):");
                string newDirectory = Console.ReadLine();
                if (!Directory.Exists(newDirectory))
                {
                    Directory.CreateDirectory(newDirectory);
                }
                foreach (string file in files)
                {
                    if (File.Exists(Path.Combine(newDirectory, Path.GetFileName(file))))
                    {
                        Console.WriteLine($"В указанном уже есть файл под названием {Path.GetFileName(file)}.");
                        Console.WriteLine("Если хотите заменить его, нажмите Enter");
                        Console.WriteLine("Если хотите оставить старый файл без изменений, нажмите любую другую клавишу");
                        if (Console.ReadKey().Key == ConsoleKey.Enter)
                        {
                            File.Copy(file, Path.Combine(newDirectory, Path.GetFileName(file)), true);
                        }
                    }
                    else
                    {
                        File.Copy(file, Path.Combine(newDirectory, Path.GetFileName(file)));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"В ходе программы было вызвано исключение. Его текст:\n  {e.Message}");
                return;
            }
            Console.WriteLine("\n\nКопирование прошло успешно!\n");
        }

        /// <summary>
        /// Основное меню программы.
        /// </summary>
        /// <param name="number">Номер операции</param>
        /// <param name="mainDrive">Имя рабочего диска</param>
        /// <param name="mainDirectory">Имя рабочей директории</param>
        static void StartWorking(int number, ref string mainDrive, ref string mainDirectory)
        {
            Console.WriteLine(CheckPath(mainDrive, mainDirectory));
            try
            {
                switch (number)
                {
                    case 1:
                        mainDrive = ShowDrives().ToString();
                        mainDirectory = null;
                        break;
                    case 2:
                        if (mainDrive != null)
                        {
                            mainDirectory = ChooseDirectory(mainDrive);
                        }
                        else
                        {
                            Console.WriteLine("Сначала следует ввести диск");
                        }
                        break;
                    case 3:
                        ShowListOfFiles(Path.Combine(mainDrive, mainDirectory));
                        break;
                    case 4:
                        OutputFileText(Path.Combine(mainDrive, mainDirectory));
                        break;
                    case 5:
                        OutputFileInChosenEncoding(Path.Combine(mainDrive, mainDirectory));
                        break;
                    case 6:
                        CopyFile(Path.Combine(mainDrive, mainDirectory));
                        break;
                    case 7:
                        MoveFile(Path.Combine(mainDrive, mainDirectory));
                        break;
                    case 8:
                        DeleteFile(Path.Combine(mainDrive, mainDirectory));
                        break;
                    case 9:
                        CreateFile(Path.Combine(mainDrive, mainDirectory));
                        break;
                    case 10:
                        CreateFileInChosenEncoding(Path.Combine(mainDrive, mainDirectory));
                        break;
                    case 11:
                        ConcatenateFiles(Path.Combine(mainDrive, mainDirectory));
                        break;
                    case 12:
                        OutputMaskedFilesInThisDirectory(Path.Combine(mainDrive, mainDirectory));
                        break;
                    case 13:
                        OutputMaskedFilesInAllDirectories(Path.Combine(mainDrive, mainDirectory));
                        break;
                    case 14:
                        CopyMaskedFiles(Path.Combine(mainDrive, mainDirectory));
                        break;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Сначала следует ввести имя диска и директории\n");
            }
        }

        /// <summary>
        /// Точка входа.
        /// </summary>
        static void Main()
        {
            string mainDrive = null;
            string mainDirectory = null;
            do
            {
                Console.Clear();
                ShowMenu();
                Console.Write("Введите номер действия: ");
                string inputNumber = Console.ReadLine();
                int number;
                while (!int.TryParse(inputNumber, out number) || number > 15 || number < 1)
                {
                    Console.WriteLine("Некорректный ввод");
                    inputNumber = Console.ReadLine();
                }
                Console.Clear();
                StartWorking(number, ref mainDrive, ref mainDirectory);
                Console.WriteLine("Чтобы выйти, нажмите ESC.");
                Console.WriteLine("Чтобы продолжить, нажмите любую другую клавишу.");
            } while (Console.ReadKey().Key != ConsoleKey.Escape);
        }
    }
}
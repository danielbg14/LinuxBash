using System.Diagnostics;

namespace LinuxBash
{
    internal static class Commands
    {
        public static void Help()
        {
            Console.WriteLine("LinuxBash - Available commands:");
            Console.WriteLine();

            Console.WriteLine("Basic:");
            Console.WriteLine("  help        Show this help message");
            Console.WriteLine("  echo        Print a message");
            Console.WriteLine("  clear       Clear the screen");
            Console.WriteLine("  exit        Exit the shell");
            Console.WriteLine("  version     Show the shell version");
            Console.WriteLine("  history     Show command history");

            Console.WriteLine();
            Console.WriteLine("File & Directory:");
            Console.WriteLine("  ls          List files and directories");
            Console.WriteLine("  pwd         Print current working directory");
            Console.WriteLine("  cd <dir>    Change directory");
            Console.WriteLine("  mkdir <d>   Create directory");
            Console.WriteLine("  touch <f>   Create file");
            Console.WriteLine("  rm <f>      Remove file");
            Console.WriteLine("  rm -r <d>   Remove directory recursively");

            Console.WriteLine();
            Console.WriteLine("File Operations:");
            Console.WriteLine("  cat <f>     Display file contents");
            Console.WriteLine("  cp <s> <t>  Copy file");
            Console.WriteLine("  mv <s> <t>  Move/Rename file");

            Console.WriteLine();
            Console.WriteLine("System:");
            Console.WriteLine("  whoami      Show current user");
            Console.WriteLine("  date        Show current date and time");

            Console.WriteLine();
        }
        public static void Clear()
        {
            Console.Clear();
        }
        public static void Echo(string[] args)
        {
            if (args.Length > 0)
                Console.WriteLine(string.Join(' ', args));
        }
        public static void Pwd()
        {
            Console.WriteLine(Directory.GetCurrentDirectory());
        }
        public static void Ls(string[] args)
        {
            try
            {
                string path = args.Length > 0 ? args[0] : ".";

                if (!Directory.Exists(path))
                {
                    Console.WriteLine($"ls: cannot access '{path}': No such directory");
                    return;
                }

                string[] files = Directory.GetFiles(path);
                string[] dirs = Directory.GetDirectories(path);

                foreach (var d in dirs)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(Path.GetFileName(d));
                    Console.ResetColor();
                }
                foreach (var f in files)
                {
                    Console.WriteLine(Path.GetFileName(f));

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error listing directory: {ex.Message}");
            }
        }
        public static void Cd(string[] args)
        {
            try
            {
                string path;

                if (args.Length == 0)
                {
                    path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                }
                else
                {
                    path = args[0];
                    if (path.StartsWith("~"))
                    {
                        string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                        path = Path.Combine(home, path.Substring(1).TrimStart('\\', '/'));
                    }
                }

                string target = Path.GetFullPath(path, Directory.GetCurrentDirectory());

                if (!Directory.Exists(target))
                {
                    Console.WriteLine($"-bash: cd: {path}: No such file or directory");
                    return;
                }

                Directory.SetCurrentDirectory(target);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cannot change directory: {ex.Message}");
            }
        }
        public static void WhoAmI()
        {
            Console.WriteLine(Environment.UserName);
        }
        public static void Mkdir(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: mkdir <directory>");
                return;
            }

            try
            {
                Directory.CreateDirectory(args[0]);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cannot create directory: {ex.Message}");
            }
        }
        public static void Touch(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: touch <filename>");
                return;
            }

            foreach (var fileName in args)
            {
                try
                {
                    if (!File.Exists(fileName))
                        File.Create(fileName).Dispose();
                    else
                        File.SetLastWriteTime(fileName, DateTime.Now);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Cannot create file '{fileName}': {ex.Message}");
                }
            }
        }
        public static void Rm(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: rm <file> or rm -r <directory>");
                return;
            }

            if (args[0] == "-r")
            {
                foreach (var dir in args.Skip(1))
                {
                    try
                    {
                        if (Directory.Exists(dir))
                            Directory.Delete(dir, true);
                        else
                            Console.WriteLine($"rm: cannot remove '{dir}': No such file or directory");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Cannot remove directory '{dir}': {ex.Message}");
                    }
                }
            }
            else
            {
                foreach (var file in args)
                {
                    try
                    {
                        if (File.Exists(file))
                            File.Delete(file);
                        else
                            Console.WriteLine($"rm: cannot remove '{file}': No such file or directory");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Cannot remove file '{file}': {ex.Message}");
                    }
                }
            }
        }
        public static void Cat(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: cat <filename>");
                return;
            }

            foreach (var file in args)
            {
                try
                {
                    if (File.Exists(file))
                        Console.Write(File.ReadAllText(file));
                    else
                        Console.WriteLine($"cat: {file}: No such file or directory");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Cannot cat file '{file}': {ex.Message}");
                }
            }
        }
        public static void Cp(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: cp <source> <target>");
                return;
            }

            string source = args[0];
            string target = args[1];

            try
            {
                if (!File.Exists(source))
                {
                    Console.WriteLine($"cp: {source}: No such file");
                    return;
                }

                File.Copy(source, target, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cannot copy file '{source}': {ex.Message}");
            }
        }
        public static void Mv(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: mv <source> <target>");
                return;
            }

            string source = args[0];
            string target = args[1];

            try
            {
                if (!File.Exists(source))
                {
                    Console.WriteLine($"mv: cannot stat '{source}': No such file or directory");
                    return;
                }
                File.Move(source, target, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cannot move file '{source}': {ex.Message}");
            }
        }
        public static void Date()
        {
            Console.WriteLine(DateTime.Now);
        }
        public static void Version()
        {
            Console.WriteLine("LinuxBash v0.2");
            Console.WriteLine("Written in C#");
        }
        public static void History(List<string> history)
        {
            for (int i = 0; i < history.Count; i++)
            {
                Console.WriteLine($"  {i + 1}  {history[i]}");
            }
        }
    }
}
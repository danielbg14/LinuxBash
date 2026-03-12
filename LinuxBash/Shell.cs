namespace LinuxBash
{
    internal class Shell
    {
        private string host;
        private bool isRunning = true;
        private List<string> history = new List<string>();
        public Shell()
        {
            host = $"{Environment.UserName}@{Environment.UserDomainName}";
        }
        private static string[] ParseArguments(string input)
        {
            var matches = System.Text.RegularExpressions.Regex.Matches(
                input,
                @"[\""].+?[\""]|[^ ]+"
            );

            return matches
                .Select(m => m.Value.Trim('"'))
                .ToArray();
        }
        public void Run()
        {
            Console.Clear();
            Directory.SetCurrentDirectory(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));

            while (isRunning)
            {
                PrintPrompt();

                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                    continue;

                history.Add(input);

                string[] parts = ParseArguments(input);

                string command = parts[0];
                string[] args = parts.Skip(1).ToArray();

                ExecuteCommand(command, args);
            }
        }

        private void ExecuteCommand(string command, string[] args)
        {
            switch (command)
            {
                case "help":
                    Commands.Help();
                    break;

                case "clear":
                    Commands.Clear();
                    break;

                case "exit":
                    isRunning = false;
                    break;

                case "echo":
                    Commands.Echo(args);
                    break;

                case "pwd":
                    Commands.Pwd();
                    break;

                case "ls":
                    Commands.Ls(args);
                    break;

                case "cd":
                    Commands.Cd(args);
                    break;

                case "whoami":
                    Commands.WhoAmI();
                    break;

                case "mkdir":
                    Commands.Mkdir(args);
                    break;

                case "touch":
                    Commands.Touch(args);
                    break;

                case "rm":
                    Commands.Rm(args);
                    break;

                case "cat":
                    Commands.Cat(args);
                    break;

                case "cp":
                    Commands.Cp(args);
                    break;

                case "mv":
                    Commands.Mv(args);
                    break;

                case "date":
                    Commands.Date();
                    break;

                case "version":
                    Commands.Version();
                    break;

                case "history":
                    Commands.History(history);
                    break;

                default:
                    Console.WriteLine($"Command not found: {command}");
                    break;
            }
        }

        private void PrintPrompt()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(host);

            Console.ResetColor();
            Console.Write(":");

            Console.ForegroundColor = ConsoleColor.Blue;


            string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string current = Directory.GetCurrentDirectory();

            if (current == home)
            {
                Console.Write("~");
            }
            else if (current.StartsWith(home))
            {
                Console.Write("~" + current.Substring(home.Length));
            }
            else
            {
                Console.Write(current);
            }

            Console.ResetColor();
            Console.Write("$ ");
        }
    }
}
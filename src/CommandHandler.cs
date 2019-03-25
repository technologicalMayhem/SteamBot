using System;
using System.Collections.Generic;
using System.Reflection;
using SteamKit2;
using System.Linq;
using technologicalMayhem.SteamBot;
using System.Threading;
using SteamKit2.Internal;
using System.IO;

namespace technologicalMayhem.SteamBot
{
    public static class CommandHandler
    {
        public static event CommandReceivedHandler CommandReceived = delegate { };
        public delegate void CommandReceivedHandler(OnCommandReceivedEventArgs e);
        public static event EventHandler<OnCommandExecutedEventArgs> CommandExecuted = delegate { };

        public static List<CommandInfo> Commands;
        private static Queue<Task> tasks;
        private static List<IChatCommand> ExecutingTasks;
        private static bool isShuttingDown;
        private static Thread thread;

        public static void Start()
        {
            ExecutingTasks = new List<IChatCommand>();
            tasks = new Queue<Task>();
            thread = new Thread(() => Run());
            Commands = new List<CommandInfo>();
            //Load all availible Commands
            foreach (var c in AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => x.GetInterfaces().Contains(typeof(IChatCommand)) && !x.GetInterfaces().Contains(typeof(ISubCommand)) && x.IsClass && !x.IsAbstract))
            {
                var cmnd = (IChatCommand)Activator.CreateInstance(c);
                var acti = new string[] { cmnd.Properties.Command };
                acti.Concat(cmnd.Properties.CommandAlias);
                Commands.Add(new CommandInfo() { type = c, activators = acti });
            }
            var table = new ConsoleTables.ConsoleTable("Source", "Class Name", "Command");
            Commands.ForEach(x => table.AddRow(Path.GetFileName(x.GetType().Assembly.Location), x.GetType().Name, String.Join(',', x.activators)));
            Console.WriteLine("Loaded Commands:");
            table.Write(ConsoleTables.Format.MarkDown);
            thread.Start();
        }

        static void Run()
        {
            while (!isShuttingDown || tasks.Count > 0)
            {
                if (tasks.Count > 0)
                {
                    var task = tasks.Dequeue();
                    var type = Commands.FirstOrDefault(x => x.activators.Contains(task.parameters[0])).type;
                    IChatCommand command;
                    try
                    {
                        command = (IChatCommand)Activator.CreateInstance(type);
                        CommandReceived(new OnCommandReceivedEventArgs()
                        {
                            steamID = task.steamID,
                            parameters = task.parameters,
                            command = command
                        });
                    }
                    catch (System.ArgumentNullException)
                    {
                        command = (IChatCommand)Activator.CreateInstance(typeof(CommandNotFound));
                    }
                    ExecutingTasks.Add(command);
                    command.Execute(task.steamID, task.parameters.Skip(1).ToArray());
                    OnCommandExecuted(new OnCommandExecutedEventArgs()
                    {
                        steamID = task.steamID,
                        parameters = task.parameters,
                        command = command
                    });
                }
            }
        }

        public static void Stop()
        {

        }

        public static void AddTask(SteamID id, string message)
        {
            tasks.Enqueue(new Task(id, message.Split(' ')));
        }

        private class Task
        {
            public Task(SteamID steamID, string[] parameters)
            {
                this.steamID = steamID;
                this.parameters = parameters;
            }

            public SteamID steamID { get; }
            public string[] parameters { get; set; }
        }

        static void OnCommandExecuted(OnCommandExecutedEventArgs e)
        {
            EventHandler<OnCommandExecutedEventArgs> handler = CommandExecuted;
            if (handler != null)
            {
                handler(null, e);
            }
        }

        public class OnCommandReceivedEventArgs : EventArgs
        {
            public SteamID steamID;
            public string[] parameters;
            public IChatCommand command;
        }

        public class OnCommandExecutedEventArgs : EventArgs
        {
            public SteamID steamID;
            public string[] parameters;
            public IChatCommand command;
        }
    }

    public struct CommandInfo
    {
        public string[] activators;
        public Type type;
    }

    public class ChatCommandProperties
    {
        public bool ShowInHelp = true;
        public string Command = "";
        public string CommandSyntax = "";
        public string CommandDescription = "";
        public string[] CommandAlias = new string[] { };
        public IChatCommand[] Subcommands = null;
    }

    public class CommandNotFound : IChatCommand
    {
        public ChatCommandProperties Properties { get; } = new ChatCommandProperties()
        {
            ShowInHelp = false,
        };

        public void Execute(SteamID steamid, string[] parameters)
        {
            ChatManager.AddTask(steamid, ChatMessages.UnknownCommandMsg);
        }
    }

    public class Help : IChatCommand
    {
        public ChatCommandProperties Properties { get; } = new ChatCommandProperties()
        {
            ShowInHelp = true,
            Command = "help",
            CommandSyntax = "",
            CommandDescription = "Zeigt eine Liste aller Befehle oder gibt Informationen über einen bestimmten Befehl",
            CommandAlias = { }
        };

        public void Execute(SteamID steamid, string[] parameters)
        {
            var commands = new List<string>();
            CommandHandler.Commands.ForEach(x =>
            {
                if (x.activators.First() != null & x.activators.First() != string.Empty)
                {
                    commands.Add(x.activators.First());
                }
            });
            //var help = ChatMessages.HelpBeginning + "\n" + commands.Aggregate((x,y) => x + "\n" + y) + ChatMessages.HelpEnding;
            //ChatManager.AddTask(steamid, help);
            var help = new string[] { ChatMessages.HelpBeginning, "/code " + commands.Aggregate((x, y) => x + "\n" + y), ChatMessages.HelpEnding };
            ChatManager.AddTask(steamid, help);
        }
    }
}
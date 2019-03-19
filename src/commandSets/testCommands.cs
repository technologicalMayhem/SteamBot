// using System;
// using System.Collections.Generic;
// using System.Linq;
// using SteamKit2;

// namespace technologicalMayhem.SteamBot
// {
//     public class exampleCommand : commandGroupBase
//     {
//         public ChatCommandProperties Properties { get; } = new ChatCommandProperties()
//         {
//             Command = "test"
//         };
//         Dictionary<string, Type> subCommands;

//         public exampleCommand()
//         {
//             subCommands = new Dictionary<string, Type>();
//             subCommands.Add("first", typeof(firstSub));
//             subCommands.Add("second", typeof(secondSub));
//         }

//         void Help()
//         {
//             Console.WriteLine("Executing Help");
//         }

//         public class firstSub : IChatCommand, ISubCommand
//         {
//             public ChatCommandProperties Properties { get; } = new ChatCommandProperties()
//             {
//                 Command = "first"
//             };

//             public void Execute(SteamID steamid, string[] parameters)
//             {
//                 Console.WriteLine("Executed firstSub");
//                 ChatManager.AddTask(steamid, "First test command.");
//             }
//         }

//         public class secondSub : IChatCommand, ISubCommand
//         {
//             public ChatCommandProperties Properties { get; } = new ChatCommandProperties()
//             {
//                 Command = "second"
//             };

//             public void Execute(SteamID steamid, string[] parameters)
//             {
//                 Console.WriteLine("Executed secondSub");
//                 ChatManager.AddTask(steamid, "Second test command.");
//             }
//         }
//     }
// }
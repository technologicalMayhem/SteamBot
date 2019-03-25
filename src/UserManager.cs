using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;
using SteamKit2;
using System.Linq;

namespace technologicalMayhem.SteamBot
{
    public static class UserManager
    {
        static List<User> users;

        public static void Start()
        {
            try
            {
                JsonConvert.DeserializeObject<List<User>>(File.ReadAllText("users.json"));
            }
            catch (Exception ex)
            {
                if (ex is FileNotFoundException || ex is JsonException)
                {
                    if (ex is JsonException)
                    {
                        Console.WriteLine("Something is wrong with the user.json file. Delete it? (y/n):  n");
                        bool delete = false;
                        while (true)
                        {
                            var key = Console.ReadKey();
                            Console.CursorLeft -= 1;
                            switch (key.Key)
                            {
                                case(ConsoleKey.Y):
                                delete = true;
                                Console.Write("y");
                                break;

                                case(ConsoleKey.N):
                                delete = false;
                                Console.Write("n");
                                break;

                                case(ConsoleKey.Enter):
                                //TODO: Recreate File;
                                throw new NotImplementedException();
                                
                                default:
                                Console.CursorLeft += 1;
                                break;
                            }
                            
                        }
                    }
                    users = new List<User>();
                }
            }
        }

        public static void Stop()
        {
            File.WriteAllText("users.json", JsonConvert.SerializeObject(users));
        }

        public static Dictionary<string, string> GetUserData(SteamID steamID)
        {
            try
            {
                return users.First(x => x.steamID == steamID).data;
            }
            catch (System.InvalidOperationException)
            {
                var newuser = new User(steamID);
                users.Add(newuser);
                return newuser.data;
            }
        } 
    }

    public class User
    {
        public SteamID steamID {get;}
        public Dictionary<string, string> data {get;set;}

        public User(SteamID steamID)
        {
            this.steamID = steamID;
            data = new Dictionary<string, string>();
        }
    }
}
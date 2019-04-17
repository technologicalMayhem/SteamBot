using System;
using System.Collections.Generic;
using System.Threading;
using SteamKit2;
using SteamKit2.Internal;

namespace technologicalMayhem.SteamBot
{
    public static class ChatManager
    {
        private static Queue<ChatTask> Tasks;
        public static SteamClient steamClient;
        private static CallbackManager manager;
        private static SteamUser steamUser;
        private static SteamFriends steamFriends;
        private static SteamConfiguration configuration;
        private static Thread thread;
        private static bool isShuttingDown;

        class ChatTask
        {
            public SteamID steamID { get; }
            public string[] message { get; set; }

            public ChatTask(SteamID steamID, string message)
            {
                this.steamID = steamID;
                this.message = new string[] { message };
            }

            public ChatTask(SteamID steamID, string[] message)
            {
                this.steamID = steamID;
                this.message = message;
            }
        }

        public static void Start()
        {
            Tasks = new Queue<ChatTask>();
            thread = new Thread(() => Run());
            steamClient = new SteamClient();
            manager = new CallbackManager(steamClient);
            steamUser = steamClient.GetHandler<SteamUser>();
            steamFriends = steamClient.GetHandler<SteamFriends>();


            //Register all the Callbacks we are interested in
            manager.Subscribe<SteamClient.ConnectedCallback>(OnConnected);
            manager.Subscribe<SteamClient.DisconnectedCallback>(OnDisconnected);
            manager.Subscribe<SteamUser.LoggedOnCallback>(OnLoggedOn);
            manager.Subscribe<SteamUser.LoggedOffCallback>(OnLoggedOff);
            manager.Subscribe<SteamUser.AccountInfoCallback>(OnAccountInfo);
            manager.Subscribe<SteamFriends.FriendAddedCallback>(OnFriendAdded);
            manager.Subscribe<SteamFriends.FriendMsgCallback>(OnFriendMsg);
            manager.Subscribe<SteamFriends.FriendsListCallback>(OnFriendList);

            steamClient.Connect();
            thread.Start();
        }

        static void Run()
        {
            Thread receive = new Thread(() =>
            {
                while (!isShuttingDown)
                {
                    manager.RunWaitCallbacks(TimeSpan.FromSeconds(1));
                }
            });
            Thread send = new Thread(() =>
            {
                while (!isShuttingDown)
                {
                    if (Tasks.Count > 0)
                    {
                        var task = Tasks.Dequeue();
                        foreach (var message in task.message)
                        {
                            steamFriends.SendChatMessage(task.steamID, EChatEntryType.InviteGame, message);
                            steamFriends.SendChatMessage(task.steamID, EChatEntryType.ChatMsg, message);
                            Thread.Sleep(TimeSpan.FromMilliseconds(100));
                        }
                    }
                    else
                    {
                        Thread.Sleep(TimeSpan.FromSeconds(0.5));
                    }
                }
            });
            receive.Start();
            send.Start();
            while (receive.IsAlive || send.IsAlive)
            {

            }
        }

        public static void Stop()
        {

        }

        public static void AddTask(SteamID steamID, string message)
        {
            Tasks.Enqueue(new ChatTask(steamID, message));
        }

        public static void AddTask(SteamID steamID, string[] message)
        {
            Tasks.Enqueue(new ChatTask(steamID, message));
        }

        static void OnFriendList(SteamFriends.FriendsListCallback callback)
        {
            foreach (var friend in callback.FriendList)
            {
                if (friend.Relationship == EFriendRelationship.RequestRecipient)
                {
                    steamFriends.AddFriend(friend.SteamID);
                }
            }
        }

        static void OnAccountInfo(SteamUser.AccountInfoCallback callback)
        {
            // before being able to interact with friends, you must wait for the account info callback
            // this callback is posted shortly after a successful logon
            // at this point, we can go online on friends, so lets do that
            steamFriends.SetPersonaState(EPersonaState.Online);
        }

        static void OnFriendMsg(SteamFriends.FriendMsgCallback callback)
        {
            if (callback.EntryType == EChatEntryType.ChatMsg)
            {
                CommandHandler.AddTask(callback.Sender, callback.Message);
            }
        }

        static void OnFriendAdded(SteamFriends.FriendAddedCallback callback)
        {
            if (steamFriends.GetFriendRelationship(callback.SteamID) == EFriendRelationship.Friend)
            {
                steamFriends.SendChatMessage(callback.SteamID, EChatEntryType.ChatMsg, ChatMessages.WelcomeMsg);
            }
        }

        static void OnConnected(SteamClient.ConnectedCallback callback)
        {
            Console.WriteLine($"Logging in as {Globals.Username}...");
            steamUser.LogOn(new SteamUser.LogOnDetails
            {
                Username = Globals.Username,
                Password = Globals.Password,
            });
        }

        static void OnDisconnected(SteamClient.DisconnectedCallback callback)
        {
            Globals.ReadyForShutdown[1] = true;
        }

        static void OnLoggedOn(SteamUser.LoggedOnCallback callback)
        {
            if (callback.Result != EResult.OK)
            {
                if (callback.Result == EResult.AccountLogonDenied)
                {
                    Console.WriteLine("Unable to logon to Steam: This account is SteamGuard protected.");
                    return;
                }

                Console.WriteLine("Unable to logon to Steam: {0} / {1}", callback.Result, callback.ExtendedResult);
                return;
            }
            else
            {
                Console.WriteLine("Sucessfully logged in.");
            }
        }

        static void OnLoggedOff(SteamUser.LoggedOffCallback callback)
        {
        }
    }
}
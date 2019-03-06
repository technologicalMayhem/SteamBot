using System;
using System.Net;
using Newtonsoft.Json;
using SteamAuth;

namespace technologicalMayhem.SteamBot
{
    public static class WebRequest
    {
        /// <summary>
        /// Adds or removes the provided workshop item from the collection.
        /// </summary>
        /// <param name="workshopid">The WorkshopID of the workshopitem.</param>
        /// <param name="remove">If true the workshopitem will be removed instead of being addded.</param>
        public static void ModifyCollection(ulong workshopid, bool remove)
        {
            var login = Globals.SteamLogin;
            var data = "sessionID=" + login.Session.SessionID + "&publishedfileid=" + workshopid + "&collections%5B1377816355%5D%5B" + (remove ? "remove" : "add")  + "%5D=true&collections%5B1377816355%5D%5Btitle%5D=Die+Unausstehlichen+-+TTT";
            CookieContainer cookies = new CookieContainer();
            login.Session.AddCookies(cookies);
            var response = SteamWeb.Request("https://steamcommunity.com/sharedfiles/ajaxaddtocollections", "POST", data, cookies);
            Console.WriteLine(response);
        }
    }
}
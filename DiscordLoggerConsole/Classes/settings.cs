using DSharpPlus.Entities;
using System.Collections.Generic;

namespace DiscordLoggerConsole.Classes
{
    public class settings
    {
        // SETTINGS YOU CAN CHANGE
        public static string configname = "config";

        // SETTINGS YOU SHOULDNT CHANGE
        public List<ulong> admins { get; set; }

        public ulong owner { get; set; }
        public List<ulong> guildstostalk { get; set; }
        public List<string> prefixes { get; set; }
        public string token { get; set; }
        public string databasename { get; set; }
        public string status { get; set; }
        public ActivityType statusmode { get; set; }
        public UserStatus userstatus { get; set; }

        public static settings GetDefault()
        {
            return new settings
            {
                admins = new List<ulong>(),
                owner = 0,
                guildstostalk = new List<ulong>(),
                prefixes = new List<string>() { "!" },
                databasename = "main",
                token = string.Empty,
                status = string.Empty,
                statusmode = ActivityType.Playing,
                userstatus = UserStatus.Online
            };
        }
    }
}
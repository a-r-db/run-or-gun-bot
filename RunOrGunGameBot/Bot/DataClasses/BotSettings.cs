using System.Collections.Generic;

namespace RunOrGunGameBot.Bot.DataClasses
{
    public class BotSettings
    {
        public static string Token { get; set; }
        public static bool Enabled = false;
        public static List<BotLog> BotLogList = new();
        public static List<BotLogError> BotLogErrorList = new();
    }
}
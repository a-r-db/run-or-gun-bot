using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RunOrGunGameBot.Bot.DataClasses
{
    public class BotLog
    {
        private static int logID = 0;
        public int LogID { get; set; }
        public DateTime DateTime { get; set; }
        public string Tag { get; set; }
        public string Message { get; set; }

        public BotLog(string tag, string message)
        {
            this.LogID = logID;
            this.DateTime = DateTime.Now;
            this.Tag = tag;
            this.Message = message;
            logID++;
        }
    }
}

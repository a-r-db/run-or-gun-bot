using System;


namespace RunOrGunGameBot.Bot.DataClasses
{
    public struct BotLogError
    {
        private static int logID = 0;
        public int LogID { get; set; }
        public DateTime DateTime { get; set; }
        private readonly Exception exception;
        public Exception Exception
        {
            get
            {
                return this.exception;
            }
        }
        public string Message
        {
            get
            {
                return this.Exception.Message;
            }
        }
        public string BasicTrace {
            get
            {
                return this.Exception.StackTrace;
            }
        }

        public BotLogError(Exception ex)
        {
            this.LogID = logID;
            this.DateTime = DateTime.Now;
            this.exception = ex;
            logID++;
        }
    }
}

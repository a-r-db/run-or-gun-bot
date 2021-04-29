using System;


namespace RunOrGunGameBot.Bot.DataClasses
{
    public struct BotLogError
    {
        private static int logID = 0;
        public int LogID { get; set; }
        public DateTime DateTime { get; set; }
        public Exception Exception { get; set; }
        public string Message => this.Exception.Message;
        public string StackTrace => this.Exception.StackTrace;

        public BotLogError(Exception ex)
        {
            this.LogID = logID;
            this.DateTime = DateTime.Now;
            this.Exception = ex;
            logID++;
        }

        public static bool operator ==(BotLogError a, Object obj)
        {
            if (obj == null) return false;
            return a.GetHashCode() == ((BotLogError)obj).GetHashCode();
        }

        public static bool operator !=(BotLogError a, Object obj)
        {
            if (obj == null) return false;
            return a.GetHashCode() != ((BotLogError)obj).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this.GetHashCode() == ((BotLogError)obj).GetHashCode();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}

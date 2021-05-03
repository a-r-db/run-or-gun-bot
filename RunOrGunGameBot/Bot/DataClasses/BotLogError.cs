using System;
using System.Text;
using System.Xml.Serialization;

namespace RunOrGunGameBot.Bot.DataClasses
{
    [Serializable]
    public class BotLogError
    {
        private static int logID = 0;
        public int LogID { get; set; }
        public DateTime DateTime { get; set; }
        [XmlIgnore]
        public Exception Exception { get; set; }
        public string ExceptionAsString { get; set; }
        public string Message => this.Exception.Message;
        public string StackTrace => this.Exception.StackTrace;


        public BotLogError()
        {

        }

        public BotLogError(Exception ex)
        {
            this.LogID = logID;
            this.DateTime = DateTime.Now;
            this.Exception = ex;
            this.ExceptionAsString = FlattenException(ex);
            logID++;
        }

        public static string FlattenException(Exception exception)
        {
            var stringBuilder = new StringBuilder();

            while (exception != null)
            {
                stringBuilder.AppendLine(exception.Message);
                stringBuilder.AppendLine(exception.StackTrace);

                exception = exception.InnerException;
            }

            return stringBuilder.ToString();
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

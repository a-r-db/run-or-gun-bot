using System.Threading.Tasks;
using DSharpPlus;
using Microsoft.Extensions.Logging;
using RunOrGunGameBot.Bot.DataClasses;

namespace RunOrGunGameBot.Bot
{
    public class Bot : IBot
    {
        /// <summary>
        /// Log Tag
        /// </summary>
        private const string Tag = "Bot";
        /// <inheritdoc/>
        public DiscordClient Client { get; }

        // this instantiates the container class and the client
        public Bot(string token)
        {
            // create config from the supplied token
            var cfg = new DiscordConfiguration
            {
                Token = token,                   // use the supplied token
                TokenType = TokenType.Bot,       // log in as a bot

                AutoReconnect = true,            // reconnect automatically
                MinimumLogLevel = LogLevel.Debug,
            };

            // initialize the client
            this.Client = new DiscordClient(cfg);
        }

        /// <inheritdoc/>
        public Task StartAsync()
        {
            BotSettings.BotLogList.Add(new BotLog(Tag, "Bot Connecting.."));
            return this.Client.ConnectAsync();
        }

        /// <inheritdoc/>        
        public Task StopAsync()
        {
            BotSettings.BotLogList.Add(new BotLog(Tag, "Bot Stopped."));
            return this.Client.DisconnectAsync();
        }
    }
}
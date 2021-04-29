using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.Entities;
using Microsoft.Extensions.Logging;
using RunOrGunGameBot.Bot.DataClasses;
using RunOrGunGameBot.Bot.Game;

namespace RunOrGunGameBot.Bot
{
    public class Bot : IBot
    {
        /// <summary>
        /// Log Tags
        /// </summary>
        private const string Tag = "Bot";
        public readonly EventId BotEventId = new EventId(123, "RoG-Bot");
        /// <inheritdoc/>
        public DiscordClient Client { get; }
        public CommandsNextExtension Commands { get; set; }

        // this instantiates the container class and the client
        public Bot(string token)
        {
            // create config from the supplied token
            var cfg = new DiscordConfiguration
            {
                Token = token,                   // use the supplied token
                TokenType = TokenType.Bot,       // log in as a bot
                Intents = DiscordIntents.AllUnprivileged
                | DiscordIntents.GuildMembers,
                AutoReconnect = true,            // reconnect automatically
                MinimumLogLevel = LogLevel.Debug,
            };

            // initialize the client
            this.Client = new DiscordClient(cfg);

            var ccfg = new CommandsNextConfiguration
            {
                // let's use the string prefix defined in config.json
                StringPrefixes = new[] { "!rog" },

                // enable responding in direct messages
                EnableDms = true,

                // enable mentioning the bot as a command prefix
                EnableMentionPrefix = true
            };

            // and hook them up
            this.Commands = this.Client.UseCommandsNext(ccfg);

            // let's hook some command events, so we know what's 
            // going on
            this.Commands.CommandExecuted += this.Commands_CommandExecuted;
            this.Commands.CommandErrored += this.Commands_CommandErrored;

            // up next, let's register our commands
            this.Commands.RegisterCommands<Commands>();

            // set up our custom help formatter
            this.Commands.SetHelpFormatter<SimpleHelpFormatter>();
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

        private Task Commands_CommandExecuted(CommandsNextExtension sender, CommandExecutionEventArgs e)
        {
            // let's log the name of the command and user
            e.Context.Client.Logger.LogInformation(BotEventId, $"{e.Context.User.Username} successfully executed '{e.Command.QualifiedName}'");

            // since this method is not async, let's return
            // a completed task, so that no additional work
            // is done
            return Task.CompletedTask;
        }

        private async Task Commands_CommandErrored(CommandsNextExtension sender, CommandErrorEventArgs e)
        {
            // let's log the error details
            e.Context.Client.Logger.LogError(BotEventId, $"{e.Context.User.Username} tried executing '{e.Command?.QualifiedName ?? "<unknown command>"}' but it errored: {e.Exception.GetType()}: {e.Exception.Message ?? "<no message>"}", DateTime.Now);

            // let's check if the error is a result of lack
            // of required permissions
            if (e.Exception is ChecksFailedException ex)
            {
                // yes, the user lacks required permissions, 
                // let them know

                var emoji = DiscordEmoji.FromName(e.Context.Client, ":no_entry:");

                // let's wrap the response into an embed
                var embed = new DiscordEmbedBuilder
                {
                    Title = "Access denied",
                    Description = $"{emoji} You do not have the permissions required to execute this command.",
                    Color = new DiscordColor(0xFF0000) // red
                };
                await e.Context.RespondAsync(embed);
            }
        }
    }
}
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;
using RunOrGunGameBot.Bot.DataClasses;

namespace RunOrGunGameBot.Bot.DataServices
{
    public class BotService : IBotService
    {
        /// <inheritdoc/>
        public Task BotThread { get; set; }
        /// <inheritdoc/>
        public Bot Bot { get; set; }
        /// <inheritdoc/>
        public CancellationTokenSource TokenSource { get; set; }

        /// <inheritdoc/>
        public void BotStateChange()
        {
            
            // check if a bot thread is running
            if (this.BotThread == null)
            {
                // start the bot
                // create the bot container
                this.Bot = new Bot(BotSettings.Token);

                // hook all the bot events
                this.Bot.Client.Ready += this.Bot_Ready;
                //this.Bot.Client.GuildAvailable += this.Bot_GuildAvailable;
                //this.Bot.Client.GuildCreated += this.Bot_GuildCreated;
                //this.Bot.Client.GuildUnavailable += this.Bot_GuildUnavailable;
                //this.Bot.Client.GuildDeleted += this.Bot_GuildDeleted;
                //this.Bot.Client.MessageCreated += this.Bot_MessageCreated;
                this.Bot.Client.ClientErrored += this.Bot_ClientErrored;

                // create a cancellation token, this will be used 
                // to cancel the infinite delay task
                this.TokenSource = new CancellationTokenSource();

                // finally, start the thread with the bot
                this.BotThread = Task.Run(this.BotThreadCallback);
                BotSettings.Enabled = true;
            }
            else
            {
                // stop the bot

                // request cancelling the task preventing the 
                // bot from stopping
                // this will effectively stop the bot
                this.TokenSource.Cancel();
                BotSettings.Enabled = false;
            }

            // clear the token text box, we don't need it anymore
            BotSettings.Token = "";
        }
        
        /// <inheritdoc/>
        public async Task BotThreadCallback()
        {
            // this will start the bot
            await this.Bot.StartAsync().ConfigureAwait(false);

            // here we wait indefinitely, or until the wait is
            // cancelled
            try
            {
                // the token will cancel the way once it's 
                // requested
                await Task.Delay(-1, this.TokenSource.Token).ConfigureAwait(false);
            }
            catch { /* ignore the exception; it's expected */ }

            // this will stop the bot
            await this.Bot.StopAsync().ConfigureAwait(false);

            // once the bot is stopped, we can
            // reset the UI state

            // and finally, dispose of our bot stuff
            this.Bot = null;
            this.TokenSource = null;
            this.BotThread = null;
        }

        /// <inheritdoc/>
        public Task Bot_Ready(DiscordClient sender, ReadyEventArgs e)
        {
            // TODO: set the window title to indicate we are connected
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task Bot_ClientErrored(DiscordClient sender, ClientErrorEventArgs e)
        {
            // TODO: show a message box by dispatching it to the Web UI
            return Task.CompletedTask;
        }
    }
}
using DSharpPlus;
using DSharpPlus.EventArgs;
using System.Threading;
using System.Threading.Tasks;

namespace RunOrGunGameBot.Bot
{
    /// <summary>
    /// Interaction logic for Bot Service
    /// </summary>
    public interface IBotService
    {
        /// <summary>
        /// this will hold the thread on which the bot will run
        /// </summary>
        Task BotThread { get; set; }

        /// <summary>
        /// this will hold the bot itself
        /// </summary>
        Bot Bot { get; set; }

        /// <summary>
        /// this will hold a token required to make the bot quit cleanly 
        /// </summary>
        CancellationTokenSource TokenSource { get; set; }

        /// <summary>
        /// this occurs when user presses the start/stop button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BotStateChange();
         
        /// <summary>
        /// this method will be ran on the bot's thread
        /// it will take care of the initialization logic, as
        /// well as actually handling the bot
        /// </summary>
        /// <returns>Task</returns>
        Task BotThreadCallback();

        /// <summary>
        /// this handles the bot's ready event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns>Task</returns>
        Task Bot_Ready(DiscordClient sender, ReadyEventArgs e);

        /// <summary>
        /// called when an unhandled exception occurs in any of the 
        /// event handlers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns>Task</returns>
        Task Bot_ClientErrored(DiscordClient sender, ClientErrorEventArgs e);
    }
}

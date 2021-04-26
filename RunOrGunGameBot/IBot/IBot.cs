using System.Threading.Tasks;
using DSharpPlus;

namespace RunOrGunGameBot.Bot {
    /// <summary>
    /// Bot Class
    /// Simply used as a container to organize the code
    /// Patially seperated from the UI logic
    /// </summary>
    public interface IBot
    {
        /// <summary>
        /// The client instance is initialized with the class.
        /// </summary>
        public DiscordClient Client { get; }

        /// <summary>
        ///  this method logs in and starts the client
        /// </summary>
        /// <returns>Task</returns>
        public Task StartAsync();

        /// <summary>
        /// this method logs out and stops the client
        /// </summary>
        /// <returns>Task</returns>
        public Task StopAsync();
    }
}
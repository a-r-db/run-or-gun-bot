using DSharpPlus.Entities;

namespace RunOrGunGameBot.Bot.DataClasses
{
    public struct BotChannel
    {
        public DiscordChannel Channel { get; }
        public ulong Id => this.Channel.Id;
        public string Name => this.Channel.Name;

        public BotChannel(DiscordChannel chn)
        {
            this.Channel = chn;
        }

        public override string ToString()
        {
            return $"#{this.Channel.Name}";
        }
    }
}
using DSharpPlus.Entities;

namespace RunOrGunGameBot.Bot.DataClasses
{
    public struct BotGuild
    {
        public DiscordGuild Guild { get; }
        public ulong Id => this.Guild.Id;
        public string Name => this.Guild.Name;
        public string IconUrl => $"{this.Guild.IconUrl}?size=32";

        public BotGuild(DiscordGuild gld)
        {
            this.Guild = gld;
        }

        public override string ToString()
        {
            return this.Guild.Name;
        }
    }
}
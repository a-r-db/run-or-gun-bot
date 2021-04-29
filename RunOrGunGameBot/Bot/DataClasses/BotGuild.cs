using DSharpPlus.Entities;
using System;

namespace RunOrGunGameBot.Bot.DataClasses
{
    public class BotGuild
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
        public static bool operator ==(BotGuild a, Object obj)
        {
            if (obj == null) return false;
            return a.GetHashCode() == ((BotGuild)obj).GetHashCode();
        }

        public static bool operator !=(BotGuild a, Object obj)
        {
            if (obj == null) return false;
            return a.GetHashCode() != ((BotGuild)obj).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this.GetHashCode() == ((BotGuild)obj).GetHashCode();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
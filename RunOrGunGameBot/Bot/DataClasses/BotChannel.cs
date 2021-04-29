using DSharpPlus.Entities;
using System;

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

        public static bool operator ==(BotChannel a, Object obj)
        {
            if (obj == null) return false;
            return a.GetHashCode() == ((BotChannel)obj).GetHashCode();
        }

        public static bool operator !=(BotChannel a, Object obj)
        {
            if (obj == null) return false;
            return a.GetHashCode() != ((BotChannel)obj).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this.GetHashCode() == ((BotChannel)obj).GetHashCode();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
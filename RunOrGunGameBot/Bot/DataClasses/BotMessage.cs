using DSharpPlus;
using DSharpPlus.Entities;
using System;

namespace RunOrGunGameBot.Bot.DataClasses
{
    public struct BotMessage
    {
        public DiscordMessage Message { get; }
        public ulong Id => this.Message.Id;
        public string AuthorName => this.Message.Author.Username;
        public string AuthorAvatarUrl => this.Message.Author.GetAvatarUrl(ImageFormat.Png, 32);
        public string Content => this.Message.Content;

        public BotMessage(DiscordMessage msg)
        {
            this.Message = msg;
        }

        public override string ToString()
        {
            return this.Message.Content;
        }

        public static bool operator ==(BotMessage a, Object obj)
        {
            if (obj == null) return false;
            return a.GetHashCode() == ((BotMessage)obj).GetHashCode();
        }

        public static bool operator !=(BotMessage a, Object obj)
        {
            if (obj == null) return false;
            return a.GetHashCode() != ((BotMessage)obj).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this.GetHashCode() == ((BotMessage)obj).GetHashCode();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}

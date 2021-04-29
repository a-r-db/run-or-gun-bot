using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using RunOrGunGameBot.Bot.DataClasses;
using System.Collections.Generic;

namespace RunOrGunGameBot.Bot.Game
{
    public class PlayerTile
    {
        public DiscordUser KilledBy;
        public DiscordEmoji DiscordEmoji;
        public PlayerStatus DeathStatus;
        public List<Player> Kills;
        public BoardPosition Position;
        public List<Mine> Mines;
        public bool HasBullet;

        public PlayerTile()
        {
            this.DeathStatus = PlayerStatus.Alive;
            this.KilledBy = null;
            this.DiscordEmoji = DiscordEmoji.FromName(BotSettings.BotService.Bot.Client, ":cowboy:");
            this.HasBullet = true;
        }

        public void ChangeEmoji(DiscordEmoji discordEmoji)
        {
            this.DiscordEmoji = discordEmoji;
        }

        public void Shot()
        {
            this.HasBullet = false;
        }

        public void Reload()
        {
            this.HasBullet = true;
        }
    }
}

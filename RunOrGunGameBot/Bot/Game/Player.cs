using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RunOrGunGameBot.Bot.Game
{
    public class Player
    {
        public DiscordMember DiscordMember;
        public DiscordUser DiscordUser;
        public DiscordDmChannel DiscordDmChannel;
        public PlayerTile PlayerTile;

        public Player(CommandContext cmdCtx)
        {
            Task<DiscordDmChannel> task = cmdCtx.Member.CreateDmChannelAsync();
            task.Wait();
            this.DiscordDmChannel = task.Result;
            this.DiscordUser = cmdCtx.Message.Author;
            this.DiscordMember = cmdCtx.Member;
            this.PlayerTile = new PlayerTile();
        }
    }
}

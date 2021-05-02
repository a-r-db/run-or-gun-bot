using DSharpPlus.Entities;
using RunOrGunGameBot.Bot.DataClasses;
using System.Collections.Generic;

namespace RunOrGunGameBot.Bot.Game
{
    public class Mine
    {
        public MineStatus MineStatus;
        public BoardPosition Position;
        public Player Owner;
        public DiscordEmoji DiscordEmoji;
        public int ReleasedRound;
        public Player PlayerGot = null;
        public List<Player> VisibleBy = new();

        public object GamesConsts { get; private set; }

        public Mine(Player owner)
        {
            this.Owner = owner;
            this.DiscordEmoji = DiscordEmoji.FromName(BotSettings.BotService.Bot.Client, GameConst.MINE_EMOJI);
            this.MineStatus = MineStatus.Stored;
        }

        public void Released(BoardPosition position, int round)
        {
            this.Position = position;
            this.ReleasedRound = round;
            this.MineStatus = MineStatus.Released;
            this.VisibleBy.Add(this.Owner);
        }

        public void Detonated(Player player)
        {
            this.MineStatus = MineStatus.Detonated;
            this.PlayerGot = player;
            this.VisibleBy = new();
        }
        public void Detonated()
        {
            this.MineStatus = MineStatus.Detonated;
            this.PlayerGot = null;
            this.VisibleBy = new();
        }

        public void CheckRound(int round)
        {
            if (this.MineStatus == MineStatus.Released)
            {
                if (round - ReleasedRound >= GameConst.MAX_ROUNDS_PER_MINE)
                {
                    this.MineStatus = MineStatus.Decommissioned;
                    this.VisibleBy = new();
                }
            }
        }
        
        public void Scanned(Player player)
        {
            this.VisibleBy.Add(player);
        }
    }
}

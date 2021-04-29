using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;

namespace RunOrGunGameBot.Bot.Game
{
    public class Game
    {
        private static ulong id = 0;
        public ulong Id;
        public bool Started = false;
        public GameMode GameMode;
        public DiscordGuild DiscordGuild;
        public Player Owner;
        public List<ulong> AllowedPlayers = new();
        public List<Player> Players = new();
        public Board Board;

        public Game(CommandContext cmdCtx) {
            this.Owner = new Player(cmdCtx);
            this.DiscordGuild = cmdCtx.Guild;
            this.Players.Add(this.Owner);
            this.Id = id;
            this.GameMode = GameMode.Default;
            id++;
        }

        public void AddPlayer(CommandContext cmdCtx)
        {
            if (this.GameMode.HasFlag(GameMode.Unlocked) || this.AllowedPlayers.Contains(cmdCtx.Member.Id))
            {
                if (Players.Count + 2 > GameConst.MAX_HEIGHT)
                {
                    throw new GamePlayerException("Can't start game too many players!" +
                        " The game board will be too big!", cmdCtx.Message.Author);
                }
                else
                {
                    if (Players.Count + 1 > GameConst.MAX_PLAYERS)
                        throw new GamePlayerException("Too many Players sorry.\n" +
                            "You can start a new game, or wait for someone to quit.");
                    Players.Add(new Player(cmdCtx));
                    if (Started)
                    {
                        Board.ResizeBoard(Players.Count);
                    }
                }
            }
            else
            {
                throw new GamePlayerException("This Game is Locked!\n" +
                    "Please ask the owner of the game to play.\n" +
                    " Use `!rog join` to see available games.", cmdCtx.Message.Author);
            }
        }

        public string PermitPlayer(ulong permittedMemberId)
        {
            AllowedPlayers.Add(permittedMemberId);
            return "\nUser is now allowed to play.";
        }

        public override string ToString()
        {
            return $"{Owner.DiscordUser.Username}" +
                $" {string.Join(',',Players)}";
        }
    }
}

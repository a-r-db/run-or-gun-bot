using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using RunOrGunGameBot.Bot.DataClasses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RunOrGunGameBot.Bot.Game
{
    public class GuildQueue : BotGuild
    {
        public List<Game> Games;

        public GuildQueue(DiscordGuild gld) : base(gld)
        {
            this.Games = new();
        }

        public string New(CommandContext cmdCtx)
        {
            if (this.Games.Where(g => g.Owner.DiscordUser.Id == cmdCtx.Message.Author.Id).Any())
                return $"Continue or end your current game in {cmdCtx.Guild.Name}";
            if (this.Games.Where(g => g.Players.Where(p => p.DiscordUser.Id == cmdCtx.Message.Author.Id).Any()).Any())
                return $"Continue or leave your current game in {cmdCtx.Guild.Name}";
            this.Games.Add(new Game(cmdCtx));
            return "Success!";
        }

        public string End(CommandContext cmdCtx)
        {
            if (!this.Games.Where(g => g.Owner.DiscordUser.Id == cmdCtx.Message.Author.Id).Any())
                return $"No game in {cmdCtx.Guild.Name}";
            Game game = this.Games.Where(g => g.Owner.DiscordUser.Id == cmdCtx.Message.Author.Id).First();
            game.Players.ForEach(p =>
            {
                p.DiscordDmChannel.SendMessageAsync($"{game.Id} Ended on Server {cmdCtx.Guild.Name}");
            });
            this.Games.Remove(game);
            return "Success!";
        }

        public string Join(CommandContext cmdCtx)
        {
            return string.Join("\n", this.Games.Select(g => g.ToString()));
        }

        public string Join(CommandContext cmdCtx, ulong gameNumber)
        {
            try
            {
                if (this.Games.Where(g => g.Players.Where(p => p.DiscordMember.Id == cmdCtx.Member.Id).Any()).Any())
                    throw new GamePlayerException("Please quit or leave your current game to start a new one!");
                this.Games.Where(g => g.Id == gameNumber).First().AddPlayer(cmdCtx);
                return "Success!";
            }
            catch (GamePlayerException e)
            {
                return e.Message;
            }
            catch (Exception e)
            {
                BotSettings.BotLogErrorList.Add(new BotLogError(e));
                return "Game not found";
            }
        }
    }
}

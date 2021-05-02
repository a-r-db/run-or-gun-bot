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
        public DiscordChannel discordCategory;
        public DiscordChannel discordChannel;

        public GuildQueue(DiscordGuild gld) : base(gld)
        {
            this.Games = new();
        }

        public string New(CommandContext cmdCtx)
        {
            if (this.Games.Where(g => !g.GameOver && g.Owner.DiscordUser.Id == cmdCtx.Message.Author.Id).Any())
                return $"Continue or `end` your current game in {cmdCtx.Guild.Name}";
            if (this.Games.Where(g => !g.GameOver && g.Players
                .Where(p => p.DiscordUser.Id == cmdCtx.Message.Author.Id).Any()).Any())
                return $"Continue or `leave` your current game in {cmdCtx.Guild.Name}";
            this.Games.Add(new Game(cmdCtx));
            return "Success!";
        }

        public string End(CommandContext cmdCtx)
        {
            if (!this.Games.Where(g => !g.GameOver && g.Owner.DiscordUser.Id == cmdCtx.Message.Author.Id).Any())
                return $"No game in {cmdCtx.Guild.Name}";
            this.Games.Where(g => !g.GameOver && g.Owner.DiscordUser.Id == cmdCtx.Message.Author.Id).First().StopGame();
            this.Games.Where(g => !g.GameOver && g.Owner.DiscordUser.Id == cmdCtx.Message.Author.Id)
                .First().Players.ForEach(p =>
            {
                p.DiscordDmChannel.SendMessageAsync($"{this.Games.Where(g => g.Owner.DiscordUser.Id == cmdCtx.Message.Author.Id).First().Id} Ended on Server {cmdCtx.Guild.Name}");
            });
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
                if (this.Games.Where(g => !g.GameOver && g.Owner.DiscordMember.Id == cmdCtx.Member.Id).Any())
                    throw new GamePlayerException($"Please `end` your current game in " +
                        $"{this.Games.Where(g => !g.GameOver && g.Owner.DiscordMember.Id == cmdCtx.Member.Id).First().DiscordGuild.Name} to start a new one!");
                if (this.Games.Where(g => !g.GameOver && g.Players.Where(p => p.DiscordMember.Id == cmdCtx.Member.Id).Any()).Any())
                    throw new GamePlayerException($"Please `leave` your current game in " +
                        $"{this.Games.Where(g => !g.GameOver && g.Players.Where(p => p.DiscordMember.Id == cmdCtx.Member.Id).Any()).First().DiscordGuild.Name} to start a new one!");
                this.Games.Where(g => !g.GameOver && g.Id == gameNumber).First().AddPlayer(cmdCtx);
                return "Success!";
            }
            catch (GamePlayerException e)
            {
                return e.Message;
            }
            catch (Exception e)
            {
                BotSettings.BotLogErrorList.Add(new BotLogError(e));
                return "Game not found!";
            }
        }

        public string Leave(CommandContext cmdCtx)
        {
            try
            {
                if (!this.Games.Where(g => !g.GameOver && g.Players.Where(p => p.DiscordMember.Id == cmdCtx.Member.Id).Any()).Any())
                    return "Your Game could not be found!";
                else
                    this.Games.Where(g => !g.GameOver && g.Players.Where(p => p.DiscordMember.Id == cmdCtx.Member.Id).Any()).First().RemovePlayer(cmdCtx);
                return "Success!";
            }
            catch (GamePlayerException e)
            {
                return e.Message;
            }
            catch (Exception e)
            {
                BotSettings.BotLogErrorList.Add(new BotLogError(e));
                return "Game not found!";
            }
        }

        public string Remove(DiscordUser discordUser)
        {
            try
            {
                if (!this.Games.Where(g => !g.GameOver && g.Players.Where(p => p.DiscordMember.Id == discordUser.Id).Any()).Any())
                    return "Player not found in a game!";
                else
                    this.Games.Where(g => !g.GameOver && g.Players.Where(p => p.DiscordMember.Id == discordUser.Id).Any()).First().RemovePlayer(discordUser);
                return "Success!";
            }
            catch (GamePlayerException e)
            {
                return e.Message;
            }
            catch (Exception e)
            {
                BotSettings.BotLogErrorList.Add(new BotLogError(e));
                return "Game not found!";
            }
        }
    }
}

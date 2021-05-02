using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using RunOrGunGameBot.Bot.DataClasses;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RunOrGunGameBot.Bot.Game
{
    public class CommandsPlayer : BaseCommandModule
    {
        [Command("airstrike")]
        [Description("```Airstrike any square\n" +
        ". airstrike A 3\n" +
        ". a B 2 \n```")]
        [Aliases("a")]
        public async Task AirStrike(CommandContext ctx, string row, int column)
        {
            string output = "";
            if (ctx.Guild != null)
                output += "Run the `. airstrike` command directly to the bot please.";
            else
            {
                try
                {
                    output += (from g in BotSettings.BotGuilds
                               from b in g.Games
                               from p in b.Players
                               where p.DiscordUser.Id == ctx.Message.Author.Id
                               select b.DoTurn(TurnOption.Airstrike, row + column.ToString(), ctx.Message.Author.Id)).First();
                }
                catch (Exception e)
                {
                    if (e is GameBoardException || e is GamePlayerException)
                    {
                        output += e.Message;
                    }
                    else
                    {
                        BotSettings.BotLogErrorList.Add(new BotLogError(e));
                        output += e.Message;
                    }
                }
            }
            await ctx.RespondAsync(output);
        }

        [Command("call")]
        [Description("```Call to ready your next airstrike!```")]
        [Aliases("ca")]
        public async Task Call(CommandContext ctx)
        {
            string output = "";
            if (ctx.Guild != null)
                output += "Run the `. call` command directly to the bot please.";
            else
            {
                try
                {
                    output += (from g in BotSettings.BotGuilds
                               from b in g.Games
                               from p in b.Players
                               where p.DiscordUser.Id == ctx.Message.Author.Id
                               select b.DoTurn(TurnOption.Call, null, ctx.Message.Author.Id)).First();
                }
                catch (Exception e)
                {
                    if (e is GameBoardException || e is GamePlayerException)
                    {
                        output += e.Message;
                    }
                    else
                    {
                        BotSettings.BotLogErrorList.Add(new BotLogError(e));
                        output += e.Message;
                    }
                }
            }
            await ctx.RespondAsync(output);
        }

        [Command("cloak")]
        [Description("```Cloak yourself for a 1-3 rounds!\n" +
        ". cloak 1\n" +
        ". c 3\n```")]
        [Aliases("c")]
        public async Task Skip(CommandContext ctx, int cloakTime)
        {
            string output = "";
            if (ctx.Guild != null)
                output += "Run the `. cloak` command directly to the bot please.";
            else
            {
                try
                {
                    output += (from g in BotSettings.BotGuilds
                               from b in g.Games
                               from p in b.Players
                               where p.DiscordUser.Id == ctx.Message.Author.Id
                               select b.DoTurn(TurnOption.Cloak, cloakTime.ToString(), ctx.Message.Author.Id)).First();
                }
                catch (Exception e)
                {
                    if (e is GameBoardException || e is GamePlayerException)
                    {
                        output += e.Message;
                    }
                    else
                    {
                        BotSettings.BotLogErrorList.Add(new BotLogError(e));
                        output += e.Message;
                    }
                }
            }
            await ctx.RespondAsync(output);
        }

        [Command("melee")]
        [Description("```Attack one square away\n" +
            "left, right, up, or down\n" +
            "or \n" +
            "l, r, u, d\n" +
            ". melee up\n" +
            ". me w\n```")]
        [Aliases("me")]
        public async Task Melee(CommandContext ctx, string options)
        {
            string output = "";
            if (ctx.Guild != null)
                output += "Run the `. melee` command directly to the bot please.";
            else
            {
                try
                {
                    output += (from g in BotSettings.BotGuilds
                               from b in g.Games
                               from p in b.Players
                               where p.DiscordUser.Id == ctx.Message.Author.Id
                               select b.DoTurn(TurnOption.Melee, options, ctx.Message.Author.Id)).First();
                }
                catch (Exception e)
                {
                    if (e is GameBoardException || e is GamePlayerException)
                    {
                        output += e.Message;
                    }
                    else
                    {
                        BotSettings.BotLogErrorList.Add(new BotLogError(e));
                        output += e.Message;
                    }
                }
            }
            await ctx.RespondAsync(output);
        }

        [Command("mine")]
        [Description("```Put a mine one square away\n" +
            "left, right, up, or down\n" +
            "or \n" +
            "l, r, u, d\n" +
            ". mine up\n" +
            ". m w\n```")]
        [Aliases("m")]
        public async Task Mine(CommandContext ctx, string options)
        {
            string output = "";
            if (ctx.Guild != null)
                output += "Run the `. mine` command directly to the bot please.";
            else
            {
                try
                {
                    output += (from g in BotSettings.BotGuilds
                               from b in g.Games
                               from p in b.Players
                               where p.DiscordUser.Id == ctx.Message.Author.Id
                               select b.DoTurn(TurnOption.Mine, options, ctx.Message.Author.Id)).First();
                }
                catch (Exception e)
                {
                    if (e is GameBoardException || e is GamePlayerException)
                    {
                        output += e.Message;
                    }
                    else
                    {
                        BotSettings.BotLogErrorList.Add(new BotLogError(e));
                        output += e.Message;
                    }
                }
            }
            await ctx.RespondAsync(output);
        }

        [Command("move")]
        [Description("```Move one square away\n" +
            "left, right, up, or down\n" +
            "or \n" +
            "l, r, u, d\n" +
            ". move up\n" +
            ". m w\n```")]
        [Aliases("mv")]
        public async Task Move(CommandContext ctx, string direction)
        {
            string output = "";
            if (ctx.Guild != null)
                output += "Run the `. move` command directly to the bot please.";
            else
            {
                try
                {
                    output += (from g in BotSettings.BotGuilds
                               from b in g.Games
                               from p in b.Players
                               where p.DiscordUser.Id == ctx.Message.Author.Id
                               select b.DoTurn(TurnOption.Move, direction, ctx.Message.Author.Id)).First();
                }
                catch (Exception e)
                {
                    if (e is GameBoardException || e is GamePlayerException)
                    {
                        output += e.Message;
                    }
                    else
                    {
                        BotSettings.BotLogErrorList.Add(new BotLogError(e));
                        output += e.Message;
                    }
                }
            }
            await ctx.RespondAsync(output);
        }

        [Command("reload")]
        [Description("```Reload your gun!```")]
        [Aliases("r")]
        public async Task Reload(CommandContext ctx)
        {
            string output = "";
            if (ctx.Guild != null)
                output += "Run the `. reload` command directly to the bot please.";
            else
            {
                try
                {
                    output += (from g in BotSettings.BotGuilds
                               from b in g.Games
                               from p in b.Players
                               where p.DiscordUser.Id == ctx.Message.Author.Id
                               select b.DoTurn(TurnOption.Reload, null, ctx.Message.Author.Id)).First();
                }
                catch (Exception e)
                {
                    if (e is GameBoardException || e is GamePlayerException)
                    {
                        output += e.Message;
                    }
                    else
                    {
                        BotSettings.BotLogErrorList.Add(new BotLogError(e));
                        output += e.Message;
                    }
                }
            }
            await ctx.RespondAsync(output);
        }

        [Command("scan")]
        [Description("```Scan all squares 1 square away\n" +
        ". scan\n" +
        ". sc\n```")]
        [Aliases("sc")]
        public async Task Scan(CommandContext ctx)
        {
            string output = "";
            if (ctx.Guild != null)
                output += "Run the `. scan` command directly to the bot please.";
            else
            {
                try
                {
                    output += (from g in BotSettings.BotGuilds
                               from b in g.Games
                               from p in b.Players
                               where p.DiscordUser.Id == ctx.Message.Author.Id
                               select b.DoTurn(TurnOption.Scan, null, ctx.Message.Author.Id)).First();
                }
                catch (Exception e)
                {
                    if (e is GameBoardException || e is GamePlayerException)
                    {
                        output += e.Message;
                    }
                    else
                    {
                        BotSettings.BotLogErrorList.Add(new BotLogError(e));
                        output += e.Message;
                    }
                }
            }
            await ctx.RespondAsync(output);
        }

        [Command("shoot")]
        [Description("```Shoot in a straight line!" +
        "left, right, up, or down\n" +
        "or \n" +
        "l, r, u, d\n" +
        ". shoot up\n" +
        ". sh d\n```")]
        [Aliases("sh")]
        public async Task Skip(CommandContext ctx, string direction)
        {
            string output = "";
            if (ctx.Guild != null)
                output += "Run the `. skip` command directly to the bot please.";
            else
            {
                try
                {
                    output += (from g in BotSettings.BotGuilds
                               from b in g.Games
                               from p in b.Players
                               where p.DiscordUser.Id == ctx.Message.Author.Id
                               select b.DoTurn(TurnOption.Shoot, direction, ctx.Message.Author.Id)).First();
                }
                catch (Exception e)
                {
                    if (e is GameBoardException || e is GamePlayerException)
                    {
                        output += e.Message;
                    }
                    else
                    {
                        BotSettings.BotLogErrorList.Add(new BotLogError(e));
                        output += e.Message;
                    }
                }
            }
            await ctx.RespondAsync(output);
        }

        [Command("skip")]
        [Description("```Skip your turn!```")]
        [Aliases("sk")]
        public async Task Skip(CommandContext ctx)
        {
            string output = "";
            if (ctx.Guild != null)
                output += "Run the `. skip` command directly to the bot please.";
            else
            {
                try
                {
                    output += (from g in BotSettings.BotGuilds
                               from b in g.Games
                               from p in b.Players
                               where p.DiscordUser.Id == ctx.Message.Author.Id
                               select b.DoTurn(TurnOption.Skip, null, ctx.Message.Author.Id)).First();
                }
                catch (Exception e)
                {
                    if (e is GameBoardException || e is GamePlayerException)
                    {
                        output += e.Message;
                    }
                    else
                    {
                        BotSettings.BotLogErrorList.Add(new BotLogError(e));
                        output += e.Message;
                    }
                }
            }
            await ctx.RespondAsync(output);
        }

        [Command("teleport")]
        [Description("```Teleport `n` square(s) away\n" +
        "left, right, up, or down\n" +
        "or \n" +
        "l, r, u, d\n" +
        ". teleport up 3\n" +
        ". t d 1\n```")]
        [Aliases("t")]
        public async Task Teleport(CommandContext ctx, string direction, int distance)
        {
            string output = "";
            if (ctx.Guild != null)
                output += "Run the `. teleport` command directly to the bot please.";
            else
            {
                try
                {
                    output += (from g in BotSettings.BotGuilds
                               from b in g.Games
                               from p in b.Players
                               where p.DiscordUser.Id == ctx.Message.Author.Id
                               select b.DoTurn(TurnOption.Teleport, direction + distance, ctx.Message.Author.Id)).First();
                }
                catch (Exception e)
                {
                    if (e is GameBoardException || e is GamePlayerException)
                    {
                        output += e.Message;
                    }
                    else
                    {
                        BotSettings.BotLogErrorList.Add(new BotLogError(e));
                        output += e.Message;
                    }
                }
            }
            await ctx.RespondAsync(output);
        }
    }
}

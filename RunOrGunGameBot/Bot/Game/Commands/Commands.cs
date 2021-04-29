using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using RunOrGunGameBot.Bot.DataClasses;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RunOrGunGameBot.Bot.Game
{
    public class Commands : BaseCommandModule
    {
        [Command("new")]
        [Description("Begins a new game and optionally invites one player")]
        public async Task New(CommandContext ctx)
        {
            string output = "";
            if (ctx.Message.Author == null || BotSettings.BotGuilds.Where(g => g.Guild.Id == ctx.Guild.Id &&
                g.Games.Where(a => a.Owner.DiscordMember.Id == ctx.Message.Author.Id).Any()).Any())
                output += "You alraedy started a game in the server";
            else if (BotSettings.BotGuilds.Where(g => g.Guild.Id == ctx.Guild.Id &&
                g.Games.Where(a => a.Players.Where(p => p.DiscordMember.Id == ctx.Message.Author.Id).Any()).Any()).Any())
                output += "You are already playing a game in the current server";
            else if (ctx.Guild == null)
                output += "Run the `new` command inside of a server text channel.";
            else
            {
                output += $"Server: {ctx.Guild.Name}\n";
                try
                {
                    output += BotSettings.BotGuilds.Where(g => g.Guild.Id == ctx.Guild.Id).First().New(ctx);
                }
                catch (Exception e)
                {
                    BotSettings.BotLogErrorList.Add(new BotLogError(e));
                    output += "Failed ";
                }
            }
            await ctx.RespondAsync(output);
        }

        [Command("new")]
        [Description("Begins a new game and optionally invites one player")]
        public async Task New(CommandContext ctx, DiscordMember discordMember)
        {
            string output = "";
            if (ctx.Guild == null)
                output += "Run the `new @invitee_username` command inside of a server text channel."; 
            else if (discordMember.Id == ctx.Member.Id)
                output += $"Please invite member of {ctx.Guild.Name}";
            else
            {
                output += $"Server: {ctx.Guild.Name}\n";
                try
                {
                    output += BotSettings.BotGuilds.Where(g => g.Guild.Id == ctx.Guild.Id).First().New(ctx);
                    output += BotSettings.BotGuilds.Where(g => g.Guild.Id == ctx.Guild.Id).First()
                        .Games.Where(g => g.Owner.DiscordMember.Id == ctx.Member.Id).First().PermitPlayer(discordMember.Id);
                    await ctx.Guild.Members.Values.Where(m => m.Id == discordMember.Id).First()
                        .SendMessageAsync($"{ctx.Member.Username} invites you to a game of RunOrGun!\n" +
                        $" Type `!rog join " +
                        $"{BotSettings.BotGuilds.Where(g => g.Guild.Id == ctx.Guild.Id).First().Games.Where(g => g.Owner.DiscordMember.Id == ctx.Member.Id).First().Id}`");
                }
                catch (Exception e)
                {
                    BotSettings.BotLogErrorList.Add(new BotLogError(e));
                    output += $"User {discordMember.Username} not found!";
                }
            }                   
            await ctx.RespondAsync(output);
        }

        [Command("end")]
        [Description("Ends a game.")]
        public async Task End(CommandContext ctx)
        {
            string output = "";
            if (ctx.Guild == null)
                output += "Run the `end` command inside of a server please.";
            else
            {
                try
                {
                    output += $"Server: {ctx.Guild.Name}\n";
                    output += BotSettings.BotGuilds.Where(g => g.Guild.Id == ctx.Guild.Id).First().End(ctx);
                }
                catch (Exception e)
                {
                    BotSettings.BotLogErrorList.Add(new BotLogError(e));
                    output += "No active games found.";
                }
            }
            await ctx.RespondAsync(output);
        }

        [Command("join")]
        [Description("Join or list available games by number.")]
        [Aliases("list")]
        public async Task Join(CommandContext ctx)
        {
            string output = "";
            if (ctx.Guild == null)
                output += "Run the `join` command inside of a server please.";
            else
            {
                output += $"Server: {ctx.Guild.Name}\n";
                try
                {
                    output += BotSettings.BotGuilds.Where(g => g.Guild.Id == ctx.Guild.Id).First().Join(ctx);
                }
                catch (Exception e)
                {
                    BotSettings.BotLogErrorList.Add(new BotLogError(e));
                    output += $"No active games found.";
                }
            }
            await ctx.RespondAsync(output);
        }

        [Command("join")]
        [Description("Join or list available games by number.")]
        public async Task Join(CommandContext ctx, ulong gameNumber)
        {
            string output = "";
            if (ctx.Guild == null)
                output += "Run the `join` command inside of a server please.";
            else
            {
                output += $"Server: {ctx.Guild.Name}\n";
                try
                {
                    output += BotSettings.BotGuilds.Where(g => g.Guild.Id == ctx.Guild.Id).First().Join(ctx, gameNumber);
                }
                catch (Exception e)
                {
                    BotSettings.BotLogErrorList.Add(new BotLogError(e));
                    output += $"No active games found.";
                }
            }
            await ctx.RespondAsync(output);
        }

        [Command("start")]
        [Description("Start the new game you created!\n" +
            "You can also input mode options comma separated:\n" +
            " For example: guided,emoji,unlocked,mapunlocked\n" +
            "guided => Players choose their start position.\n" +
            "emoji => Players choose their emoji.\n" +
            "unlocked => Random players can join the game.\n" +
            "mapunlocked => The map will autoresize based on player count.\n" +
            "customorder => Player's turns will be in a custom order.\n" +
            "The default is to randomize emojis and positions\n")]
        public async Task Start(CommandContext ctx)
        {
            string output = "";
            try
            {
                if (!BotSettings.BotGuilds.Where(g => g.Guild.Id == ctx.Guild.Id).First()
                    .Games.Where(a => a.Owner.DiscordUser.Id == ctx.Message.Author.Id).Any())
                    throw new GameException("No active games found.\n" +
                        " Use `!rog new (@invitee_username)` to begin one.");
            }
            catch (Exception exception)
            {
                output = exception.Message;
            }
            await ctx.RespondAsync(output);
        }

        [Command("start")]
        [Description("Start the new game you created!\n" +
            "You can also input mode options comma separated:\n" +
            " For example: guided,emoji,unlocked,mapunlocked\n" +
            "guided => Players choose their start position.\n" +
            "emoji => Players choose their emoji.\n" +
            "unlocked => Random players can join the game.\n" +
            "mapunlocked => The map will autoresize based on player count.\n" +
            "customorder => Player's turns will be in a custom order.\n" +
            "The default is to randomize emojis and positions\n")]
        public async Task Start(CommandContext ctx, string modeOptions)
        {
            string output = "This function is not yet implemented.";
            await ctx.RespondAsync(output);
        }
    }
}

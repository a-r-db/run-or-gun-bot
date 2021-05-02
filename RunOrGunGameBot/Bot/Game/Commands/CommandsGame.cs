using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using RunOrGunGameBot.Bot.DataClasses;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RunOrGunGameBot.Bot.Game
{
    [Group("game")]
    [Description("Commands to get the game started!")]
    [Aliases("g")]
    public class CommandsGame : BaseCommandModule
    {
        [Command("new")]
        [Description("Begins a new game and optionally invites one player")]
        [Aliases("n")]
        public async Task New(CommandContext ctx)
        {
            string output = "";
            if (ctx.Guild == null)
                output += "Run the `new` command inside of a server `Bot` category and `runorgunbot` channel.";
            else if (ctx.Channel.Id != BotSettings.BotGuilds.Where(g => g.Guild.Id == ctx.Guild.Id).First().discordChannel.Id)
                output += "Run the `new` command inside of a server `Bot` category and `runorgunbot` channel.";
            else if (ctx.Message.Author == null || BotSettings.BotGuilds.Where(g => g.Guild.Id == ctx.Guild.Id &&
                g.Games.Where(a => !a.GameOver && a.Owner.DiscordMember.Id == ctx.Message.Author.Id).Any()).Any())
                output += "You already started a game in the server" +
                    "Continue your game or type `. g end` to end the current game.";
            else if (BotSettings.BotGuilds.Where(g => g.Guild.Id == ctx.Guild.Id &&
                g.Games.Where(a => !a.GameOver && a.Players.Where(p => p.DiscordMember.Id == ctx.Message.Author.Id).Any()).Any()).Any())
                output += "You are already playing a game in the current server\n" +
                    "Continue your game or type `. g leave` to leave the current game.";
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
                    output += e.Message;
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
            else if (ctx.Channel.Id != BotSettings.BotGuilds.Where(g => g.Guild.Id == ctx.Guild.Id).First().discordChannel.Id)
                output += "Run the `new` command inside of a server `Bot` category and `runorgunbot` channel.";
            else if (discordMember.Id == ctx.Member.Id)
                output += $"Please invite member of {ctx.Guild.Name} not yourself!";
            else if (ctx.Message.Author == null || BotSettings.BotGuilds.Where(g => g.Guild.Id == ctx.Guild.Id &&
                g.Games.Where(a => !a.GameOver && a.Owner.DiscordMember.Id == ctx.Message.Author.Id).Any()).Any())
                output += "You already started a game in the server" +
                    "Continue your game or type `. g end` to end the current game.";
            else if (BotSettings.BotGuilds.Where(g => g.Guild.Id == ctx.Guild.Id &&
                g.Games.Where(a => !a.GameOver && a.Players.Where(p => p.DiscordMember.Id == ctx.Message.Author.Id).Any()).Any()).Any())
                output += "You are already playing a game in the current server\n" +
                    "Continue your game or type `. g leave` to leave the current game.";
            else
            {
                output += $"Server: {ctx.Guild.Name}\n";
                try
                {
                    output += BotSettings.BotGuilds.Where(g => g.Guild.Id == ctx.Guild.Id).First().New(ctx);
                    output += BotSettings.BotGuilds.Where(g => g.Guild.Id == ctx.Guild.Id).First()
                        .Games.Where(a => !a.GameOver && a.Owner.DiscordMember.Id == ctx.Member.Id).First().PermitPlayer(discordMember.Id);
                    await ctx.Guild.Members.Values.Where(m => m.Id == discordMember.Id).First()
                        .SendMessageAsync($"{ctx.Member.Username} invites you to a game of RunOrGun!\n" +
                        $" Type `. game join " +
                        $"{BotSettings.BotGuilds.Where(g => g.Guild.Id == ctx.Guild.Id).First().Games.Where(a => !a.GameOver && a.Owner.DiscordMember.Id == ctx.Member.Id).First().Id}`" +
                        $" in {ctx.Guild.Name} #{BotSettings.BotGuilds.Where(g => g.Guild.Id == ctx.Guild.Id).First().discordChannel.Name}");
                }
                catch (Exception e)
                {
                    BotSettings.BotLogErrorList.Add(new BotLogError(e));
                    output += e.Message;
                }
            }                   
            await ctx.RespondAsync(output);
        }

        [Command("invite")]
        [Description("Invites one player")]
        [Aliases("i")]
        public async Task Invite(CommandContext ctx, DiscordMember discordMember)
        {
            string output = "";
            if (ctx.Guild == null)
                output += "Run the `invite @invitee_username` command inside of a server `Bot` category and `runorgunbot` channel.";
            else if (ctx.Channel.Id != BotSettings.BotGuilds.Where(g => g.Guild.Id == ctx.Guild.Id).First().discordChannel.Id)
                output += "Run the `new` command inside of a server `Bot` category and `runorgunbot` channel.";
            else if (discordMember.Id == ctx.Member.Id)
                output += $"Please invite member of {ctx.Guild.Name} not yourself!";
            else
            {
                output += $"Server: {ctx.Guild.Name}\n";
                try
                {
                    output += BotSettings.BotGuilds.Where(g => g.Guild.Id == ctx.Guild.Id).First()
                        .Games.Where(a => !a.GameOver && a.Owner.DiscordMember.Id == ctx.Member.Id).First().PermitPlayer(discordMember.Id);
                    await ctx.Guild.Members.Values.Where(m => m.Id == discordMember.Id).First()
                        .SendMessageAsync($"{ctx.Member.Username} invites you to a game of RunOrGun!\n" +
                        $" Type `. game join " +
                        $"{BotSettings.BotGuilds.Where(g => g.Guild.Id == ctx.Guild.Id).First().Games.Where(a => !a.GameOver && a.Owner.DiscordMember.Id == ctx.Member.Id).First().Id}`" +
                        $" in {ctx.Guild.Name} #{BotSettings.BotGuilds.Where(g => g.Guild.Id == ctx.Guild.Id).First().discordChannel.Name}");
                }
                catch (Exception e)
                {
                    BotSettings.BotLogErrorList.Add(new BotLogError(e));
                    output += e.Message;
                }
            }
            await ctx.RespondAsync(output);
        }

        [Command("revoke")]
        [Description("Revoke Invite to player (must use `remove` if the player has already joined)")]
        [Aliases("rv")]
        public async Task Revoke(CommandContext ctx, DiscordUser discordUser)
        {
            string output = "";
            if (ctx.Guild == null)
                output += "Run the `revoke @invitee_username` command inside of a server `Bot` category and `runorgunbot` channel.";
            else if (ctx.Channel.Id != BotSettings.BotGuilds.Where(g => g.Guild.Id == ctx.Guild.Id).First().discordChannel.Id)
                output += "Run the `revoke` command inside of a server `Bot` category and `runorgunbot` channel.";
            else if (discordUser.Id == ctx.Member.Id)
                output += $"Please revoke member of {ctx.Guild.Name} not yourself!";
            else
            {
                output += $"Server: {ctx.Guild.Name}\n";
                try
                {
                    output += BotSettings.BotGuilds.Where(g => g.Guild.Id == ctx.Guild.Id).First()
                        .Games.Where(a => !a.GameOver && a.Owner.DiscordMember.Id == ctx.Member.Id).First().RevokePermission(discordUser.Id);
                }
                catch (Exception e)
                {
                    BotSettings.BotLogErrorList.Add(new BotLogError(e));
                    output += e.Message;
                }
            }
            await ctx.RespondAsync(output);
        }

        [Command("end")]
        [Description("Ends a game.")]
        [Aliases("e")]
        public async Task End(CommandContext ctx)
        {
            string output = "";
            if (ctx.Guild == null)
                output += "Run the `end` command inside of a server `Bot` category and `runorgunbot` channel.";
            else if (ctx.Channel.Id != BotSettings.BotGuilds.Where(g => g.Guild.Id == ctx.Guild.Id).First().discordChannel.Id)
                output += "Run the `new` command inside of a server `Bot` category and `runorgunbot` channel.";
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
                    output += e.Message;
                }
            }
            await ctx.RespondAsync(output);
        }

        [Command("stats")]
        [Description("View stats of completed games!")]
        [Aliases("i")]
        public async Task Stats(CommandContext ctx, ulong gameId)
        {
            string output = "";
            if (ctx.Guild == null)
                output += "Run the `invite @invitee_username` command inside of a server `Bot` category and `runorgunbot` channel.";
            else if (ctx.Channel.Id != BotSettings.BotGuilds.Where(g => g.Guild.Id == ctx.Guild.Id).First().discordChannel.Id)
                output += "Run the `new` command inside of a server `Bot` category and `runorgunbot` channel.";
            else
            {
                try
                {
                    output += $"Server: {ctx.Guild.Name}\n";
                    output += BotSettings.BotGuilds.Where(g => g.Guild.Id == ctx.Guild.Id).First()
                        .Games.Where(a => a.Id == gameId).First().Stats();
                }
                catch (Exception e)
                {
                    BotSettings.BotLogErrorList.Add(new BotLogError(e));
                    output += e.Message;
                }
            }
            await ctx.RespondAsync(output);
        }

        [Command("join")]
        [Description("Join or list available games by number.")]
        [Aliases("list", "ls")]
        public async Task Join(CommandContext ctx)
        {
            string output = "";
            if (ctx.Guild == null)
                output += "Run the `join` command inside of a server `Bot` category and `runorgunbot` channel.";
            else if (ctx.Channel.Id != BotSettings.BotGuilds.Where(g => g.Guild.Id == ctx.Guild.Id).First().discordChannel.Id)
                output += "Run the `new` command inside of a server `Bot` category and `runorgunbot` channel.";
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
                    output += e.Message;
                }
            }
            await ctx.RespondAsync(output);
        }

        [Command("join")]
        [Description("Join or list available games by number.")]
        [Aliases("j")]
        public async Task Join(CommandContext ctx, ulong gameNumber)
        {
            string output = "";
            if (ctx.Guild == null)
                output += "Run the `join` command inside of a server `Bot` category and `runorgunbot` channel.";
            else if (ctx.Channel.Id != BotSettings.BotGuilds.Where(g => g.Guild.Id == ctx.Guild.Id).First().discordChannel.Id)
                output += "Run the `new` command inside of a server `Bot` category and `runorgunbot` channel.";
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
                    output += e.Message;
                }
            }
            await ctx.RespondAsync(output);
        }

        [Command("leave")]
        [Description("Leave current game.")]
        [Aliases("lv", "exit","quit")]
        public async Task Leave(CommandContext ctx)
        {
            string output = "";
            if (ctx.Guild == null)
                output += "Run the `leave` command inside of a server `Bot` category and `runorgunbot` channel.";
            else if (ctx.Channel.Id != BotSettings.BotGuilds.Where(g => g.Guild.Id == ctx.Guild.Id).First().discordChannel.Id)
                output += "Run the `new` command inside of a server `Bot` category and `runorgunbot` channel.";
            else
            {
                output += $"Server: {ctx.Guild.Name}\n";
                try
                {
                    output += BotSettings.BotGuilds.Where(g => g.Guild.Id == ctx.Guild.Id).First().Leave(ctx);
                }
                catch (Exception e)
                {
                    BotSettings.BotLogErrorList.Add(new BotLogError(e));
                    output += e.Message; ;
                }
            }
            await ctx.RespondAsync(output);
        }

        [Command("remove")]
        [Description("Remove player from current game.")]
        [Aliases("rm")]
        public async Task Remove(CommandContext ctx, DiscordUser discordUser)
        {
            string output = "";
            if (ctx.Guild == null)
                output += "Run the `join` command inside of a server `Bot` category and `runorgunbot` channel.";
            else if (ctx.Channel.Id != BotSettings.BotGuilds.Where(g => g.Guild.Id == ctx.Guild.Id).First().discordChannel.Id)
                output += "Run the `new` command inside of a server `Bot` category and `runorgunbot` channel.";
            else
            {
                output += $"Server: {ctx.Guild.Name}\n";
                try
                {
                    output += BotSettings.BotGuilds.Where(g => g.Guild.Id == ctx.Guild.Id).First().Remove(discordUser);
                }
                catch (Exception e)
                {
                    BotSettings.BotLogErrorList.Add(new BotLogError(e));
                    output += e.Message;
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
        [Aliases("s")]
        public async Task Start(CommandContext ctx)
        {
            string output = "";
            if (ctx.Guild == null)
                output += "Run the `join` command inside of a server `Bot` category and `runorgunbot` channel.";
            else if (ctx.Channel.Id != BotSettings.BotGuilds.Where(g => g.Guild.Id == ctx.Guild.Id).First().discordChannel.Id)
                output += "Run the `new` command inside of a server `Bot` category and `runorgunbot` channel.";
            else
            {
                try
                {
                    if (!BotSettings.BotGuilds.Where(g => g.Guild.Id == ctx.Guild.Id).First()
                        .Games.Where(a => a.Owner.DiscordUser.Id == ctx.Message.Author.Id).Any())
                        throw new GameException("No active games found.\n" +
                            " Use `. game new (@invitee_username)` to begin one.");
                    BotSettings.BotGuilds.Where(g => g.Guild.Id == ctx.Guild.Id).First()
                        .Games.Where(a => !a.GameOver && a.Owner.DiscordUser.Id == ctx.Message.Author.Id)
                        .First().StartGame();
                    output = "Success!";
                }
                catch (Exception exception)
                {
                    BotSettings.BotLogErrorList.Add(new BotLogError(exception));
                    output = exception.Message;
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
        public async Task Start(CommandContext ctx, string modeOptions)
        {
            string output = "This function is not yet implemented.";
            await ctx.RespondAsync(output);
        }

        /**
         * For debugging purposes
         * 
        [Command("id")]
        [Description("Report users user id and member id if possible.")]
        public async Task Id(CommandContext ctx)
        {
            string output = $"UserId: {ctx.Message.Author.Id} MemberId: { (ctx.Member == null ? "Not Found" : ctx.Member.Id) }";
            await ctx.RespondAsync(output);
        }
        */
    }
}

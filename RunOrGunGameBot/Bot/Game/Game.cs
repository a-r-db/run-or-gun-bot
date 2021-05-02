using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using RunOrGunGameBot.Bot.DataClasses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RunOrGunGameBot.Bot.Game
{
    public class Game
    {
        private static ulong id = 0;
        public ulong Id;
        public GameMode GameMode;
        public DiscordGuild DiscordGuild;
        public Player Owner;
        public GameStatus GameStatus;
        public bool GameOver;
        public List<ulong> AllowedPlayers = new();
        public List<Player> Players = new();
        public List<Player> KilledPlayers = new();
        public Player Victor;
        public List<Mine> Mines = new();
        public int PlayerTurn = 0;
        public int Rounds = 0;
        public bool TurnComplete = false;
        public List<string> TakenEmojis = new();
        public List<TurnOption> TurnOptions = new();
        public List<GameTurn> GameTurns = new();
        public Board Board;

        public Game(CommandContext cmdCtx)
        {
            this.Owner = new Player(cmdCtx, GetUnusedEmoji());
            this.DiscordGuild = cmdCtx.Guild;
            this.Players.Add(this.Owner);
            this.Id = id;
            this.GameMode = GameMode.Default;
            this.GameStatus = GameStatus.Created;
            this.GameOver = false;
            id++;
        }

        public void StartGame()
        {
            // TODO: build Bot to play as artificial intelligence
            if (this.Players.Count < 2)
                throw new GameException("Not Enough players to start the game!\n" +
                    " `. game invite @discord_user`");
            if (this.GameStatus.Equals(GameStatus.Created))
            {
                Random rand = new();
                Board = new Board(this.Players.Count);
                Players = Players.OrderBy(p => rand.Next()).ToList(); // shuffle player order
                Players[PlayerTurn].Notify("Please start your turn!\n" +
                    "Use `. help player` for more information!");
                Players[PlayerTurn].TurnActive = true;
                if (!GameMode.HasFlag(GameMode.Guided))
                {
                    foreach (Player p in Players)
                    {
                        p.Position = Board.GenerateRandomBoardPosition();
                        GameTurns.Add(new GameTurn(DiscordGuild.Id,
                            DiscordGuild.Name, Id, p.DiscordUser.Id,
                            TurnOption.Init, p.Position.ToString() + " " + Board.ToString(), p.DiscordUser.Username));
                    }
                }
                else
                {
                    // TODO: add logic for GameMode.Guided
                }
                this.GameStatus = GameStatus.Started;
                this.RenderAll();
            }
            else
                throw new GameException($"Game already {this.GameStatus}.");
        }

        public void StopGame()
        {
            GameOver = true;
            GameStatus = GameStatus.Stopped;
        }

        public void NextTurn(TurnOption turnOption)
        {
            TurnOptions.Add(turnOption);
            Players[PlayerTurn].Notify("Turn Complete!");
            RenderAll();
            UpdateGame();
            if (Players.Count == 1)
            {
                BotSettings.SaveLogs(BotSettings.BotLogErrorList, "bot_error_log_");
                GameOver = true;
                GameStatus = GameStatus.Completed;
                BotSettings.SaveLogs(GameTurns, "game_turns_" + this.Id + "_");
                Victor = Players[0];
                Players[0].Notify("Game Over! You WON! :slight_smile:");
                foreach (Player k in KilledPlayers)
                {
                    k.Notify("Game Over! You lost... :smiling_face_with_tear:");
                    k.Notify(Board.RenderBoard(GameStatus, Victor, KilledPlayers, Mines));
                }
                Victor.Notify(Board.RenderBoard(GameStatus, Victor, KilledPlayers, Mines));
                Players.Remove(Victor);
                return;
            }
            else if (Players.Count == 0) // if people somehow tie...
            {
                BotSettings.SaveLogs(BotSettings.BotLogErrorList, "bot_error_log_");
                GameOver = true;
                GameStatus = GameStatus.Completed;
                BotSettings.SaveLogs(GameTurns, "game_turns_" + this.Id + "_");
                foreach (Player k in KilledPlayers)
                {
                    k.Notify("Game Over! You all lost... :smiling_face_with_tear:");
                    k.Notify(Board.RenderBoard(GameStatus, Victor, KilledPlayers, Mines));
                }
                return;
            }
            if (Players.Count <= PlayerTurn + 1)
            {
                PlayerTurn = 0;
                UpdateRound();
            }
            else
                PlayerTurn++;
            Players[PlayerTurn].Notify("Please start your turn!\n" +
                "`. help` and `. help command`\n" +
                "for more information!");
        }

        public void UpdateGame()
        {
            foreach (Player p in KilledPlayers)
            {
                try
                {
                    if(Players.Contains(p))
                        DropPlayer(p);
                }
                catch (Exception e)
                {
                    BotSettings.BotLogErrorList.Add(new BotLogError(e));
                }
            }
            try
            {
                foreach (Player p in Players)
                {
                    p.VisibleBy = new();
                }
            }
            catch (Exception e)
            {
                BotSettings.BotLogErrorList.Add(new BotLogError(e));
            }
        }

        public void UpdateRound()
        {
            Rounds++;
            foreach (Player p in Players)
            {
                foreach (Mine m in p.Mines)
                {
                    if (m.MineStatus == MineStatus.Released)
                    {
                        m.CheckRound(Rounds);
                    }
                }
                p.RoundEnded();
            }
        }

        public string DoTurn(TurnOption turnOption, string options, ulong playerId)
        {
            try
            {
                if (GameOver)
                    throw new GamePlayerException($"Game {GameStatus}!\n" +
                        $"`. game stats {this.Id}`\n" +
                        $"`. game leave`\n");
                if (playerId != Players[PlayerTurn].DiscordUser.Id)
                    throw new GamePlayerException($"It is {Players[PlayerTurn].DiscordUser.Username}'s turn!");
                GameTurns.Add(new GameTurn(DiscordGuild.Id,
                    DiscordGuild.Name, Id, Players[PlayerTurn].DiscordUser.Id,
                    turnOption, options, Players[PlayerTurn].DiscordUser.Username));
                switch (turnOption)
                {
                    case TurnOption.Airstrike:
                        Players[PlayerTurn].AirStrike(options, ref Players, ref Board,
                            ref KilledPlayers, ref Mines);
                        break;
                    case TurnOption.Call:
                        Players[PlayerTurn].Call(ref Players);
                        break;
                    case TurnOption.Cloak:
                        Players[PlayerTurn].Cloak(Convert.ToInt32(options), ref Players);
                        break;
                    case TurnOption.Melee:
                        Players[PlayerTurn].Melee(options, ref Players, ref Board,
                            ref KilledPlayers, ref Mines);
                        break;
                    case TurnOption.Mine:
                        Players[PlayerTurn].Mine(Rounds, options, ref Players, ref Board,
                            ref KilledPlayers, ref Mines);
                        break;
                    case TurnOption.Move:
                        Players[PlayerTurn].Move(options, ref Players, ref Board,
                            ref KilledPlayers, ref Mines);
                        break;
                    case TurnOption.Reload:
                        Players[PlayerTurn].Reload(ref Players);
                        break;
                    case TurnOption.Scan:
                        Players[PlayerTurn].Scan(ref Players, ref Mines);
                        break;
                    case TurnOption.Shoot:
                        Players[PlayerTurn].Shoot(options, ref Players, ref Board,
                            ref KilledPlayers, ref Mines);
                        break;
                    case TurnOption.Skip:
                        Players[PlayerTurn].Skip(ref Players);
                        break;
                    case TurnOption.Teleport:
                        Players[PlayerTurn].Teleport(options, ref Players, ref Board,
                            ref KilledPlayers, ref Mines);
                        break;
                    default:
                        throw new GamePlayerException("Internal Error, please try again!");
                }
                NextTurn(turnOption);
                return "Success!";
            }
            catch (Exception e)
            {
                BotSettings.BotLogErrorList.Add(new BotLogError(e));
                return e.Message;
            }
        }

        public string GetUnusedEmoji()
        {
            Random rand = new();
            string emoji = GameConst.PLAYER_EMOJIS.OrderBy((o) => rand.Next())
                .Where(e => !TakenEmojis.Contains(e)).First();
            TakenEmojis.Add(emoji);
            return emoji;
        }

        public void AddPlayer(CommandContext cmdCtx)
        {
            if (this.GameMode.HasFlag(GameMode.Unlocked))
            {
                throw new GamePlayerException("This Game is Locked!\n" +
                    "Please ask the owner of the game to play.\n" +
                    " Use `. game join` to see available games.", cmdCtx.Message.Author);
            }
            if (Players.Count + 2 > GameConst.MAX_HEIGHT)
            {
                throw new GamePlayerException("Can't start game too many players!" +
                    " The game board will be too big!", cmdCtx.Message.Author);
            }
            else if (this.Players.Where(p => p.Kills.Where(k => k.DiscordMember.Id == cmdCtx.Member.Id).Any()).Any())
            {
                // TODO: add respawn logic
                throw new GamePlayerException("Can't join, you already died in this game!");
            }
            else if (!this.AllowedPlayers.Contains(cmdCtx.Member.Id))
            {
                throw new GamePlayerException("Can't join, you must be invited!");
            }
            else
            {
                if (Players.Count + 1 > GameConst.MAX_PLAYERS)
                    throw new GamePlayerException("Too many Players sorry.\n" +
                        "You can start a new game, or wait for someone to quit.");
                Player player = new Player(cmdCtx, GetUnusedEmoji());
                if (Board != null &&
                    GameStatus != GameStatus.Created)
                {
                    player.Position = Board.GenerateRandomBoardPosition();
                    GameTurns.Add(new GameTurn(DiscordGuild.Id,
                        DiscordGuild.Name, Id, player.DiscordUser.Id,
                        TurnOption.Init, player.Position.ToString() + " " + Board.ToString(), 
                        player.DiscordUser.Username));
                }
                
                Player rejoined = KilledPlayers
                    .Where(p => p.DiscordUser.Id == cmdCtx.Message.Author.Id).FirstOrDefault();
                if (rejoined != default(Player))
                {
                    KilledPlayers.Remove(rejoined);
                }
                Players.Add(player);
                if (GameStatus != GameStatus.Created)
                    Board.ResizeBoard(Players.Count);
            }
        }

        public string Stats()
        {
            string output = "";
            if (!GameOver)
                return "Wait until game is complete!";
            foreach(GameTurn t in GameTurns)
            {
                output += $"{t}\n";
            }
            return output;
        }

        public void DropPlayer(Player p)
        {
            GameTurns.Add(new GameTurn(DiscordGuild.Id,
                DiscordGuild.Name, Id, p.DiscordUser.Id,
                TurnOption.Leave, p.Position.ToString() + " " + Board.ToString(), p.DiscordUser.Username));
            p.JustKilled = false;
            AllowedPlayers.Remove(p.DiscordUser.Id);
            Players.Remove(p);
            if (Players.Count < 1)
            {
                GameStatus = GameStatus.Abandoned;
                GameOver = true;
            }
        }

        public string RemovePlayer(CommandContext cmdCtx)
        {
            Player temp = Players.Where(p => p.DiscordMember.Id == cmdCtx.Member.Id).FirstOrDefault();
            if (temp.Equals(default(Player)))
                return "Player not found!";
            DropPlayer(temp);
            return "Success!";
        }
        public string RemovePlayer(DiscordUser discordUser)
        {
            Player temp = Players.Where(p => p.DiscordMember.Id == discordUser.Id).FirstOrDefault();
            if (temp.Equals(default(Player)))
                return "Player not found!";
            DropPlayer(temp);
            return "Success!";
        }


        public string PermitPlayer(ulong permittedMemberId)
        {
            if (AllowedPlayers.Contains(permittedMemberId))
                return "\nPlayer already allowed to play";
            AllowedPlayers.Add(permittedMemberId);
            return "\nUser is now allowed to play.";
        }

        public string RevokePermission(ulong permittedMemberId)
        {
            if (!AllowedPlayers.Contains(permittedMemberId))
                return "\nPlayer already revoked";
            AllowedPlayers.Remove(permittedMemberId);
            return "Success!";
        }

        public void RenderAll()
        {
            foreach (Player p in Players)
            {
                p.Notify(Board.RenderBoard(GameStatus, p, Players, Mines));
            }
        }

        public static void NotifyAll(List<Player> players, string msg)
        {
            players.ForEach(p =>
            {
                p.Notify(msg);
            });
        }

        public override string ToString()
        {
            return $"{this.Id}) {this.Owner.DiscordUser.Username}" +
                $" {string.Join(',', this.Players)}";
        }
    }
}

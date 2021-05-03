using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using RunOrGunGameBot.Bot.DataClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RunOrGunGameBot.Bot.Game
{
    public class Player
    {
        public DiscordMember DiscordMember;
        public DiscordUser DiscordUser;
        public DiscordDmChannel DiscordDmChannel;
        public Player KilledBy;
        public string Emoji;
        public PlayerStatus PlayerStatus;
        public List<Player> Kills = new();
        public List<Player> VisibleBy = new();
        public bool JustKilled = false;
        public BoardPosition Position;
        public List<Mine> Mines;
        public bool TurnActive;
        public bool HasBullet;
        public bool HasAirstrike;
        public int CloakRoundRemaining;

        public Player(CommandContext cmdCtx, string emoji)
        {
            Task<DiscordDmChannel> task = cmdCtx.Member.CreateDmChannelAsync();
            task.Wait();
            this.DiscordDmChannel = task.Result;
            this.DiscordUser = cmdCtx.Message.Author;
            this.DiscordMember = cmdCtx.Member;
            this.PlayerStatus = PlayerStatus.Alive;
            this.KilledBy = null;
            this.Emoji = emoji;
            this.HasBullet = true;
            this.HasAirstrike = true;
            this.Mines = Enumerable.Range(0, GameConst.START_MINE_COUNT).Select((i) => new Mine(this)).ToList();
        }

        public void Notify(string msg)
        {
            try
            {
                this.DiscordDmChannel.SendMessageAsync(msg);
            }
            catch (Exception exception)
            {
                BotSettings.BotLogErrorList.Add(new BotLogError(exception));
            }
        }

        public void Notify(DiscordEmbed discordEmbed)
        {
            try
            {
                this.DiscordDmChannel.SendMessageAsync(discordEmbed);
            }
            catch (Exception exception)
            {
                BotSettings.BotLogErrorList.Add(new BotLogError(exception));
            }
        }

        public void ChangeEmoji(string emoji)
        {
            this.Emoji = emoji;
        }

        public void AirStrike(string options, ref List<Player> players,
            ref Board board, ref List<Player> killed, ref List<Mine> mines)
        {
            bool failed = true;
            if (!this.HasAirstrike)
                throw new GamePlayerException("Airstrikes used up `. call` to ready next airstrike.");
            options = options.Trim();
            int intPart = int.Parse(Regex.Match(options, @"\d+").Value);
            char charPart = Convert.ToChar(options.Replace(intPart.ToString(), "").ToUpper());
            BoardPosition boardPosition =
                BoardPosition.UserPositionInput(charPart, intPart - 1, board.BoardHeight, board.BoardHeight);
            foreach (Player p in players)
            {
                if (p.Position == boardPosition)
                {
                    p.PlayerStatus = PlayerStatus.Airstriked;
                    p.KilledBy = this;
                    p.JustKilled = true;
                    this.Kills.Add(p);
                    killed.Add(p);
                    failed = false;
                    Game.NotifyAll(players,
                        $"{this.DiscordUser.Username} killed" +
                        $" {p.DiscordUser.Username} by airstrike!");
                }
            }
            foreach (Mine m in mines)
            {
                if (m.Position == boardPosition)
                {
                    m.Detonated();
                    Game.NotifyAll(players,
                        $"{this.DiscordUser.Username} detonated a mine");
                }
            }
            this.HasAirstrike = false;
            if (failed && !PlayerStatus.HasFlag(PlayerStatus.Cloaked))
                Game.NotifyAll(players,
                    $"{this.DiscordUser.Username}'s airstrike missed!\n");
        }

        public void Cloak(int cloakTime, ref List<Player> players)
        {
            if (this.PlayerStatus.HasFlag(PlayerStatus.Cloaked))
                throw new GamePlayerException("Already cloaked!");
            else if (0 < cloakTime && cloakTime <= GameConst.MAX_CLOAK_INTERVAL)
            {
                this.CloakRoundRemaining = cloakTime + 1; // + 1 For when TurnEnded() is called!
                this.PlayerStatus |= PlayerStatus.Cloaked;
                Game.NotifyAll(players,
                   $"{this.DiscordUser.Username} is cloaked!\n");
            }
            else
                throw new GamePlayerException("Invalid cloak time, try again. (1-3)");
        }

        public void Melee(string direction, ref List<Player> players,
            ref Board board, ref List<Player> killed, ref List<Mine> mines)
        {
            bool failed = true;
            BoardPosition boardPosition =
                new BoardPosition(Position.PositionRow, Position.PositionCol,
                board.BoardHeight, board.BoardWidth);
            boardPosition.TryFuturePosition(direction, 1, board.BoardHeight, board.BoardWidth);
            foreach (Player p in players)
            {
                if (p.Position == boardPosition)
                {
                    p.PlayerStatus = PlayerStatus.Slashed;
                    p.KilledBy = this;
                    p.JustKilled = true;
                    this.Kills.Add(p);
                    killed.Add(p);
                    failed = false;
                    Game.NotifyAll(players,
                        $"{this.DiscordUser.Username} killed" +
                        $" {p.DiscordUser.Username} by melee!");
                }
            }
            if (failed && !PlayerStatus.HasFlag(PlayerStatus.Cloaked))
                Game.NotifyAll(players,
                    $"{this.DiscordUser.Username} slashed air!\n" +
                    $"Melee and miss.");
        }

        public void Mine(int rounds, string direction,
            ref List<Player> players, ref Board board, ref List<Player> killed,
            ref List<Mine> mines)
        {
            bool failed = true;
            int index = -1;
            BoardPosition boardPosition =
                new BoardPosition(Position.PositionRow, Position.PositionCol,
                board.BoardHeight, board.BoardWidth);
            boardPosition.TryFuturePosition(direction, 1, board.BoardHeight, board.BoardWidth);
            for (int i = 0; i < Mines.Count; i++)
            {
                if (Mines[i].MineStatus.Equals(MineStatus.Stored))
                {
                    index = i;
                    i = Mines.Count;
                }
            }
            if (index == -1)
                throw new GamePlayerException("Out of mines!\n" +
                    "Try again!");
            Mines[index].Released(boardPosition, rounds);
            foreach (Player p in players)
            {
                if (p.Position == boardPosition)
                {
                    p.PlayerStatus = PlayerStatus.Exploded;
                    p.KilledBy = this;
                    p.JustKilled = true;
                    this.Kills.Add(p);
                    killed.Add(p);
                    Mines[index].Detonated(p);
                    failed = false;
                    Game.NotifyAll(players,
                        $"{this.DiscordUser.Username} killed" +
                        $" {p.DiscordUser.Username} by mine!");
                }
            }
            mines.Add(Mines[index]);
            Mines.RemoveAt(index);
            if (failed && !PlayerStatus.HasFlag(PlayerStatus.Cloaked))
                Game.NotifyAll(players,
                    $"{this.DiscordUser.Username} placed a mine!\n");
        }

        internal void Call(ref List<Player> players)
        {
            if (this.HasAirstrike)
                throw new GamePlayerException("Already called in!\n" +
                    "Try again!");
            this.HasAirstrike = true;
            if (!PlayerStatus.HasFlag(PlayerStatus.Cloaked))
                Game.NotifyAll(players,
                    $"{this.DiscordUser.Username} readied an airstrike!\n");
        }

        public void Move(string direction, ref List<Player> players,
            ref Board board, ref List<Player> killed, ref List<Mine> mines)
        {
            Position.TryFuturePosition(direction, 1, board.BoardHeight, board.BoardWidth);
            CheckForMines(ref players, ref mines, ref killed);
            if (!PlayerStatus.HasFlag(PlayerStatus.Cloaked))
                Game.NotifyAll(players,
                    $"{this.DiscordUser.Username} moved!\n");
        }

        public void Scan(ref List<Player> players, ref List<Mine> mines)
        {
            int r, c;
            foreach (Player p in players)
            {
                r = Math.Abs(p.Position.PositionRow - this.Position.PositionRow);
                c = Math.Abs(p.Position.PositionCol - this.Position.PositionCol);
                if ((r < 2 && c < 2) && !(r == 0 && c == 0))
                    p.VisibleBy.Add(this);
            }
            foreach (Mine m in mines)
            {
                if (m.MineStatus == MineStatus.Released)
                {
                    r = Math.Abs(m.Position.PositionRow - this.Position.PositionRow);
                    c = Math.Abs(m.Position.PositionCol - this.Position.PositionCol);
                    if ((r < 2 && c < 2) && !(r == 0 && c == 0))
                        m.Scanned(this);
                }
            }
            if (!PlayerStatus.HasFlag(PlayerStatus.Cloaked))
                Game.NotifyAll(players,
                    $"{this.DiscordUser.Username} scanned!\n");
        }

        public void Skip(ref List<Player> players)
        {
            if (PlayerStatus != PlayerStatus.Cloaked)
                Game.NotifyAll(players,
                    $"{this.DiscordUser.Username} skipped a turn!\n");
        }

        public void Shoot(string options, ref List<Player> players,
            ref Board board, ref List<Player> killed, ref List<Mine> mines)
        {
            if (!this.HasBullet)
                throw new GamePlayerException("Bullets used up `. reload` to reload.");
            bool failed = true;
            BoardPosition boardPosition =
                new BoardPosition(Position.PositionRow, Position.PositionCol,
                board.BoardHeight, board.BoardWidth);
            boardPosition.TryFuturePosition(options, -1, board.BoardHeight, board.BoardWidth);
            bool rc = Position.PositionRow < boardPosition.PositionRow; // row compare
            bool cc = Position.PositionCol < boardPosition.PositionCol; // col compare
            bool re = Position.PositionRow == boardPosition.PositionRow; // row equal
            bool ce = Position.PositionCol == boardPosition.PositionCol; // col equal
            bool targetFound = false;
            List<Player> ps = new();
            List<Mine> ms = new();
            for (int i = Position.PositionRow + (re ? 0 : (rc ? 1 : -1));
                (rc ? i <= boardPosition.PositionRow : i >= boardPosition.PositionRow);
                i += (rc ? 1 : -1))
            {
                for (int j = Position.PositionCol + (ce ? 0 : (cc ? 1 : -1));
                (cc ? j <= boardPosition.PositionCol : j >= boardPosition.PositionCol);
                j += (cc ? 1 : -1))
                {
                    foreach (Player p in players)
                    {
                        if (p.Position.PositionRow == i
                            && p.Position.PositionCol == j)
                        {
                            ps.Add(p);
                            targetFound = true;
                        }
                    }
                    foreach (Mine m in mines)
                    {

                        if (m.Position.PositionRow == i
                            && m.Position.PositionCol == j)
                        {
                            ms.Add(m);
                            targetFound = true;
                        }
                    }
                    if (targetFound)
                        break;
                }
                if (targetFound)
                    break;
            }
            if (ms.Count() > 0)
            {
                foreach(Mine m in ms)
                {
                    m.Detonated();
                    Game.NotifyAll(players,
                        $"{this.DiscordUser.Username} shot" +
                        $" a mine and blew it up!");
                }
            }
            if (ps.Count() > 0)
            {
                foreach (Player p in ps)
                {
                    p.PlayerStatus = PlayerStatus.Shot;
                    p.KilledBy = this;
                    p.JustKilled = true;
                    this.Kills.Add(p);
                    killed.Add(p);
                    failed = false;
                    Game.NotifyAll(players,
                        $"{this.DiscordUser.Username} shot" +
                        $" {p.DiscordUser.Username}!");
                }
            }
            this.HasBullet = false;
            if (failed && !PlayerStatus.HasFlag(PlayerStatus.Cloaked))
                Game.NotifyAll(players,
                    $"{this.DiscordUser.Username} shot and missed!\n");
        }

        public void Reload(ref List<Player> players)
        {
            if (this.HasBullet)
                throw new GamePlayerException("Already loaded!\n" +
                    "Try again!");
            this.HasBullet = true;
            if (!PlayerStatus.HasFlag(PlayerStatus.Cloaked))
                Game.NotifyAll(players,
                    $"{this.DiscordUser.Username} reloaded!\n");
        }

        public void Teleport(string options, ref List<Player> players,
            ref Board board, ref List<Player> killed, ref List<Mine> mines)
        {
            options = options.Trim();
            int intPart = int.Parse(Regex.Match(options, @"\d+").Value);
            string stringPart = Convert.ToString(options.Replace(intPart.ToString(), ""));
            if (GameConst.MAX_TELEPORT_DISTANCE < intPart)
                throw new GamePlayerException($"Cannot teleport {intPart} tiles\n" +
                    $"Max distance is {GameConst.MAX_TELEPORT_DISTANCE}");
            Position.TryFuturePosition(stringPart, intPart, board.BoardHeight, board.BoardWidth);
            PlayerStatus = PlayerStatus.Teleporting;
            if (!PlayerStatus.HasFlag(PlayerStatus.Cloaked))
                Game.NotifyAll(players,
                    $"{this.DiscordUser.Username} teleported!\n");
            CheckForMines(ref players, ref mines, ref killed);
        }

        public void CheckForMines(ref List<Player> players,
            ref List<Mine> mines, ref List<Player> killed)
        {
            foreach (Player p in players)
            {
                foreach (Mine m in mines)
                {
                    // TODO: add stacked mines logic
                    if (m.MineStatus == MineStatus.Released
                        && m.Position == this.Position)
                    {
                        this.PlayerStatus = PlayerStatus.Exploded;
                        this.KilledBy = p;
                        p.Kills.Add(this);
                        killed.Add(this);
                        m.Detonated(this);
                        Game.NotifyAll(players,
                            $"{this.DiscordUser.Username} died from" +
                            $" a mine left by {p.DiscordUser.Username}!");
                    }
                }
            }
        }

        public void RoundEnded()
        {
            this.CloakRoundRemaining--;
        }

        public static bool operator ==(Player a, Player b)
        {
            if (a is null || b is null)
                return false;
            return a.DiscordUser.Id == b.DiscordUser.Id;
        }

        public static bool operator !=(Player a, Player b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            return this == obj as Player;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return this.DiscordUser.Username;
        }
    }
}

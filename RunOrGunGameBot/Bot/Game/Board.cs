using DSharpPlus.Entities;
using System;
using System.Linq;
using System.Collections.Generic;

namespace RunOrGunGameBot.Bot.Game
{
    public class Board
    {
        public int BoardWidth;
        public int BoardHeight;
        public List<List<BoardTile>> boardTiles;

        public Board(int numberOfPlayers)
        {
            this.BoardHeight = numberOfPlayers + GameConst.PLAYER_COUNT_ADDER;
            this.BoardWidth = this.BoardHeight;
            boardTiles = GenerateBoard();
        }

        public List<List<BoardTile>> GenerateBoard()
        {
            List<List<BoardTile>> temp = new();
            for (int i = 0; i < BoardHeight; i++)
            {
                temp.Add(new());
                for (int j = 0; j < BoardWidth; j++)
                {
                    temp[i].Add(new(i, j, this.BoardWidth, this.BoardHeight));
                }
            }
            return temp;
        }

        public List<List<BoardTile>> RegenerateBoard()
        {
            List<List<BoardTile>> temp = GenerateBoard();
            for (int i = 0; i < BoardHeight; i++)
            {
                for (int j = 0; j < BoardWidth; j++)
                {
                    BoardTile tempTile;
                    try
                    {
                        tempTile = boardTiles[i][j];
                    }
                    catch (Exception)
                    {
                        tempTile = new(i, j, this.BoardWidth, this.BoardHeight);
                    }
                    temp[i][j] = tempTile;
                }
            }
            return temp;
        }

        public void ResizeBoard(int numberOfPlayers)
        {
            this.BoardWidth = numberOfPlayers + GameConst.PLAYER_COUNT_ADDER;
            this.BoardHeight = this.BoardWidth;
        }

        public DiscordEmbed RenderBoard(GameStatus gameStatus, Player viewer,
            List<Player> enemies, List<Mine> mines)
        {
            Board temp = new Board(BoardHeight);
            for (int i = 0; i < BoardHeight; i++)
            {
                for (int j = 0; j < BoardWidth; j++)
                {
                    temp.boardTiles[i][j].Emoji = ":black_large_square:";
                }
            }
            List<Player> visible = (from p in enemies
                                    from v in p.VisibleBy
                                    where v.DiscordUser.Id == viewer.DiscordUser.Id
                                    select p).ToList();
            List<BoardPosition> minesPositions = (from m in mines
                                         from p in m.VisibleBy
                                         where m.MineStatus == MineStatus.Released
                                         where p.DiscordUser.Id == viewer.DiscordUser.Id
                                         select m.Position).ToList();
            List<BoardPosition> justKilled;
            if (gameStatus.Equals(GameStatus.Completed))
            {
                justKilled = (from k in viewer.Kills
                              select k.Position).ToList();
            }
            else
            {
                justKilled = (from k in enemies
                              where k.JustKilled
                              select k.Position).ToList();
            }
            foreach (BoardPosition mine in minesPositions)
            {
                temp.boardTiles[mine.PositionRow][mine.PositionCol].Emoji = GameConst.MINE_EMOJI;
            }
            foreach (Player v in visible)
            {
                temp.boardTiles[v.Position.PositionRow][v.Position.PositionCol].Emoji = v.Emoji;
            }
            foreach (BoardPosition player in justKilled)
            {
                temp.boardTiles[player.PositionRow][player.PositionCol].Emoji = GameConst.PLAYER_DEATH_EMOJI;
            }
            try
            {
                _ = (from e in enemies
                     from k in e.Kills
                     where k.JustKilled
                     where k.DiscordUser.Id == viewer.DiscordUser.Id
                     select k).First();
                temp.boardTiles[viewer.Position.PositionRow][viewer.Position.PositionCol].Emoji = GameConst.PLAYER_DEATH_EMOJI;
            }
            catch (Exception)
            {
                temp.boardTiles[viewer.Position.PositionRow][viewer.Position.PositionCol].Emoji = viewer.Emoji;
            }
            string boardOutput = "";
            for (int i = 0; i < BoardHeight; i++)
                boardOutput += GameConst.COLUMN_EMOJI[i];
            boardOutput += "\n";
            for (int i = 0; i < BoardHeight; i++)
            {
                for (int j = 0; j < BoardWidth; j++)
                {
                    boardOutput += temp.boardTiles[i][j].Emoji;
                }
                boardOutput += GameConst.ROW_EMOJI[i];
                boardOutput += "\n";
            }
            DiscordEmbedBuilder discordEmbedBuilder = new()
            {
                Title = "RunOrGun Game | `. help`",
                Description = boardOutput
            };
            discordEmbedBuilder.AddField("Player", $"{viewer.DiscordUser.Username}");
            discordEmbedBuilder.AddField("Help", "```\n" +
                ". help\n" +
                ". help (cmd)\n" +
                ". help move\n" +
                "\n```");
            return discordEmbedBuilder.Build();
        }

        public BoardPosition GenerateRandomBoardPosition()
        {
            Random rand = new();
            return new BoardPosition(rand.Next(0, BoardHeight), rand.Next(0, BoardWidth),
                BoardHeight, BoardWidth);
        }

        public override string ToString()
        {
            return $"{BoardHeight} {BoardWidth}";
        }
    }
}

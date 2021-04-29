using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                for (int j = 0; j < BoardWidth; j++)
                {
                    temp[i][j] = new(i, j);
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
                        tempTile = new(i, j);
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
    }
}

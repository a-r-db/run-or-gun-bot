using System.Collections.Generic;

namespace RunOrGunGameBot.Bot.Game
{
    public class BoardTile
    {
        public BoardPosition BoardPosition;
        public string Emoji;

        public BoardTile(int i, int j, int boardHeight, int boardWidth)
        {
            this.BoardPosition = new(i, j, boardHeight, boardWidth);
        }
    }
}

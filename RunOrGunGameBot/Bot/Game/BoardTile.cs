using System.Collections.Generic;

namespace RunOrGunGameBot.Bot.Game
{
    public class BoardTile
    {
        public BoardPosition BoardPosition;

        public BoardTile(int i, int j)
        {
            this.BoardPosition = new(i, j);
        }
    }
}

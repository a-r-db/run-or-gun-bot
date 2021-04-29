using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RunOrGunGameBot.Bot.Game
{
    public class BoardPosition
    {
        public int PositionAcrossWidth;
        public int PositionAcrossHeight;

        public BoardPosition(int i, int j)
        {
            this.PositionAcrossHeight = i;
            this.PositionAcrossWidth = j;
        }

        public string PositionWidthChar()
        {
            return $"{Convert.ToChar(PositionAcrossWidth + 40),2}";
        }

        public string PositionHeightString()
        {
            return $"{PositionAcrossHeight.ToString().PadLeft(2, '0')}";
        }

        public bool BadPosition(int boardHeight, int boardWidth)
        {
            if (PositionAcrossHeight < boardHeight && PositionAcrossHeight > -1
                && PositionAcrossWidth < boardWidth && PositionAcrossWidth > -1)
                return false;
            return true;
        }

        public void TryFuturePosition(string direction, int distance, int boardHeight, int boardWidth)
        {
            BoardPosition temp = this;
            switch (direction)
            {
                case "up":
                case "w":
                    temp.PositionAcrossHeight -= distance;
                    break;
                case "down":
                case "s":
                    temp.PositionAcrossHeight += distance;
                    break;
                case "right":
                case "d":
                    temp.PositionAcrossWidth += distance;
                    break;
                case "left":
                case "a":
                    temp.PositionAcrossWidth -= distance;
                    break;
            }
            if (BadPosition(boardHeight, boardWidth))
                throw new GameBoardException("Position off the board (out of range). Try again.");
            this.PositionAcrossHeight = temp.PositionAcrossHeight;
            this.PositionAcrossWidth = temp.PositionAcrossWidth;
        }

        public static BoardPosition UserPositionInput(char c, int i)
        {
            return new(Convert.ToInt32(c) + 40, i);
        }
    }
}

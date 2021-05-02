using System;

namespace RunOrGunGameBot.Bot.Game
{
    public class BoardPosition
    {
        public int PositionRow;
        public int PositionCol;

        public BoardPosition(int row, int col, int boardHeight, int boardWidth)
        {
            this.PositionRow = row;
            this.PositionCol = col;
            if (this.BadPosition(boardHeight, boardWidth))
            {
                throw new GameBoardException($"Position off the board (out of range). Try again. {row} {col}");
            }
        }

        public string PositionToLetterString()
        {
            return $"{Convert.ToChar(PositionCol + 65),2}";
        }

        public string PositionToNumberString()
        {
            return $"{PositionRow.ToString().PadLeft(2, '0')}";
        }

        public bool BadPosition(int boardHeight, int boardWidth)
        {
            if (PositionRow < boardHeight && PositionRow > -1
                && PositionCol < boardWidth && PositionCol > -1)
                return false;
            return true;
        }

        public static bool BadPosition(BoardPosition temp, int boardHeight, int boardWidth)
        {
            if (temp.PositionRow < boardHeight && temp.PositionRow > -1
                && temp.PositionCol < boardWidth && temp.PositionCol > -1)
                return false;
            return true;
        }

        public void TryFuturePosition(string direction, int distance, int boardHeight, int boardWidth)
        {
            BoardPosition temp
                = new BoardPosition(this.PositionRow, this.PositionCol,
                boardHeight, boardWidth);
            switch (direction)
            {
                case "up":
                case "u":
                    if (distance != -1)
                        temp.PositionRow -= distance;
                    else
                        temp.PositionRow = 0;
                    break;
                case "down":
                case "d":
                    if (distance != -1)
                        temp.PositionRow += distance;
                    else
                        temp.PositionRow = boardHeight - 1;
                    break;
                case "right":
                case "r":
                    if (distance != -1)
                        temp.PositionCol += distance;
                    else
                        temp.PositionCol = boardHeight - 1;
                    break;
                case "left":
                case "l":
                    if (distance != -1)
                        temp.PositionCol -= distance;
                    else
                        temp.PositionCol = 0;
                    break;
                default:
                    throw new GameBoardException($"Invalid option {direction}");
            }
            if (BadPosition(temp, boardHeight, boardWidth))
                throw new GameBoardException($"Position off the board (out of range). Try again. {temp.PositionRow} {temp.PositionCol}");
            this.PositionRow = temp.PositionRow;
            this.PositionCol = temp.PositionCol;
        }

        public static BoardPosition UserPositionInput(char c, int i, int boardHeight, int boardWidth)
        {
            return new(Convert.ToInt32(c) - 65, i, boardHeight, boardWidth);
        }

        public static bool operator ==(BoardPosition a, BoardPosition b)
        {
            return a.PositionRow == b.PositionRow
                && a.PositionCol == b.PositionCol;
        }

        public static bool operator !=(BoardPosition a, BoardPosition b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            return this == obj as BoardPosition;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public static string PositionToLetterString(int input)
        {
            return $"{Convert.ToChar(input + 65),2}";
        }

        public static string PositionToNumberString(int input)
        {
            return $"{input.ToString().PadLeft(2, '0')}";
        }

        public override string ToString()
        {
            return $"{this.PositionRow} {this.PositionCol}";
        }
    }
}

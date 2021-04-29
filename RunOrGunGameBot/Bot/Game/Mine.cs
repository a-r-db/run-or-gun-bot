namespace RunOrGunGameBot.Bot.Game
{
    public class Mine
    {
        public MineStatus MineStatus = MineStatus.Stored;
        public BoardPosition Position;
        public Player Owner;
        public int ReleasedRound;
        public Player PlayerGot = null;

        public object GamesConsts { get; private set; }

        public Mine(Player owner)
        {
            this.Owner = owner;
        }

        public void Released(BoardPosition position, int round)
        {
            this.Position = position;
            this.ReleasedRound = round;
            this.MineStatus = MineStatus.Released;
        }

        public void Detonated(Player player)
        {
            this.MineStatus = MineStatus.Detonated;
            this.PlayerGot = player;
        }

        public void CheckRound(int round)
        {
            if (round - ReleasedRound >= GameConst.MAX_ROUNDS_PER_MINE)
            {
                this.MineStatus = MineStatus.Decommissioned;
            }
        }
    }
}

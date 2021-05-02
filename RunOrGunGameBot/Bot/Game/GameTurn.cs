using System;

namespace RunOrGunGameBot.Bot.Game
{
    [Serializable]
    public class GameTurn
    {
        public ulong GuildId { get; set; }
        public string GuildName { get; set; }
        public ulong GameId { get; set; }
        public ulong PlayerId { get; set; }
        public TurnOption TurnOption { get; set; }
        public string Options { get; set; }
        public string PlayerName { get; set; }

        public GameTurn()
        {

        }

        public GameTurn(ulong guildId, string guildName, ulong gameId, ulong playerId, 
            TurnOption turnOption, string options, string playerName)
        {
            this.GuildId = guildId;
            this.GuildName = guildName;
            this.GameId = gameId;
            this.PlayerId = playerId;
            this.TurnOption = turnOption;
            this.Options = options;
            this.PlayerName = playerName;
        }

        public override string ToString()
        {
            return $"{GuildName} {GameId} {PlayerName} {TurnOption} {Options}";
        }
        public string ToLog()
        {
            return $"{GuildId} {GameId} {PlayerId} {TurnOption} {Options}";
        }
    }
}

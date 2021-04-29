using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RunOrGunGameBot.Bot.Game
{
    [Serializable]
    public class GamePlayerException : Exception
    {
        public DiscordUser DiscordUser { get; }

        public GamePlayerException() { }

        public GamePlayerException(string message)
            : base(message) { }

        public GamePlayerException(string message, Exception inner)
            : base(message, inner) { }

        public GamePlayerException(string message, DiscordUser personWantsToJoin)
            : this(message)
        {
            DiscordUser = personWantsToJoin;
        }
    }

    [Serializable]
    public class GameBoardException : Exception
    {

        public GameBoardException() { }

        public GameBoardException(string message)
            : base(message) { }

        public GameBoardException(string message, Exception inner)
            : base(message, inner) { }
    }


    [Serializable]
    public class GameException : Exception
    {

        public GameException() { }

        public GameException(string message)
            : base(message) { }

        public GameException(string message, Exception inner)
            : base(message, inner) { }
    }
}

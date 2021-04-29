using System;

namespace RunOrGunGameBot.Bot.Game
{
    [Flags]
    public enum GameMode
    {
        Default = 0,
        Guided = 1,
        Emoji = 2,
        Unlocked = 4,
        MapUnlocked = 8,
        CustomOrder = 16
    }
}

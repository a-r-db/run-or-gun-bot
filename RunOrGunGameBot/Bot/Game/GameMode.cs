using System;

namespace RunOrGunGameBot.Bot.Game
{
    [Flags]
    public enum GameMode
    {
        Default = 0,
        Guided = 1, // Users pick their start position
        Emoji = 2, // Users customize their emoji
        Unlocked = 4, // Anyone can join
        MapUnlocked = 8, // Map size is variable
        CustomOrder = 16, // Users play in a custom order
        Respawn = 32 // respawns allowed
    }
}

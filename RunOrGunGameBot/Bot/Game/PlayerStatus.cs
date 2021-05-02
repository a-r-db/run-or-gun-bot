using System;

namespace RunOrGunGameBot.Bot.Game
{
    [Flags]
    public enum PlayerStatus
    {
        Alive = 0, // default
        Forfeit = 1, // automatic loss
        Cloaked = 2, // player is cloaked
        Teleporting = 4, // takes one turn, player is in a wormhole during teleport
        Exploded = 8, // loss/death by mine
        Fallout = 16, // loss/death by stacked mines
        Ignited = 32, // loss/death by ingited mines (mines blown up by gun)
        Slashed = 64, // loss/death by melee
        Shot = 128, // loss/death by gun
        Victory = 256, // last emoji standing, game over!
        Airstriked = 512 // loss/death by airstrike
    }
}

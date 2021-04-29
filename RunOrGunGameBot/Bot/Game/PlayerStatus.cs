using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RunOrGunGameBot.Bot.Game
{
    public enum PlayerStatus
    {
        Alive, // default
        Teleporting, // takes one turn
        Forfeit, // automatic loss
        Exploded, // loss/death by mine
        Fallout, // loss/death by stacked mines
        Ignited, // loss/death by ingited mines (mines blown up by gun)
        Slashed, // loss/death by melee
        Shot // loss/death by gun
    }
}

namespace RunOrGunGameBot.Bot.Game
{
    public class GameConst
    {
        //
        // Command Shortforms
        // melee => me
        // mine => m
        // reload => r
        // scan => s
        // shoot => sh
        // teleport => t
        //
        // Keyboard Shortforms
        // left => a
        // right => d
        // up => w
        // down => s
        //
        // |q|w|e|
        //  |a|s|d|
        //
        // Board.cs
        public static readonly int MIN_WIDTH = 4;
        public static readonly int MIN_HEIGHT = 4;
        public static readonly int MAX_WIDTH = 10;
        public static readonly int MAX_HEIGHT = 10;
        public static readonly int PLAYER_COUNT_ADDER = 3;
        // Game.cs
        public static readonly int MAX_PLAYERS = 8;
        public static readonly int MIN_PLAYERS = 2; // requires two unless AI/bot is created!
        // Cloak
        public static readonly int MAX_ROUNDS_OF_CLOAK = 8;
        public static readonly int MAX_CLOAK_INTERVAL = 3;
        /**
         * Hide yourself from enemy scans
         * Hide your habits from enemy players
         * 
         * !rog cloak 3 (player u1)
         * !rog cloak 1 (player u2)
         * 
         * !rog c 3 (player u1)
         * !rog c 1 (player u2)
         */
        // Melee
        /**
         * Melee Attack 1 tile up, down, left or right to your posiition
         * Your Placement is limited to the board.
         * Melee attacks do not remain on the board.
         * 
         * example: `u` is a player, `me` is a mine
         *
         * |  |  |  |  |  |
         * |me|u1|  |  |  |
         * |  |  |  |  |  |
         * |  |  |  |  |  |
         * |  |  |  |u2|me|
         * |  |  |  |  |  |
         * 
         * Sample Commands
         * !rog melee left (player u1)
         * !rog melee right (player u2)
         * 
         * !rog me a (player u1)
         * !rog me d (player u2)
         */
        // Mine.cs
        public static readonly int MAX_ROUNDS_PER_MINE = 6;
        public static readonly int MAX_DISTANCE = 1;
        public static readonly int START_MINE_COUNT = 10;
        /**
         * Place a mine 1 tile up, down, left or right to your posiition
         * Your Placement is limited to the board.
         * Mines you've place will show up on your screen only.
         * Other Player's Mines you've scanned will show up on your screen too.
         * 
         * If you stack 1 mine, when it is detonated the blast raidus is 1x1.
         * If you stack 2 mines, when they are detonated the blast radius is 3x3.
         * If you stack 3 mines, they detonate immediately the blast radius is 3x3.
         * 3 mines stacked will kill whoever stacked them and anyone in the blast radius.
         * 
         * You can plant mines and move away, to shoot them later.
         * 
         * example: `u` is a player, `m` is a mine
         *
         * |  |  |  |  |  |
         * |m |u1|  |  |  |
         * |  |  |  |  |  |
         * |  |  |  |  |  |
         * |  |  |  |u2|m |
         * |  |  |  |  |  |
         * 
         * Sample Commands
         * !rog mine left (player u1)
         * !rog mine right (player u2)
         * !rog m a (player u1)
         * !rog m d (player u2)
         */
        // Reload
        /**
         * Reloads your gun after shooting, 
         * Takes one turn.
         * !rog reload
         * !rog r
         */
        // Scan
        /**
         * Scan the first 3 tiles up, down, left or right to your posiition
         * Your Scan is limited to the board.
         * 
         * example: `u` is a player, `s` is a scan
         *
         * |s |  |  |  |  |
         * |s |u1|  |  |  |
         * |s |  |  |  |  |
         * |  |  |  |  |  |
         * |  |  |  |u2|  |
         * |  |  |s |s |s |
         * 
         * Sample Commands
         * !rog scan left (player u1)
         * !rog scan down (player u2)
         * !rog s a (player u1)
         * !rog s d (player u2)
         */
        // Shoot
        /**
         * Shoot.
         * You have unlimited shots, but must reload,
         * Reloading takes one turn.
         * 
         * example: `u` is a player, `s` is a shot
         *
         * |s |- |- |- |u1|
         * |  |  |  |  |  |
         * |  |  |  |  |  |
         * |  |s |  |  |  |
         * |  |  |\ |  |  |
         * |  |  |  |u2|  |
         * 
         * Sample Commands
         * !rog shoot A 1 (player u1)
         * !rog shoot B 4 (player u2)
         * !rog sh A 1 (player u1)
         * !rog sh B 4 (player u2)
         */
        // Teleport
        public static readonly int MAX_TELEPORT_DISTANCE = 5;
        public static readonly int MAX_TELEPORT_TOTAL = 10;
        /**
         * Teleport 5 or less tiles at a time.
         * Warning you only have 10 tiles of teleportation available.
         * 
         * example: `u` is a player, `t` is a teleport
         *
         * |u1|- |- |- |t |
         * |  |  |  |  |  |
         * |  |  |  |  |  |
         * |  |  |  |  |  |
         * |  |  |  |t |  |
         * |  |  |  |u2|  |
         * 
         * Sample Commands
         * !rog teleport right 4 (player u1)
         * !rog teleport up 1 (player u2)
         * !rog t d 4 (player u1)
         * !rog t w 1 (player u2)
         */
    }
}

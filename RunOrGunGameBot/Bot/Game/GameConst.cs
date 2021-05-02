namespace RunOrGunGameBot.Bot.Game
{
    public class GameConst
    {
        // Guild
        public static readonly string GUILD_CATEGORY_NAME = "BOT";
        public static readonly string GUILD_CHANNEL_NAME = "run-or-gun-bot";
        //
        // Command Shortforms
        // cloak => c
        // melee => me
        // mine => m
        // move => mv
        // reload => r
        // scan => s
        // shoot => sh
        // skip => sk
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
        // Positional data can be input a number of ways
        // All of the following are valid
        //
        // Board.cs
        public static readonly string[] ROW_EMOJI = new string[]
        {
            ":regional_indicator_a:",":regional_indicator_b:",
            ":regional_indicator_c:",":regional_indicator_d:",
            ":regional_indicator_e:",":regional_indicator_f:",
            ":regional_indicator_g:",":regional_indicator_h:",
            ":regional_indicator_i:",":regional_indicator_j:"
        };
        public static readonly string[] COLUMN_EMOJI = new string[]
        {
            ":one:",":two:",
            ":three:",":four:",
            ":five:",":six:",
            ":seven:",":eight:",
            ":nine:",":keycap_ten:"
        };
        public static readonly int MIN_WIDTH = 4;
        public static readonly int MIN_HEIGHT = 4;
        public static readonly int MAX_WIDTH = 10;
        public static readonly int MAX_HEIGHT = 10;
        public static readonly int PLAYER_COUNT_ADDER = 3;
        public static readonly string PLAIN_TILE_EMOJI = ":brown_square:";
        // Game.cs
        public static readonly int MAX_PLAYERS = 7;
        public static readonly int MIN_PLAYERS = 2; // requires two unless AI/bot is created!
        // PlayerTile.cs
        public static readonly string[] PLAYER_EMOJIS = new string[] { ":cowboy:", ":alien:", ":firefighter:", ":farmer:",
            ":spy:", ":cop:", ":superhero:", ":ninja:", ":zombie:", ":mage:", ":ghost:", ":poop:", ":elf:", ":supervillain:",
            ":genie:", ":fairy:"};
        public static readonly string PLAYER_DEATH_EMOJI = ":skull:";
        // Cloak
        public static readonly int MAX_ROUNDS_OF_CLOAK = 8;
        public static readonly int MAX_CLOAK_INTERVAL = 3;
        /**
         * Hide yourself from enemy scans
         * Hide your habits from enemy players
         * 
         * . cloak 3 (player u1)
         * . cloak 1 (player u2)
         * 
         * . c 3 (player u1)
         * . c 1 (player u2)
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
         * . melee left (player u1)
         * . melee right (player u2)
         * 
         * . me a (player u1)
         * . me d (player u2)
         */
        // Mine.cs
        public static readonly int MAX_ROUNDS_PER_MINE = 3;
        public static readonly int MAX_DISTANCE = 1;
        public static readonly int START_MINE_COUNT = 8;
        public static readonly string MINE_EMOJI = ":bomb:";
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
         * . mine left (player u1)
         * . mine right (player u2)
         * . m a (player u1)
         * . m d (player u2)
         */
        // Move
        /**
         * Place a move 1 tile up, down, left or right to your posiition
         * Your Placement is limited to the board dimensions.
         * 
         * example: `u` is a player, `mv` is a move
         *
         * |  |  |  |  |  |
         * |mv|u1|  |  |  |
         * |  |  |  |  |  |
         * |  |  |  |  |  |
         * |  |  |  |u2|mv|
         * |  |  |  |  |  |
         * 
         * Sample Commands
         * . move left (player u1)
         * . move right (player u2)
         * . mv a (player u1)
         * . mv d (player u2)
         */
        // Reload
        /**
         * Reloads your gun after shooting, 
         * Takes one turn.
         * . reload
         * . r
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
         * . scan left (player u1)
         * . scan down (player u2)
         * . s a (player u1)
         * . s d (player u2)
         */
        // Shoot
        public static readonly int MAX_SHOOT_DISTANCE = 6;
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
         * . shoot A 1 (player u1)
         * . shoot B 4 (player u2)
         * . sh A 1 (player u1)
         * . sh B 4 (player u2)
         */

        // Skip
        /**
         * Skip.
         * Turn is skipped.
         * 
         * . skip
         * . sk
         */
        // Teleport
        public static readonly int MAX_TELEPORT_DISTANCE = 6;
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
         * . teleport right 4 (player u1)
         * . teleport up 1 (player u2)
         * . t d 4 (player u1)
         * . t w 1 (player u2)
         */
    }
}

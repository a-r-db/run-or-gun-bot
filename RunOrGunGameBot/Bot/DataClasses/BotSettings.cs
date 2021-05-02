using RunOrGunGameBot.Bot.DataServices;
using RunOrGunGameBot.Bot.Game;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace RunOrGunGameBot.Bot.DataClasses
{
    public class BotSettings
    {
        public static readonly BotService BotService = new();
        public static readonly List<BotLog> BotLogList = new();
        public static readonly List<BotLogError> BotLogErrorList = new();
        public static readonly List<GuildQueue> BotGuilds = new();
        public static readonly List<BotChannel> BotChannels = new();
        public static readonly List<BotMessage> BotMessages = new();
        public static string Token { get; set; }

        private static bool _enabled;
        public static bool Enabled
        {
            get => _enabled;
            set
            {
                try
                {
                    BotService.BotStateChange();
                    _enabled = value;
                }
                catch (Exception exception)
                {
                    _enabled = false;
                    BotLogErrorList.Add(new BotLogError(exception));
                }
            }
        }

        private static BotGuild _selectedGuild;
        public static BotGuild SelectedGuild
        {
            get => _selectedGuild;
            set
            {
                _selectedGuild = value;
                _selectedChannel = default;
                _selectedMessage = default;
                BotChannels.Clear();
                BotMessages.Clear();

                if (_selectedGuild.Guild != null)
                {
                    var chns = _selectedGuild.Guild.Channels.Values
                        .Where(xc => xc.Type == DSharpPlus.ChannelType.Text)
                        .OrderBy(xc => xc.Position)
                        .Select(xc => new BotChannel(xc));
                    foreach (var xbc in chns)
                        BotChannels.Add(xbc);
                }
            }
        }

        private static BotChannel _selectedChannel;
        public static BotChannel SelectedChannel
        {
            get => _selectedChannel;
            set
            {
                _selectedChannel = value;
                _selectedMessage = default;
                BotMessages.Clear();
            }
        }

        private static BotMessage _selectedMessage;
        public static BotMessage SelectedMessage
        {
            get => _selectedMessage;
            set {
                _selectedMessage = value; 
            }
        }

        public static void SaveLogs<T>(T data, string prefix)
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));
            TextWriter writer =
                new StreamWriter("." + Path.DirectorySeparatorChar + prefix
                + DateTimeOffset.Now.ToUnixTimeSeconds().ToString() + ".log");
            ser.Serialize(writer, data);
            writer.Close();
        }
    }
}
﻿using System;

namespace TwitchLib.Client.Events
{
    /// <inheritdoc />
    /// <summary>Args representing the client left a channel event.</summary>
    public class OnLeftChannelArgs : EventArgs
    {
        /// <summary>The username of the bot that left the channel.</summary>
        public string BotUsername;
        /// <summary>Channel that bot just left (parted).</summary>
        public string Channel;
    }
}

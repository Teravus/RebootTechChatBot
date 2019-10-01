﻿using System;
using TwitchLib.Client.Models;

namespace TwitchLib.Client.Events
{
    /// <inheritdoc />
    /// <summary>Args representing message received event.</summary>
    public class OnMessageReceivedArgs : EventArgs
    {
        /// <summary>Property representing received chat message.</summary>
        public ChatMessage ChatMessage;
    }
}

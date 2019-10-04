﻿using Newtonsoft.Json;

namespace TwitchLib.Api.V5.Models.Search
{
    public class SearchChannels
    {
        #region Total
        [JsonProperty(PropertyName = "_total")]
        public int Total { get; protected set; }
        #endregion
        #region Channels
        [JsonProperty(PropertyName = "channels")]
        public Channels.Channel[] Channels { get; protected set; }
        #endregion
    }
}

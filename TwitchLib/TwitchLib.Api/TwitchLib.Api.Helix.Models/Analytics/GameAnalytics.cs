﻿using Newtonsoft.Json;

namespace TwitchLib.Api.Helix.Models.Analytics
{
    public class GameAnalytics
    {
        [JsonProperty(PropertyName = "game_id")]
        public string GameId { get; protected set; }
        [JsonProperty(PropertyName = "URL")]
        public string Url { get; protected set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; protected set; }
        [JsonProperty(PropertyName = "date_range")]
        public Common.DateRange DateRange { get; protected set; }
    }
}

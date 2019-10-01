﻿using Newtonsoft.Json;

namespace TwitchLib.Api.Helix.Models.StreamsMetadata
{
    public class HeroHearthstone
    {
        [JsonProperty(PropertyName = "class")]
        public string Class { get; protected set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; protected set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; protected set; }
    }
}

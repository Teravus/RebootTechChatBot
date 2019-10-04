﻿using Newtonsoft.Json;

namespace TwitchLib.Api.Core.Models.Undocumented.ClipChat
{
    public class ReChatMessage
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; protected set; }
        [JsonProperty(PropertyName = "id")]
        public string Id { get; protected set; }
        [JsonProperty(PropertyName = "attributes")]
        public ReChatMessageAttributes Attributes { get; protected set; }
    }
}

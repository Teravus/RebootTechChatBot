﻿using Newtonsoft.Json;

namespace TwitchLib.Api.V5.Models.Chat
{
    public class ChatRoom
    {
        [JsonProperty(PropertyName = "_id")]
        public string Id { get; protected set; }
        [JsonProperty(PropertyName = "owner_id")]
        public string OwnerId { get; protected set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; protected set; }
        [JsonProperty(PropertyName = "topic")]
        public string Topic { get; protected set; }
        [JsonProperty(PropertyName = "is_previewable")]
        public bool IsPreviewable { get; protected set; }
        [JsonProperty(PropertyName = "minimum_allowed_role")]
        public string MinimumAllowedRole { get; protected set; }
    }
}

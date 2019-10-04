﻿using Newtonsoft.Json;

namespace TwitchLib.Api.V5.Models.UploadVideo
{
    public class Preview
    {
        [JsonProperty(PropertyName = "small")]
        public string Small { get; protected set; }
        [JsonProperty(PropertyName = "medium")]
        public string Medium { get; protected set; }
        [JsonProperty(PropertyName = "large")]
        public string Large { get; protected set; }
        [JsonProperty(PropertyName = "template")]
        public string Template { get; protected set; }
    }
}

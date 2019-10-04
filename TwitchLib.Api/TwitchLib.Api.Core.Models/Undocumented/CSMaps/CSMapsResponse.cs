﻿using Newtonsoft.Json;

namespace TwitchLib.Api.Core.Models.Undocumented.CSMaps
{
    public class CSMapsResponse
    {
        [JsonProperty(PropertyName = "_total")]
        public int Total { get; protected set; }
        [JsonProperty(PropertyName = "maps")]
        public Map[] Maps { get; protected set; }
    }
}

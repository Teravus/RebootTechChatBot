using Newtonsoft.Json;

namespace TwitchLib.Api.Core.Models.Undocumented.Comments
{
    public class UserBadges
    {
        [JsonProperty(PropertyName = "_id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "version")]
        public string Version { get; set; }
    }
}
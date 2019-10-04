using Newtonsoft.Json;

namespace TwitchLib.Api.Core.Models.Undocumented.Comments
{
    public class Fragment
    {
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }
        [JsonProperty(PropertyName = "emoticon")]
        public Emoticon Emoticon { get; set; }
    }
}
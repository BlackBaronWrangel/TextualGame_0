using GlobalServices.Interfaces;
using Newtonsoft.Json;

namespace GlobalServices.Entities
{
    public class Tag : ITag
    {
        [JsonProperty]
        public string Id { get; set; } = string.Empty;
        [JsonProperty]
        public string Name { get; set; } = string.Empty;
        [JsonProperty]
        public string Description { get; set; } = string.Empty;
        public override string ToString() => Id;
    }
}

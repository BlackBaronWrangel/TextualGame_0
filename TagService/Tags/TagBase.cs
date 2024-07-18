using GlobalServices.Enums;
using GlobalServices.Interfaces;
using Newtonsoft.Json;

namespace GlobalServices.Tags
{
    public class TagBase : ITag
    {
        [JsonProperty]
        public string Id { get; set; } = string.Empty;
        [JsonProperty]
        public string Name { get; set; } = string.Empty;
        [JsonProperty]
        public string Description { get; set; } = string.Empty;
        public virtual TagType TagType { get; set; }
        public override string ToString() => Id;
    }
}

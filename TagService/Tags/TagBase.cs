using GlobalServices.Enums;
using GlobalServices.Interfaces;
using Newtonsoft.Json;

namespace GlobalServices.Tags
{
    public class TagBase : ITag
    {
        [JsonProperty]
        public string Id { get; protected set; } = string.Empty;
        [JsonProperty]
        public string Name { get; protected set; } = string.Empty;
        [JsonProperty]
        public string Description { get; protected set; } = string.Empty;
        public virtual TagType TagType { get; protected set; }
        public override string ToString() => Id;
    }
}

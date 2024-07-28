using GlobalServices.Interfaces;
using Newtonsoft.Json;

namespace GlobalServices.Entities
{
    public class Location : TaggableBase
    {
        [JsonProperty]
        public string Id { get; protected set; } = string.Empty;
        [JsonProperty]
        public string Name { get; protected set; } = string.Empty;
        [JsonProperty]
        public string Description { get; protected set; } = string.Empty;
        [JsonProperty]
        public HashSet<Connection> ConnectedLocations { get; protected set; } = new ();
        public Location(string id, string name, string description, HashSet<Connection> connections, HashSet<string> tags) : base(tags)
        {
            Id = id;
            Name = name;
            Description = description;
            ConnectedLocations = connections;
        }
        public Location(string id, string name, string description, HashSet<string> tags) : base(tags)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public Location(string id, string name, string description) : base()
        {
            Id = id;
            Name = name;
            Description = description;
        }
        [Newtonsoft.Json.JsonConstructor]
        public Location() : base() { }
        public override string ToString() => $"{GetType().Name} [{Id}, {Name}]";
    }
}

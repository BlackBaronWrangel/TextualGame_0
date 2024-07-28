using GlobalServices.Enums;

namespace GlobalServices.Entities
{
    public class Event : TaggableBase
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public EventType EventType { get; protected set; } = EventType.Default;
        public string LocationId { get; protected set; } = string.Empty;
        public string EventeDescription { get; set; } = string.Empty;
        public HashSet<string> CharacterIds { get; set; } = new();
        public HashSet<string> ItemIds { get; set; } = new();
        public Dictionary<string, string> PossibleNextEvents { get; set; } = new();
        public List<string> Commands { get; set; } = new();

        public Event() : base() { }
        public Event(string id, EventType type, string locationId ) : base()
        {
            Id = id;
            EventType = type;
            LocationId = locationId;
        }
        public Event(string id, EventType type, string locationId, HashSet<string> characters, HashSet<string> items, Dictionary<string, string> nextEvents) : base()
        {
            Id = id;
            EventType = type;
            LocationId = locationId;
            CharacterIds = characters;
            ItemIds = items;
            PossibleNextEvents = nextEvents;
        }
        public Event(string id, EventType type, string locationId, HashSet<string> characters, HashSet<string> items, Dictionary<string, string> nextEvents, HashSet<string> tags) : base(tags)
        {
            Id = id;
            EventType = type;
            LocationId = locationId;
            CharacterIds = characters;
            ItemIds = items;
            PossibleNextEvents = nextEvents;
        }
        public override string ToString() => $"{GetType().Name} [{Id}, {EventType}, {LocationId}]";
    }
}

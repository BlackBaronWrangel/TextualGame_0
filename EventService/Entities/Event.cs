using GlobalServices.Enums;
using GlobalServices.Interfaces;

namespace GlobalServices.Entities
{
    public class Event : EventBase
    {
        private Guid _id { get; } = Guid.NewGuid();
        public string Id { get => _id.ToString(); }
        public EventType EventType { get; protected set; } = EventType.Default;
        public string LocationId { get; protected set; } = string.Empty;
        public HashSet<string> CharacterIds { get; protected set; } = new();
        public HashSet<string> ItemIds { get; protected set; } = new();

        public Event() : base() { }
        public Event(EventType type, string locationId, HashSet<string> characters, HashSet<string> items) : base()
        {
            EventType = type;
            LocationId = locationId;
            CharacterIds = characters;
            ItemIds = items;
        }
        public Event(EventType type, string locationId, HashSet<string> characters, HashSet<string> items, HashSet<ITag> tags) : base(tags)
        {
            EventType = type;
            LocationId = locationId;
            CharacterIds = characters;
            ItemIds = items;
        }
        public override string ToString() => $"{GetType().Name} [{Id}, {EventType}, {LocationId}]";
    }
}

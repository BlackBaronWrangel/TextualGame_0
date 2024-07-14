using GlobalServices.Entities;
using GlobalServices.Enums;

namespace GlobalServices.Interfaces
{
    public interface IEventService
    {
        HashSet<Event> Events { get; }
        Event? GetEvent(string eventId);
        Event CreateEvent(string eventId, string locationId, EventType type, HashSet<string> characterIds, HashSet<string> itemIds, HashSet<string> nextEvents);
        void RegisterEvent(Event gameEvent);
        void AddTag(string eventId, ITag tag);
        void AddTag(string eventId, TagId.EventTagId tag);
        void RemoveTag(string eventId, ITag tag);
        void RemoveTag(string eventId, TagId.EventTagId tag);
    }
}

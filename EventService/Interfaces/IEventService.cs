using GlobalServices.Entities;
using GlobalServices.Enums;

namespace GlobalServices.Interfaces
{
    public interface IEventService
    {
        HashSet<Event> Events { get; }
        Event? GetEvent(string eventId);
        Event CreateEvent(string eventId, string locationId, EventType type, HashSet<string> characterIds, HashSet<string> itemIds, Dictionary<string, string> nextEvents);
        Event CreateDefaultLocationEvent(string locationId);
        void RegisterEvent(Event gameEvent);
        void AddTag(string eventId, string tag);
        void RemoveTag(string eventId, string tag);
    }
}

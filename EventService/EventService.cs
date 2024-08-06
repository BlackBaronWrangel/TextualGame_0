using GlobalServices.Entities;
using GlobalServices.Enums;
using GlobalServices.Interfaces;

namespace GlobalServices
{
    public class EventService : IEventService
    {
        private ILogger _logger;
        private ITagService _tagService;
        private ILocationService _locationService;
        public HashSet<Event> Events { get; protected set; } = new();

        public EventService(ILogger logger, ITagService tagService, ILocationService locationService)
        {
            _logger = logger;
            _tagService = tagService;
            _locationService = locationService;
        }
        public Event? GetEvent(string eventId) => Events.FirstOrDefault(e => e.Id == eventId);
        public Event CreateEvent(string eventId, string locationId, EventType type, HashSet<string> characterIds, HashSet<string> itemIds, Dictionary<string, string> nextEvents)
        {
            var gameEvent = new Event(eventId, type, locationId, characterIds, itemIds, nextEvents);
            RegisterEvent(gameEvent);
            return gameEvent;
        }
        public Event CreateDefaultEvent()
        {
            var gameEvent = new Event();
            RegisterEvent(gameEvent);
            return gameEvent;
        }
        public Event CreateDefaultLocationEvent(string locationId)
        {
            var gameEvent = new Event(locationId, EventType.Transition, locationId);
            RegisterEvent(gameEvent);
            return gameEvent;
        }

        public void RegisterEvent(Event gameEvent)
        {
            if (Events.Any(e => e.Id == gameEvent.Id))
            {
                _logger.LogWarning($"Attempt to register existing event with id {gameEvent.Id}");
                return;
            }
            Events.Add(gameEvent);
            _logger.LogInfo($"Registered {gameEvent}");
            _tagService.RegisterITaggable(gameEvent);
        }
        public void AddTag(string eventId, string tag)
        {
            var gameEvent = GetEvent(eventId);
            if (gameEvent is null)
            {
                _logger.LogWarning($"Attempt to add a tag to unexisting {eventId}");
                return;
            }
            gameEvent.AddTag(tag);
        }
        public void RemoveTag(string eventId, string tag)
        {
            var gameEvent = GetEvent(eventId);
            if (gameEvent is not null)
                gameEvent.RemoveTag(tag);
            else
                _logger.LogWarning($"Can't get {eventId} to remove tag {tag}");
        }
    }
}

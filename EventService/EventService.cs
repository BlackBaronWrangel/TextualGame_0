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
            UpdateNavigationEvents();
        }
        public Event? GetEvent(string eventId)
        {
            return Events.FirstOrDefault(e => e.Id == eventId);
        }
        public Event CreateEvent(string eventId, string locationId, EventType type, HashSet<string> characterIds, HashSet<string> itemIds, HashSet<string> nextEvents)
        {
            var gameEvent = new Event(eventId, type, locationId, characterIds, itemIds, nextEvents);
            Events.Add(gameEvent);
            _logger.LogInfo($"Created {gameEvent}");
            _tagService.RegisterITaggable(gameEvent);
            return gameEvent;
        }
        public Event CreateDefaultLocationEvent(string locationId)
        {
            var gameEvent = new Event(locationId, EventType.Transition, locationId);
            Events.Add(gameEvent);
            _logger.LogInfo($"Created {gameEvent}");
            _tagService.RegisterITaggable(gameEvent);
            return gameEvent;
        }

        public Event? GetNavigationEvent(string locationId)
        {
            UpdateNavigationEvents();
            var gameEvent = Events.Where(e => e.LocationId == locationId).FirstOrDefault();
            return gameEvent;
        }

        public void AddTag(string eventId, ITag tag)
        {
            var gameEvent = GetEvent(eventId);
            if (gameEvent is null)
            {
                _logger.LogWarning($"Attempt to add a tag to unexisting {eventId}");
                return;
            }
            gameEvent.AddTag(tag);
        }
        public void AddTag(string eventId, TagId.EventTagId tagId)
        {
            var tag = _tagService.GetEventTag(tagId);
            if (tag != null)
                AddTag(eventId, tag);
            else
                _logger.LogError($"Can't add tag {tag} to the {eventId}");
        }
        public void RemoveTag(string eventId, ITag tag)
        {
            var gameEvent = GetEvent(eventId);
            if (gameEvent is not null)
                gameEvent.RemoveTag(tag);
            else
                _logger.LogWarning($"Can't get {eventId} to remove tag {tag}");
        }
        public void RemoveTag(string eventId, TagId.EventTagId tagId)
        {
            var tag = _tagService.GetEventTag(tagId);
            if (tag != null)
                RemoveTag(eventId, tag);
            else
                _logger.LogError($"Can't remove tag {tag} from the {eventId}");
        }

        private void UpdateNavigationEvents()
        {
            foreach (var loc in _locationService.Locations)
            {
                var gameEvent = CreateDefaultLocationEvent(loc.Id);
                foreach (var connection in loc.ConnectedLocations)
                {
                    gameEvent.PossibleNextEvents.Add(connection.LocationId);
                }
            }
        }
    }
}

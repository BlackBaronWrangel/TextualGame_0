﻿using GlobalServices.Entities;
using GlobalServices.Enums;
using GlobalServices.Interfaces;

namespace GlobalServices
{
    public class EventService : IEventService
    {
        private ILogger _logger;
        private ITagService _tagService;

        public HashSet<Event> Events { get; protected set; } = new();

        public EventService(ILogger logger, ITagService tagService) 
        {
            _logger = logger;
            _tagService = tagService;
        }
        public Event? GetEvent(string eventId)
        {
            return Events.FirstOrDefault(e => e.Id == eventId);
        }
        public Event CreateEvent(string locationId, EventType type, HashSet<string> characterIds, HashSet<string> itemIds)
        {
            var gameEvent = new Event(type, locationId, characterIds, itemIds);
            Events.Add(gameEvent);
            _logger.LogInfo($"Created {gameEvent.Id}");
            _tagService.RegisterITaggable(gameEvent);
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
        public void AddTag(string eventId, TagId.Event tagId)
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
        public void RemoveTag(string eventId, TagId.Event tagId)
        {
            var tag = _tagService.GetEventTag(tagId);
            if (tag != null)
                RemoveTag(eventId, tag);
            else
                _logger.LogError($"Can't remove tag {tag} from the {eventId}");
        }
    }
}
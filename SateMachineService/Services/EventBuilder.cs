using AutoMapper;
using GlobalServices.Entities;
using GlobalServices.Enums;
using GlobalServices.Interfaces;

namespace GlobalServices
{
    public class EventBuilder : IEventBuilder
    {
        private IEventService _eventService;
        private ICharacterService _characterService;
        private ILogger _logger;
        private ITagService _tagService;
        private IItemService _itemService;
        private ILocationService _locationService;
        private ICommandHandler _commandHandler;
        private IMapper _mapper;

        public EventBuilder(
            ILogger logger,
            IEventService eventService,
            ITagService tagService,
            IItemService itemService,
            ICharacterService characterService,
            ILocationService locationService,
            ICommandHandler commandHandler,
            IMapper mapper
            )
        {
            _logger = logger;
            _eventService = eventService;
            _tagService = tagService;
            _itemService = itemService;
            _locationService = locationService;
            _characterService = characterService;
            _mapper = mapper;
            _commandHandler = commandHandler;
        }

        public KeyValuePair<string,string> BuildConfrontationEvent(Event currentEvent)
        {
            var newEvent = _eventService.CreateDefaultEvent();

            newEvent.LocationId = currentEvent.LocationId;
            newEvent.EventType = EventType.Confrontation;
            newEvent.CharacterIds = new(currentEvent.CharacterIds);
            newEvent.ItemIds = new(currentEvent.ItemIds);

            var description = string.Empty;

            description += ProcessCharactersBehaviors(newEvent);

            newEvent.EventeDescription = description;

            var entryText = "Monsters are going to attack you";
            return new KeyValuePair<string, string>(entryText ,newEvent.Id);
        }

        private string ProcessCharactersBehaviors(Event newEvent)
        {
            var description = string.Empty;

            description += ProcessCiviliansBehaviors(newEvent);
            description += ProcessMonstersBehaviors(newEvent);
            return description;
        }

        private string ProcessCiviliansBehaviors(Event newEvent)
        {
            var description = string.Empty;

            var characters = newEvent.CharacterIds.Select(id => _characterService.GetCharacter(id)).ToList();
            foreach (var character in characters.Where(c => c is not null && c.Type == CharacterType.Civilian))
            {
                description += $"[url={character!.Id}]{character!.BodyType} {character!.Gender} {character!.Species}[/url] scattered in terror horrified at what was about to happen.\n";
                if (character.Persistence is not CharacterPersistence.Temporary)
                {
                    var possibleLocations = _locationService.GetConnections(newEvent.LocationId)?.Select(c => c.LocationId);
                    if (possibleLocations is not null && possibleLocations.Any())
                        _characterService.MoveCharacter(character.Id, RandomSelector.Select(possibleLocations)!);
                }

                newEvent.CharacterIds.Remove(character.Id);
            }
            return description;
        }
        private string ProcessMonstersBehaviors(Event newEvent)
        {
            var description = string.Empty;

            var characters = newEvent.CharacterIds.Select(id => _characterService.GetCharacter(id)).ToList();
            foreach (var character in characters.Where(c => c is not null && c.Type == CharacterType.Monster))
            {
                description += $"[url={character!.Id}]{character!.BodyType} {character!.Gender} {character!.Species}[/url] approaches you with grim intentions.\n";
            }
            return description;
        }
    }
}

using AutoMapper;
using GlobalServices.Entities;
using GlobalServices.Interfaces;

namespace GlobalServices
{
    public class StateMachine : IStateMachine
    {
        private const string _defaultJsonScenesPath = "Resources/Scenes";
        public HashSet<Scene> Scenes { get; set; } = new HashSet<Scene>();
        public Event? CurrentState { get; protected set; } = null;

        private IEventService _eventService;
        private ICharacterService _characterService;
        private ILogger _logger;
        private ITagService _tagService;
        private IItemService _itemService;
        private ILocationService _locationService;
        private ICommandHandler _commandHandler;
        private IMapper _mapper;

        public event EventHandler StateChanged = delegate { };

        public StateMachine(
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
            InitPreDefinedScenes();
        }
        public void RunScene(string sceneId)
        {
            var scene = Scenes.Where(s => s.Id == sceneId).FirstOrDefault();
            if (scene is null)
            {
                _logger.LogError($"Can't get scene {sceneId} to run.");
                return;
            }
            RunEvent(scene.StartEventId);
        }

        public void NextState(string eventId)
        {
            if (CurrentState is null)
                _logger.LogWarning("Attempt to run the next state from state which is null.");
            if (CurrentState is not null && !CurrentState.PossibleNextEvents.Values.Contains(eventId))
                _logger.LogWarning($"Attempt to run the next state {eventId}, which is not defined in the list of PossibleNextEvents.");

            if (CurrentState is not null)
            {
                CleanEventTemporalItems(CurrentState);
                CleanEventTemporalCharacters(CurrentState);
            }
            RunEvent(eventId);
        }

        private void RunEvent(string eventId)
        {
            var gameEvent = _eventService.GetEvent(eventId);
            if (gameEvent is null)
            {
                _logger.LogError($"Can't get event {eventId} to set it as a new state.");
                return;
            }

            foreach (var command in gameEvent.Commands)
                _commandHandler.ExecuteEventCommand(gameEvent, command);
            LoadExistingCharactersInEvent(gameEvent);

            CurrentState = gameEvent;
            OnStateChanged();
            _logger.LogInfo($"Running {CurrentState}");
        }

        private void RegisterScene(Scene scene)
        {
            if (Scenes.Any(s => s.Id == scene.Id))
            {
                _logger.LogWarning($"Attempt to register existing scene with id {scene.Id}");
                return;
            }

            Scenes.Add(scene);
            _logger.LogInfo($"Registered {scene}");
        }
        private void InitPreDefinedScenes()
        {
            var jsonScenes = GameEntitiesJsonLoader.ReadFolderAsCollection<JsonScene>(_defaultJsonScenesPath);
            if (jsonScenes is null)
            {
                _logger.LogError($"Can't load default scenes from json.");
                return;
            }
            foreach (var jsonScene in jsonScenes.ToList())
            {
                foreach (var jsonEvent in jsonScene.Events)
                {
                    var gameEvent = _mapper.Map<Event>(jsonEvent);
                    _eventService.RegisterEvent(gameEvent);
                }
                var scene = _mapper.Map<Scene>(jsonScene);

                RegisterScene(scene);
            }
        }
        private void CleanEventTemporalCharacters(Event gameEvent)
        {
            foreach (var charId in gameEvent.CharacterIds)
            {
                var character = _characterService.GetCharacter(charId);
                if (character is null)
                {
                    _logger.LogWarning($"Attempt to delete unexisting character {charId} during cleaning event entities.");
                    continue;
                }
                if (character.Persistence == Enums.CharacterPersistence.Temporary)
                {
                    _characterService.RemoveCharacter(charId);
                    gameEvent.CharacterIds.Remove(charId);
                }
            }
        }
        private void CleanEventTemporalItems(Event gameEvent)
        {
            foreach (var itemId in gameEvent.ItemIds)
            {
                var item = _itemService.GetItem(itemId);
                if (item is null)
                {
                    _logger.LogWarning($"Attempt to delete unexisting item {itemId} during cleaning event entities.");
                    continue;
                }
                if (item.Persistence == Enums.ItemPersistence.Temporary)
                {
                    _itemService.RemoveItem(item.Id);
                    gameEvent.ItemIds.Remove(itemId);
                }
            }
        }
        private void LoadExistingCharactersInEvent(Event gameEvent)
        {
            var relatedCharacters = _characterService.Characters.Where(c => c.Location == gameEvent.LocationId).ToList();
            if (relatedCharacters is not null && relatedCharacters.Count != 0)
            {
                foreach (var character in relatedCharacters)
                    gameEvent.CharacterIds.Add(character.Id);
            }
        }

        protected virtual void OnStateChanged() => StateChanged?.Invoke(this, EventArgs.Empty);
        
    }
}

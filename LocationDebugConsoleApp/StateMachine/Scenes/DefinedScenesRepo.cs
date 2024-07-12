using DebugConsoleApp.StateMachine.Entities;
using GlobalServices.Interfaces;

namespace DebugConsoleApp.StateMachine.Scenes
{
    public partial class DefinedScenesRepo
    {
        private static ITagService? _tagService;
        private static ILocationService? _locationService;
        private static ICharacterService? _characterService;
        private static IItemService? _itemService;
        private static IEventService? _eventService;
        private static ILogger? _logger;
        public DefinedScenesRepo(
            ILogger logger, 
            ITagService tagService, 
            ILocationService locationService, 
            ICharacterService characterService, 
            IItemService itemService, 
            IEventService eventService            
            )
        {
            _logger = logger;
            _tagService = tagService;
            _locationService = locationService;
            _characterService = characterService;
            _itemService = itemService;
            _eventService = eventService;
        }

        public HashSet<Scene> Scenes { get => GetDefinedScenes(); }

        private HashSet<Scene> GetDefinedScenes()
        {
            var scenes = new HashSet<Scene>
            {
                TestingScene_0(),
            };


            return scenes;
        }

    }
}

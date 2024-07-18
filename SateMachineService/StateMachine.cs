using AutoMapper;
using GlobalServices.Entities;
using GlobalServices.Interfaces;

namespace GlobalServices
{
    public class StateMachine : IStateMachine
    {
        public HashSet<Scene> Scenes { get; set; } = new HashSet<Scene>();
        public Event? CurrentState { get; protected set; } = null;

        private IEventService _eventService;
        private ILogger _logger;
        private ITagService _tagService;
        private ICommandHandler _commandHandler;
        private IMapper _mapper;
        public StateMachine(
            ILogger logger,
            IEventService eventService,
            ITagService tagService,
            ICommandHandler commandHandler,
            IMapper mapper
            )
        {
            _logger = logger;
            _eventService = eventService;
            _tagService = tagService;
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
            CurrentState = _eventService.GetEvent(scene.StartEventId);
            if (CurrentState is null)
            {
                _logger.LogError($"Can't get event {sceneId} to set it as current state.");
                return;
            }

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
            var jsonScenes = SceneJsonReader.ReadScenesFromJson();
            foreach (var jsonScene in jsonScenes)
            {
                foreach (var jsonEvent in jsonScene.Events)
                {
                    var gameEvent = _mapper.Map<Event>(jsonEvent);

                    foreach (var command in jsonEvent.Commands)
                        _commandHandler.ExecuteEventCommand(gameEvent, command);

                    _eventService.RegisterEvent(gameEvent);
                }
                var scene = _mapper.Map<Scene>(jsonScene);


                RegisterScene(scene);
            }
        }
    }
}

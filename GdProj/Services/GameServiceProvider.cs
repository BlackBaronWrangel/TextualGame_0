using AutoMapper;
using GlobalServices;
using GlobalServices.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GdProj.Services
{
    public class GameServiceProvider
    {
        private static readonly Lazy<GameServiceProvider> _instance = new Lazy<GameServiceProvider>(() => new GameServiceProvider());

        private ServiceCollection _serviceCollection;
        private ServiceProvider _serviceProvider;

        public ITagService TagService;
        public ILocationService LocationService;
        public ICharacterService CharacterService;
        public IItemService ItemService;
        public IEventService EventService;
        public ILogger Logger;
        public IStateMachine StateMachine;
        public IEventBuilder EventBuilder;
        public ICommandHandler CommandHandler;
        public IMapper Mapper;
        private GameServiceProvider()
        {
            ConfigureServices();
            TagService = _serviceProvider!.GetRequiredService<ITagService>();
            LocationService = _serviceProvider!.GetRequiredService<ILocationService>();
            Logger = _serviceProvider!.GetRequiredService<ILogger>();
            CharacterService = _serviceProvider!.GetRequiredService<ICharacterService>();
            ItemService = _serviceProvider!.GetRequiredService<IItemService>();
            EventService = _serviceProvider!.GetRequiredService<IEventService>();
            StateMachine = _serviceProvider!.GetRequiredService<IStateMachine>();
            EventBuilder = _serviceProvider!.GetRequiredService<IEventBuilder>();
            CommandHandler = _serviceProvider!.GetRequiredService<ICommandHandler>();
            Mapper = _serviceProvider!.GetRequiredService<IMapper>();
        }
        public static GameServiceProvider Instance => _instance.Value;

        private void ConfigureServices()
        {
            _serviceCollection = new ServiceCollection();

            _serviceCollection.AddSingleton<ITagService, TagService>();
            _serviceCollection.AddSingleton<ILocationService, LocationService>();
            _serviceCollection.AddSingleton<ICharacterService, CharacterService>();
            _serviceCollection.AddSingleton<IEventService, EventService>();
            _serviceCollection.AddSingleton<ILogger, Logger>();
            _serviceCollection.AddSingleton<IItemService, ItemService>();
            _serviceCollection.AddSingleton<IEventBuilder, EventBuilder>();
            _serviceCollection.AddSingleton<IStateMachine, StateMachine>();
            _serviceCollection.AddSingleton<ICommandHandler, CommandHandler>();
            _serviceCollection.AddAutoMapper(typeof(MappingProfile));

            _serviceProvider = _serviceCollection.BuildServiceProvider();
        }
    }
}

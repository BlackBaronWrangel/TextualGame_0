//Place code for testing and debugging here.
using AutoMapper;
using GlobalServices;
using GlobalServices.Enums;
using GlobalServices.Interfaces;
using Microsoft.Extensions.DependencyInjection;

internal class Program
{
    private static ServiceCollection? _serviceCollection;
    private static ServiceProvider? _serviceProvider;
    private static ITagService? _tagService ;
    private static ILocationService? _locationService; 
    private static ICharacterService? _characterService;
    private static IItemService? _itemService;
    private static IEventService? _eventService;
    private static ILogger? _logger;
    private static IStateMachine? _stateMachine;
    private static ICommandHandler? _commandHandler;
    private static IMapper? _mapper;
    private static void Main(string[] args)
    {
        ConfigureServices();
        _tagService = _serviceProvider!.GetRequiredService<ITagService>();
        _locationService = _serviceProvider!.GetRequiredService<ILocationService>();
        _logger = _serviceProvider!.GetRequiredService<ILogger>();
        _characterService = _serviceProvider!.GetRequiredService<ICharacterService>();
        _itemService = _serviceProvider!.GetRequiredService<IItemService>();
        _eventService = _serviceProvider!.GetRequiredService<IEventService>();
        _stateMachine = _serviceProvider!.GetRequiredService<IStateMachine>();
        _commandHandler = _serviceProvider!.GetRequiredService<ICommandHandler>();
        _mapper = _serviceProvider!.GetRequiredService<IMapper>();
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //_locationService.AddConnection("loc_swamp", "loc_forest", 0.5);
        //_locationService.RemoveConnection("loc_forest", "loc_meadow");

        //var c1 = _characterService.CreateRandomCharacter(CharacterType.Monster, CharacterPersistence.Temporary);
        //var c2 = _characterService.CreateRandomCharacter(CharacterType.Other, CharacterPersistence.Permanent);
        //var c3 = _characterService.CreateDefaultCharacter();
        //var c4 = _characterService.CreateDefaultCharacter();
        //var c5 = _characterService.CreateRandomMonster();
        //var c6 = _characterService.CreateRandomTemporalCivilian();
        //var c7 = _characterService.CreateRandomPermanentCivilian();
        //var c8 = _characterService.CreateMainCharacter("Buba", CharacterBodyType.Medium, CharacterGender.Female, CharacterSpecies.Horse);

        //_characterService.AddTag(c1.Id, TagId.CharacterTagId.Hostile);
        //_characterService.AddTag(c2.Id, TagId.CharacterTagId.Friendly);
        //_characterService.AddTag(c3.Id, TagId.CharacterTagId.Hostile);

        //var chars = _characterService.Characters;
        //var locs = _locationService.Locations;
        //var player = _characterService.GetPlayer();

        //_characterService.MoveCharacter(player.Id, "loc_swamp");
        //_characterService.MoveCharacter(player.Id, "loc_forest");
        //_characterService.MoveCharacter(player.Id, "loc_meadow");
        //_characterService.MoveCharacter(player.Id, "loc_forest");

        //var apple = _itemService.CreateItem("Apple", ItemType.Food);
        //var banana = _itemService.CreateItem("Banana", ItemType.Food);
        //var sword = _itemService.CreateItem("Sword of the power", ItemType.Artifact);
        //_itemService.CreateItem("Diamond", ItemType.Trinket);
        //var diamond = _itemService.GetItemByName("Diamond");
        //_characterService.AssignItem(diamond.Id, player.Id);
        //_characterService.AssignItem(apple.Id, player.Id);
        //_characterService.AssignItem(apple.Id, chars.ToList()[0].Id);
        //_characterService.UnAssignItem(apple.Id, player.Id);
        //apple = null;

        //_eventService.CreateEvent(locs.ToList()[2].Id, EventType.Default, new() { chars.ToList()[0].Id, chars.ToList()[2].Id, chars.ToList()[3].Id }, new() {banana.Id, diamond.Id, sword.Id}, new());
        //var events = _eventService.Events;

        //_tagService.ValidateITaggables();

        //Examples of how to get any ITaggable entity in the game from the tagService:
        //var characterEntities = _tagService.TaggableEntities.Where(i => i is Character).Cast<Character>().ToHashSet();
        //var itemEntities = _tagService.TaggableEntities.Where(i => i is Item).Cast<Item>().ToHashSet(); 

        //var scenes = _scenesRepo.Scenes.ToList();
        //var event1 = _eventService.GetEvent(scenes[0].StartEventId);
        //var event2 = _eventService.GetEvent(event1.PossibleNextEvents.ToList()[1]);
        //var event3 = _eventService.GetEvent(event2.PossibleNextEvents.ToList()[0]);
        //var eventAfterExit = _eventService.GetEvent(event3.PossibleNextEvents.ToList()[0]);

        _stateMachine.RunScene("TestingScene_0");
    }

    private static void ConfigureServices()
    {
        _serviceCollection = new ServiceCollection();

        _serviceCollection.AddSingleton<ITagService, TagService>();
        _serviceCollection.AddSingleton<ILocationService, LocationService>();
        _serviceCollection.AddSingleton<ICharacterService, CharacterService>(); 
        _serviceCollection.AddSingleton<IEventService, EventService>();
        _serviceCollection.AddSingleton<ILogger, Logger>();
        _serviceCollection.AddSingleton<IItemService, ItemService>();
        _serviceCollection.AddSingleton<IStateMachine, StateMachine>();
        _serviceCollection.AddSingleton<ICommandHandler, CommandHandler>();
        _serviceCollection.AddAutoMapper(typeof(MappingProfile));

        _serviceProvider = _serviceCollection.BuildServiceProvider();
    }
}
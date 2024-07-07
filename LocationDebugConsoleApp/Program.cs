//Place code for testing and debugging here.
using GlobalServices;
using GlobalServices.Enums;
using GlobalServices.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using GlobalServices.Entities;

internal class Program
{
    private static ServiceCollection? _serviceCollection;
    private static ServiceProvider? _serviceProvider;
    private static ITagService? _tagService ;
    private static ILocationService? _locationService; 
    private static ICharacterService? _characterService;
    private static ICharacterFactory? _characterFactory;
    private static IItemFactory? _itemFactory;
    private static IItemService? _itemService;
    private static IEventService? _eventService;
    private static ILogger? _logger;
    private static void Main(string[] args)
    {
        ConfigureServices();
        _tagService = _serviceProvider!.GetRequiredService<ITagService>();
        _locationService = _serviceProvider!.GetRequiredService<ILocationService>();
        _logger = _serviceProvider!.GetRequiredService<ILogger>();
        _characterFactory = _serviceProvider!.GetRequiredService<ICharacterFactory>();
        _characterService = _serviceProvider!.GetRequiredService<ICharacterService>();
        _itemFactory = _serviceProvider!.GetRequiredService<IItemFactory>();
        _itemService = _serviceProvider!.GetRequiredService<IItemService>();
        _eventService = _serviceProvider!.GetRequiredService<IEventService>();
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        _locationService.AddConnection("L001", "L003", 0.5);
        _locationService.RemoveConnection("L003", "L002");

        _characterService.CreateRandomCharacter(CharacterType.Monster, CharacterPersistence.Temporary).AddTag(_tagService.GetCharacterTag(TagId.Character.Neutral)) ;
        _characterService.CreateRandomCharacter(CharacterType.Other, CharacterPersistence.Permanent);
        _characterService.CreateDefaultCharacter();
        _characterService.CreateDefaultCharacter();
        _characterService.CreateRandomMonster();
        _characterService.CreateRandomTemporalCivilian();
        _characterService.CreateRandomPermanentCivilian();
        _characterService.CreateMainCharacter("Biba", CharacterBodyType.Medium, CharacterGender.Female, CharacterSpecies.Horse);

        var chars = _characterService.Characters;
        var locs = _locationService.Locations;
        var player = _characterService.GetPlayer();

        _characterService.MoveCharacter(player, "L002");
        _characterService.MoveCharacter(player, "L003");
        _characterService.MoveCharacter(player, "L001");
        _characterService.MoveCharacter(player, "L002");

        var apple = _itemService.CreateItem("Apple", ItemType.Food);
        var banana = _itemService.CreateItem("Banana", ItemType.Food);
        var sword = _itemService.CreateItem("Sword of the power", ItemType.Artifact);
        _itemService.CreateItem("Diamond", ItemType.Trinket);
        var diamond = _itemService.GetItemByName("Diamond");
        _characterService.AssignItem(diamond.Id, player.Id);
        _characterService.AssignItem(apple.Id, player.Id);
        _characterService.AssignItem(apple.Id, chars.ToList()[0].Id);
        _characterService.UnAssignItem(apple.Id, player.Id);
        apple = null;


        _eventService.CreateEvent(locs.ToList()[2].Id, EventType.Default, new() { chars.ToList()[0].Id, chars.ToList()[2].Id, chars.ToList()[3].Id }, new() {banana.Id, diamond.Id, sword.Id});
        var events = _eventService.Events;

        _tagService.ValidateITaggables();

        //Examples of how to get any ITaggable entity in the game from the tagService:
        var characterEntities = _tagService.TaggableEntities.Where(i => i is Character).Cast<Character>().ToHashSet();
        _itemService.RemoveItem(_itemService.Items.ToList()[0].Id);
        var itemEntities = _tagService.TaggableEntities.Where(i => i is Item).Cast<Item>().ToHashSet(); 
    }

    private static void ConfigureServices()
    {
        _serviceCollection = new ServiceCollection();

        _serviceCollection.AddSingleton<ITagService, TagService>();
        _serviceCollection.AddSingleton<ILocationService, LocationService>();
        _serviceCollection.AddSingleton<ICharacterService, CharacterService>(); 
        _serviceCollection.AddSingleton<IEventService, EventService>();
        _serviceCollection.AddSingleton<ICharacterFactory, CharacterFactory>();
        _serviceCollection.AddSingleton<ILogger, Logger>();
        _serviceCollection.AddSingleton<IItemFactory, ItemFactory>();
        _serviceCollection.AddSingleton<IItemService, ItemService>();

        _serviceProvider = _serviceCollection.BuildServiceProvider();
    }
}
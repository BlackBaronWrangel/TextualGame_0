//Place code for testing and debugging here.
using GlobalServices;
using GlobalServices.Enums;
using GlobalServices.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using tags = GlobalServices.Enums.TagId;
using loc = GlobalServices.Entities.DefaultMap.LocationId;

internal class Program
{
    private static ServiceCollection _serviceCollection;
    private static ServiceProvider _serviceProvider;
    private static ITagService _tagService ;
    private static ILocationFactory _locationFactory ;
    private static ILocationService _locationService; 
    private static ICharacterService _characterService;
    private static ICharacterFactory _characterFactory;
    private static IItemFactory _itemFactory;
    private static IItemService _itemService;
    private static ILogger _logger;
    private static void Main(string[] args)
    {
        ConfigureServices();
        _tagService = _serviceProvider.GetService<ITagService>();
        _locationFactory = _serviceProvider.GetService<ILocationFactory>();
        _locationService = _serviceProvider.GetService<ILocationService>();
        _logger = _serviceProvider.GetService<ILogger>();
        _characterFactory = _serviceProvider.GetService<ICharacterFactory>();
        _characterService = _serviceProvider.GetService<ICharacterService>();
        _itemFactory = _serviceProvider.GetService<IItemFactory>();
        _itemService = _serviceProvider.GetService<IItemService>();
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        _locationService.AddConnection(loc.Castle, loc.Village);
        _locationService.RemoveConnection(loc.Village, loc.Swamp);

        _characterService.CreateRandomCharacter(CharacterType.Monster, CharacterPersistence.Temporary).AddTag(_tagService.GetCharacterTag(TagId.Character.Neutral)) ; //rewrite tags to be assigned automatically on creation
        _characterService.CreateRandomCharacter(CharacterType.Other, CharacterPersistence.Permanent);
        _characterService.CreateDefaultCharacter();
        _characterService.CreateDefaultCharacter();
        _characterService.CreateRandomMonster();
        _characterService.CreateRandomTemporalCivilian();
        _characterService.CreateRandomPermanentCivilian();
        _characterService.CreateMainCharacter("Biba", CharacterBodyType.Medium, CharacterGender.Female, CharacterSpecies.Horse);

        _tagService.ValidateITaggables();
        var chars = _characterService.Characters;
        var locs = _locationService.Locations;
        var player = _characterService.GetPlayer();

        _characterService.MoveCharacter(player, loc.Village);

        var apple = _itemService.CreateItem("Apple", ItemType.Food);
        _itemService.CreateItem("Diamond", ItemType.Trinket);
        var diamond = _itemService.GetItemByName("Diamond");
        _characterService.AssignItem(diamond.Id, player.Id);
        _characterService.AssignItem(apple.Id, player.Id);
        _characterService.AssignItem(apple.Id, chars[0].Id);
        _characterService.UnAssignItem(apple.Id, player.Id);
    }
    private static void ConfigureServices()
    {
        _serviceCollection = new ServiceCollection();
        _serviceCollection.AddSingleton<ITagService, TagService>();
        _serviceCollection.AddSingleton<ILocationService, LocationService>();
        _serviceCollection.AddSingleton<ILocationFactory, LocationFactory>();
        _serviceCollection.AddSingleton<ICharacterService, CharacterService>();
        _serviceCollection.AddSingleton<ICharacterFactory, CharacterFactory>();
        _serviceCollection.AddSingleton<ILogger, Logger>();
        _serviceCollection.AddSingleton<IItemFactory, ItemFactory>();
        _serviceCollection.AddSingleton<IItemService, ItemService>();

        _serviceProvider = _serviceCollection.BuildServiceProvider();
    }
}
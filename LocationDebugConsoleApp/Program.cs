//Place code for testing and debugging here.
using GlobalServices;
using GlobalServices.Enums;
using GlobalServices.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using GlobalServices.Entities;

internal class Program
{
    private static ServiceCollection _serviceCollection;
    private static ServiceProvider _serviceProvider;
    private static ITagService _tagService ;
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
        _locationService = _serviceProvider.GetService<ILocationService>();
        _logger = _serviceProvider.GetService<ILogger>();
        _characterFactory = _serviceProvider.GetService<ICharacterFactory>();
        _characterService = _serviceProvider.GetService<ICharacterService>();
        _itemFactory = _serviceProvider.GetService<IItemFactory>();
        _itemService = _serviceProvider.GetService<IItemService>();
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
        _itemService.CreateItem("Diamond", ItemType.Trinket);
        var diamond = _itemService.GetItemByName("Diamond");
        _characterService.AssignItem(diamond.Id, player.Id);
        _characterService.AssignItem(apple.Id, player.Id);
        _characterService.AssignItem(apple.Id, chars.ToList()[0].Id);
        _characterService.UnAssignItem(apple.Id, player.Id);
        apple = null;

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
        _serviceCollection.AddSingleton<ICharacterFactory, CharacterFactory>();
        _serviceCollection.AddSingleton<ILogger, Logger>();
        _serviceCollection.AddSingleton<IItemFactory, ItemFactory>();
        _serviceCollection.AddSingleton<IItemService, ItemService>();

        _serviceProvider = _serviceCollection.BuildServiceProvider();
    }
}
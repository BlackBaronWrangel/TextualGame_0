using GlobalServices.Entities;
using GlobalServices.Enums;
using GlobalServices.Interfaces;

namespace GlobalServices
{
    public class ItemFactory : IItemFactory
    {
        private ILogger _logger;
        private ITagService _tagService;
        public HashSet<Item> Items { get; protected set; } = new();

        public ItemFactory(ITagService tagService, ILogger loggerService)
        {
            _tagService = tagService;
            _logger = loggerService;
        }

        public void AddItem(Item item)
        {
            Items.Add(item);
        }

        public Item GenerateDefaultItem()
        {
            Item item = new();
            RegisterItem(item);
            return item;
        }

        public Item GenerateItem(string name, ItemType type)
        {
            Item item = new Item(name, type);
            RegisterItem(item);
            return item;
        }
        public Item? GetItemById(string id)
        {
            return Items.Where(i => i.Id == id).FirstOrDefault();
        }

        public void RemoveItem(Item item)
        {
            Items.Remove(item);
            _tagService.UnregisterITaggable(item);
            _logger.LogInfo($"Removed item {item}");
        }

        public void RemoveItem(string itemId)
        {
            var item = GetItemById(itemId);
            if (item is not null) RemoveItem(item);
        }
        private void RegisterItem(Item item)
        {
            Items.Add(item);
            _tagService.RegisterITaggable(item);
            _logger.LogInfo($"Created item {item}");
        }
    }
}

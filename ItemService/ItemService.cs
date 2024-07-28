﻿using GlobalServices.Entities;
using GlobalServices.Enums;
using GlobalServices.Interfaces;

namespace GlobalServices
{
    public class ItemService : IItemService
    {
        private const string _itemsJsonPath = "Resources/NamedItems.json";

        ILogger _logger;
        ITagService _tagService;
        public HashSet<Item> Items { get; set; } = new();
        public ItemService(ILogger logger, ITagService tagService)
        {
            _logger = logger;
            _tagService = tagService;
            InitItems();
        }
        public void AddTag(string itemId, string tag)
        {
            var item = GetItem(itemId);
            if (item is not null)
                item.AddTag(tag);
            else
                _logger.LogError($"Can't add tag {tag} to the item {item}");
        }
        public void RemoveTag(string itemId, string tag)
        {
            var item = GetItem(itemId);
            if (item is not null)
                item.AddTag(tag);
            else
                _logger.LogError($"Can't add tag {tag} to the item {item}");
        }
        public Item CreateItem(string name, ItemType itemType)
        {
            Item item = new Item(name, itemType);
            RegisterItem(item);
            return item;
        }
        public Item CreateDefaultItem()
        {
            Item item = new();
            RegisterItem(item);
            return item;
        }
        public Item? GetItem(string itemId) => Items.Where(i => i.Id == itemId).FirstOrDefault();
        public Item? GetItemByName(string ItemName) => Items.FirstOrDefault(x => x.Name == ItemName);
        public void RemoveItem(string itemId)
        {
            var item = GetItem(itemId);
            if (item is null)
            {
                _logger.LogWarning($"Can't get item {itemId} to remove it.");
                return;
            }
            Items.Remove(item);
            _tagService.UnregisterITaggable(item);
            _logger.LogInfo($"Removed {item}");
        }
        private void RegisterItem(Item item)
        {
            Items.Add(item);
            _tagService.RegisterITaggable(item);
            _logger.LogInfo($"Registered {item}");
        }
        private void InitItems()
        {
            try
            {
                var items = GameEntitiesJsonLoader.ReadJsonAsCollection<Item>(_itemsJsonPath);
                if (items is null)
                {
                    throw new("Json items are empty.");
                }
                foreach (var item in items)
                {
                    item.Persistence = ItemPersistence.Permanent;
                    RegisterItem(item);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Can't read items from Json. Error: {ex.Message}");
            }
        }
    }
}

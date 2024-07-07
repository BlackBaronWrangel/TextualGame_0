using GlobalServices.Entities;
using GlobalServices.Enums;
using GlobalServices.Interfaces;

namespace GlobalServices
{
    public class ItemService : IItemService
    {
        IItemFactory _itemfactory;
        ILogger _logger;
        ITagService _tagService;
        public HashSet<Item> Items { get => _itemfactory.Items; }
        public ItemService(IItemFactory itemFactory, ILogger logger, ITagService tagService)
        {
            _logger = logger;
            _tagService = tagService;
            _itemfactory = itemFactory;
        }
        public void AddTag(string itemId, ITag tag)
        {
            var item = _itemfactory.GetItemById(itemId);
            if (item is not null)
            {
                item.AddTag(tag);
            }
            else
            {
                _logger.LogError($"Can't add tag {tag} to the item {item}");
            }
        }

        public void AddTag(string itemId, TagId.ItemTagId tagId)
        {
            var tag = _tagService.GetItemTag(tagId);
            if (tag is not null)
            {
                AddTag(itemId, tag);
            }
            else
            {
                _logger.LogError($"Can't add tag {tag} to the item {itemId}");
            }
        }
        public Item CreateItem(string name, ItemType itemType)
        {
            var item = _itemfactory.GenerateItem(name,itemType);
            return item;
        }
        public Item CreateDefaultItem()
        {
            var item = _itemfactory.GenerateDefaultItem();
            return item;
        }

        public Item? GetItem(string itemId)
        {
            return _itemfactory.GetItemById(itemId);
        }

        public Item? GetItemByName(string ItemName)
        {
            return Items.FirstOrDefault(x => x.Name == ItemName);
        }
        public void RemoveItem(string itemId)
        {
            _itemfactory.RemoveItem(itemId);
        }

        public void RemoveTag(string itemId, ITag tag)
        {
            var item = _itemfactory.GetItemById(itemId);
            item?.RemoveTag(tag);
        }

        public void RemoveTag(string ItemId, TagId.ItemTagId tagId)
        {
            var tag = _tagService.GetItemTag(tagId);
            if (tag is not null) RemoveTag(ItemId, tag);
        }
    }
}

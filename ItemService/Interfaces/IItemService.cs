using GlobalServices.Entities;
using GlobalServices.Enums;

namespace GlobalServices.Interfaces
{
    public interface IItemService
    {
        HashSet<Item> Items { get; }
        Item? GetItem(string itemId);
        Item? GetItemByName(string itemName);
        Item CreateDefaultItem();
        Item CreateItem(string name, ItemType itemType);
        void RemoveItem(string itemId);
        void AddTag(string itemId, ITag tag);
        void AddTag(string itemId, TagId.Item tag);
        void RemoveTag(string itemId, ITag tag);
        void RemoveTag(string itemId, TagId.Item tag);
    }
}

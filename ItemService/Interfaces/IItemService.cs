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
        void AddTag(string itemId, string tagId);
        void RemoveTag(string itemId, string tagId);
    }
}

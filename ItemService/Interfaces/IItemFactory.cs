using GlobalServices.Entities;
using GlobalServices.Enums;

namespace GlobalServices.Interfaces
{
    public interface IItemFactory
    {
        List<Item> Items { get; }
        Item? GetItemById(string id);
        void AddItem(Item Item);
        Item GenerateItem(string name, ItemType type);
        Item GenerateDefaultItem();
        void RemoveItem(Item Item);
        void RemoveItem(string ItemId);
    }
}

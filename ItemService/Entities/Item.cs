using GlobalServices.Enums;
using GlobalServices.Interfaces;

namespace GlobalServices.Entities
{
    public class Item : ItemBase
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = String.Empty;
        public ItemType Type { get; set; } = ItemType.Other;
        public ItemPersistence Persistence { get; set; } = ItemPersistence.Temporary;
        public string Description { get; set; } = String.Empty;
        public Item() : base() { }
        public Item(string name, ItemType itemType) : base() 
        {
            Name = name;
            Type= itemType;
        }
        public Item(string name, string description, ItemType itemType, HashSet<ITag> tags) : base(tags) 
        {
            Name = name;
            Description = description;
            Type = itemType;
            Tags = tags;
        }
        public override string ToString() => $"{GetType().Name} [{Id}, {Name}, {Type}]";

    }
}
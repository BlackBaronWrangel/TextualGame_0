using GlobalServices.Enums;
using GlobalServices.Interfaces;

namespace GlobalServices.Entities
{
    public class Item : ItemBase
    {
        private readonly Guid _id = Guid.NewGuid();
        public string Id { get => _id.ToString(); }
        public string Name { get; protected set; } = String.Empty;
        public ItemType Type { get; protected set; } = ItemType.Other;
        public string Description { get; protected set; } = String.Empty;
        public Item() : base() { }
        public Item(string name, ItemType itemType) : base() 
        {
            Name = name;
            Type= itemType;
        }
        public Item(string name, string description, ItemType itemType, List<ITag> tags) : base(tags) 
        {
            Name = name;
            Description = description;
            Type = itemType;
            Tags = tags;
        }
        public override string ToString()
        {
            string description = $"{Id} [{Name},{Type}]";
            return description;
        }

    }
}
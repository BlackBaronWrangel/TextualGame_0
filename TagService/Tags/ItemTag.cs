using GlobalServices.Enums;
using GlobalServices.Interfaces;

namespace GlobalServices.Tags
{
    public class ItemTag : ITag
    {
        public string Name { get; }
        public TagType TagType { get => TagType.ItemTag; }

        public string Description { get; }

        public ItemTag(string name ,string description)
        {
            Name = name;
            Description = description;
        }
        public override string ToString() => Name;
    }
}

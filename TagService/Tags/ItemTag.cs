using GlobalServices.Enums;
using GlobalServices.Interfaces;

namespace GlobalServices.Tags
{
    public class ItemTag : ITag
    {
        public string Id { get; }
        public TagType TagType { get => TagType.ItemTag; }

        public string Description { get; }

        public ItemTag(string tagId ,string description)
        {
            Id = tagId;
            Description = description;
        }

        public override string ToString()
        {
            return Id;
        }
    }
}

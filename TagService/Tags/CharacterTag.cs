using GlobalServices.Enums;
using GlobalServices.Interfaces;

namespace GlobalServices.Tags
{
    public class CharacterTag : ITag
    {
        public string Id { get; }
        public TagType TagType { get => TagType.CharacterTag; }

        public string Description { get; }

        public CharacterTag(string tagId ,string description)
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

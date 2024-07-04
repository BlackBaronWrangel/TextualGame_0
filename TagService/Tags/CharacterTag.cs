using GlobalServices.Enums;
using GlobalServices.Interfaces;

namespace GlobalServices.Tags
{
    public class CharacterTag : ITag
    {
        public string Name { get; }
        public TagType TagType { get => TagType.CharacterTag; }

        public string Description { get; }

        public CharacterTag(string name ,string description)
        {
            Name = name;
            Description = description;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

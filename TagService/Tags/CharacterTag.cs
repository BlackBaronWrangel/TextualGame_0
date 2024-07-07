using GlobalServices.Enums;

namespace GlobalServices.Tags
{
    public class CharacterTag : TagBase
    {
        public override TagType TagType { get => TagType.CharacterTag; }
        public CharacterTag(string id, string name ,string description) : base()
        {
            Id = id;
            Name = name;
            Description = description;
        }
    }
}

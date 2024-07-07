using GlobalServices.Enums;
using GlobalServices.Interfaces;

namespace GlobalServices.Tags
{
    public class EventTag : ITag
    {
        public string Name { get; }
        public TagType TagType { get => TagType.EventTag; }

        public string Description { get; }

        public EventTag(string name ,string description)
        {
            Name = name;
            Description = description;
        }

        public override string ToString() => Name;
    }
}

using GlobalServices.Enums;
using GlobalServices.Interfaces;

namespace GlobalServices.Tags
{
    public class EventTag : ITag
    {
        public string Id { get; }
        public TagType TagType { get => TagType.EventTag; }

        public string Description { get; }

        public EventTag(string tagId ,string description)
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

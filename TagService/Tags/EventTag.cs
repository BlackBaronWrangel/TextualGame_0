using GlobalServices.Enums;

namespace GlobalServices.Tags
{
    public class EventTag : TagBase
    {
        public override TagType TagType { get => TagType.EventTag; }
        public EventTag(string id, string name ,string description) : base()
        {
            Id = id;
            Name = name;
            Description = description;
        }

    }
}

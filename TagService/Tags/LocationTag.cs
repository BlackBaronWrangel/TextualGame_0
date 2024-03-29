using GlobalServices.Enums;
using GlobalServices.Interfaces;

namespace GlobalServices.Tags
{
    public class LocationTag : ITag
    {
        public string Id { get; }
        public TagType TagType { get => TagType.LocationTag; }

        public string Description { get; }

        public LocationTag(string tagId ,string description)
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

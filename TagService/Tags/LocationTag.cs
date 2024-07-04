using GlobalServices.Enums;
using GlobalServices.Interfaces;

namespace GlobalServices.Tags
{
    public class LocationTag : ITag
    {
        public string Name { get; }
        public TagType TagType { get => TagType.LocationTag; }

        public string Description { get; }

        public LocationTag(string name, string description)
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

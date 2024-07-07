using GlobalServices.Enums;

namespace GlobalServices.Tags
{
    public class LocationTag : TagBase
    {
        public override TagType TagType { get => TagType.LocationTag; }

        public LocationTag(string id, string name, string description) : base()
        {
            Id = id;
            Name = name;
            Description = description;
        }
    }
}

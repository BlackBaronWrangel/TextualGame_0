using GlobalServices.Interfaces;
using LocationId = GlobalServices.Entities.DefaultMap.LocationId;

namespace GlobalServices.Entities
{
    public class Location : LocationBase
    {
        public LocationId Id { get; protected set; }
        public HashSet<Location> ConnectedLocations { get; protected set; }

        public Location(LocationId id, HashSet<ITag> tags) : base(tags)
        {
            Id = id;
            ConnectedLocations = new HashSet<Location>();
        }
        public Location(LocationId id) : base()
        {
            Id = id;
            ConnectedLocations = new HashSet<Location>();
        }
        public override string ToString()
        {
            return Id.ToString();
        }
    }
}

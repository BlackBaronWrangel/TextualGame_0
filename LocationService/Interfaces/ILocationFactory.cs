using GlobalServices.Entities;
using LocationId = GlobalServices.Entities.DefaultMap.LocationId;

namespace GlobalServices.Interfaces
{
    public interface ILocationFactory
    {
        List<Location> Locations { get; }
        Location GetLocation(LocationId id);
        void AddConnection(LocationId id1, LocationId id2);
        void RemoveConnection(LocationId id1, LocationId id2);
    }
}

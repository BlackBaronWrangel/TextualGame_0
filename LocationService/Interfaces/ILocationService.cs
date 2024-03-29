using GlobalServices.Entities;
using GlobalServices.Enums;
using LocationId = GlobalServices.Entities.DefaultMap.LocationId;

namespace GlobalServices.Interfaces
{
    public interface ILocationService
    {
        List<Location> Locations { get; }
        Location? GetLocation(LocationId locationId);
        List<Location> GetConnectedLocations(LocationId locationId);
        void AddConnection(LocationId location1, LocationId location2);
        void RemoveConnection(LocationId location1, LocationId location2);
        void AddTag(LocationId locationId, ITag tag);
        void AddTag(LocationId locationId, TagId.Location tag);
        void RemoveTag(LocationId locationId, ITag tag);
        void RemoveTag(LocationId locationId, TagId.Location tag);
    }
}

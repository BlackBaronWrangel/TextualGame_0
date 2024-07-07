using GlobalServices.Entities;
using GlobalServices.Enums;

namespace GlobalServices.Interfaces
{
    public interface ILocationService
    {
        HashSet<Location> Locations { get; }
        Location? GetLocation(string locationId);
        HashSet<Connection>? GetConnections(string locationId);
        void AddConnection(string location1, string location2, double distance);
        void RemoveConnection(string location1, string location2);
        void AddTag(string locationId, ITag tag);
        void AddTag(string locationId, TagId.LocationTagId tag);
        void RemoveTag(string locationId, ITag tag);
        void RemoveTag(string locationId, TagId.LocationTagId tag);
    }
}

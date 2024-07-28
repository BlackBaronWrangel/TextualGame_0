using GlobalServices.Entities;

namespace GlobalServices.Interfaces
{
    public interface ILocationService
    {
        HashSet<Location> Locations { get; }
        Location? GetLocation(string locationId);
        HashSet<Connection>? GetConnections(string locationId);
        void AddConnection(string location1, string location2, double distance);
        void RemoveConnection(string location1, string location2);
        void AddTag(string locationId, string tag);
        void RemoveTag(string locationId, string tag);
    }
}

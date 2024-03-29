using GlobalServices.Entities;
using GlobalServices.Interfaces;
using LocationId = GlobalServices.Entities.DefaultMap.LocationId;

namespace GlobalServices
{
    public class LocationFactory : ILocationFactory
    {
        private ILogger _logger;
        private ITagService _tagService;

        public LocationFactory(ITagService tagService, ILogger logger) 
        {
            _logger = logger;
            _tagService = tagService;
        }
        public List<Location> Locations { get => _locations.Values.ToList(); }

        private readonly Dictionary<LocationId, Location> _locations = new();
        public Location GetLocation(LocationId id)
        {
            if (!_locations.ContainsKey(id))
            {
                var newLoc = new Location(id);
                _locations[id] = newLoc;
                _tagService.RegisterITaggable(newLoc);
            }
            return _locations[id];
        }

        public void AddConnection(LocationId id1, LocationId id2)
        {
            var location1 = GetLocation(id1);
            var location2 = GetLocation(id2);

            if (!location1.ConnectedLocations.Contains(location2))
            {
                location1.ConnectedLocations.Add(location2);
            }
            else
            {
                _logger.LogWarning($"Attempt to add an existing connection between {location1} and {location2}");
            }

            if (!location2.ConnectedLocations.Contains(location1))
            {
                location2.ConnectedLocations.Add(location1);
            }
            else
            {
                _logger.LogWarning($"Attempt to add an existing connection between {location2} and {location1}");
            }
        }

        public void RemoveConnection(LocationId id1, LocationId id2)
        {
            var location1 = GetLocation(id1);
            var location2 = GetLocation(id2);

            if (location1.ConnectedLocations.Contains(location2))
            {
                location1.ConnectedLocations.Remove(location2);
            }
            else 
            {
                _logger.LogWarning($"Attempt to remove an unexisting connection between {location1} and {location2}");
            }

            if (location2.ConnectedLocations.Contains(location1))
            {
                location2.ConnectedLocations.Remove(location1);
            }
            else
            {
                _logger.LogWarning($"Attempt to remove an unexisting connection between {location2} and {location1}");
            }
        }
    }


}
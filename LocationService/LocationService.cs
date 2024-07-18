using GlobalServices.Entities;
using GlobalServices.Enums;
using GlobalServices.Interfaces;

namespace GlobalServices
{
    public class LocationService : ILocationService
    {
        private const string _locationsJsonPath = "Resources/Locations.json";

        private ILogger _logger;
        private ITagService _tagService;
        public HashSet<Location> Locations { get; protected set; } = new();

        public LocationService(ITagService tagService, ILogger logger)
        {
            _tagService = tagService;
            _logger = logger;

            InitLocations();
        }
        public Location? GetLocation(string locationId) => 
            Locations.FirstOrDefault(l => l.Id == locationId);
        public HashSet<Connection>? GetConnections(string locationId)
        {
            var loc = GetLocation(locationId);
            if (loc is null)
            {
                var message = $"Location {locationId} can't be loaded.";
                _logger.LogError(message);
                throw new Exception(message);
            }
            return loc.ConnectedLocations;
        }
        public void AddConnection(string id1, string id2, double distance)
        {
            var location1 = GetLocation(id1);
            var location2 = GetLocation(id2);

            if (location1 is null || location2 is null)
            {
                _logger.LogWarning($"Can't add connection between {id1} and {id2} because one or both locations can't be found.");
                return;
            }

            if (!location1.ConnectedLocations.Any(c => c.LocationId == id2))
                location1.ConnectedLocations.Add(new(location2.Id, distance));
            else
                _logger.LogWarning($"Attempt to add an existing connection between {id1} and {id2}");
        }
        public void RemoveConnection(string id1, string id2)
        {
            var location1 = GetLocation(id1);

            if (location1 is null)
            {
                _logger.LogWarning($"Can't remove connection between {id1} and {id2} because location can't be found.");
                return;
            }

            var connection1 = location1.ConnectedLocations.FirstOrDefault(c => c.LocationId == id2);

            if (connection1 is not null)
                location1.ConnectedLocations.Remove(connection1);
            else
                _logger.LogWarning($"Attempt to remove an unexisting connection between {id1} and {id2}");
        }
        public void AddTag(string locationId, ITag tag)
        {
            var location = GetLocation(locationId);
            if (location is null)
            {
                _logger.LogWarning($"Attempt to add a tag to unexisting location {locationId}");
                return;
            }
            location.AddTag(tag);
        }
        public void AddTag(string locationId, TagId.LocationTagId tagId)
        {
            var tag = _tagService.GetLocationTag(tagId);
            if (tag != null)
                AddTag(locationId, tag);
            else
                _logger.LogError($"Can't add tag {tag} to the location {locationId}");
        }
        public void RemoveTag(string locationId, ITag tag)
        {
            var location = GetLocation(locationId);
            if (location is not null)
                location.RemoveTag(tag);
            else
                _logger.LogWarning($"Can't get location {locationId} to remove tag {tag}");
        }
        public void RemoveTag(string locationId, TagId.LocationTagId tagId)
        {
            var tag = _tagService.GetLocationTag(tagId);
            if (tag != null)
                RemoveTag(locationId, tag);
            else
                _logger.LogError($"Can't remove tag {tag} from the location {locationId}");
        }
        private void InitLocations()
        {
            try
            {
                var locations = GameEntitiesJsonLoader.ReadJsonAsCollection<Location>(_locationsJsonPath);
                if (locations is null)
                {
                    throw new("Locations are empty.");
                }
                foreach (var location in locations)
                    RegisterLocation(location);
            }
            catch (Exception ex) 
            {
                _logger.LogError($"Can't read locations from Json. Error: {ex.Message}");
            }
        }
        private void ReadLocationsDefaultTags()
        {
        }
        private void RegisterLocation(Location location)
        {
            Locations.Add(location);
            _tagService.RegisterITaggable(location);
            _logger.LogInfo($"Registered {location}");
        }
    }
}

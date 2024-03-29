using GlobalServices.Entities;
using GlobalServices.Enums;
using GlobalServices.Interfaces;
using static GlobalServices.Entities.DefaultMap;
using LocationId = GlobalServices.Entities.DefaultMap.LocationId;

namespace GlobalServices
{
    public class LocationService : ILocationService
    {
        private ILogger _logger;
        private ILocationFactory _locationFactory;
        private ITagService _tagService;
        public List<Location> Locations { get => _locationFactory.Locations; }

        public LocationService(ILocationFactory locationFactory, ITagService tagService, ILogger logger)
        {
            _locationFactory = locationFactory;
            _tagService = tagService;
            _logger = logger;

            InitLocations();
            InitLocationsDefaultConnections();
            InitLocationsDefaultTags();
        }
        public Location? GetLocation(LocationId locationId)
        {
            return _locationFactory.GetLocation(locationId);
        }
        public List<Location> GetConnectedLocations(LocationId locationId)
        {
            var loc = GetLocation(locationId);
            if (loc == null)
            {
                var message = $"Location {locationId} can't be loaded.";
                _logger.LogError(message);
                throw new Exception(message);
            }
            return loc.ConnectedLocations;
        }
        public void AddConnection(LocationId location1, LocationId location2)
        {
            _locationFactory.AddConnection(location1, location2);
        }
        public void RemoveConnection(LocationId location1, LocationId location2)
        {
            _locationFactory.RemoveConnection(location1, location2);
        }
        public void AddTag(LocationId locationId, ITag tag)
        {
            var location = _locationFactory.GetLocation(locationId);
            location.AddTag(tag);
        }
        public void AddTag(LocationId locationId, TagId.Location tagId)
        {
            var tag = _tagService.GetLocationTag(tagId);
            if (tag != null)
            {
                AddTag(locationId, tag);
            }
            else
            {
                _logger.LogError($"Can't add tag {tag} to the location {locationId}");
            }
        }
        public void RemoveTag(LocationId locationId, ITag tag)
        {
            var location = _locationFactory.GetLocation(locationId);
            location.RemoveTag(tag);
        }

        public void RemoveTag(LocationId locationId, TagId.Location tagId)
        {
            var tag = _tagService.GetLocationTag(tagId);
            if (tag != null)
            {
                RemoveTag(locationId, tag);
            }
            else
            {
                _logger.LogError($"Can't remove tag {tag} from the location {locationId}");
            }
        }
        private void InitLocations()
        {
            foreach (LocationId item in Enum.GetValues(typeof(LocationId)))
            {
                _locationFactory.GetLocation(item);
            }
        }
        private void InitLocationsDefaultConnections()
        {
            foreach (var connection in DefaultMap.DefaultConnections)
            {
                _locationFactory.AddConnection(connection.Item1, connection.Item2);
            }
        }
        private void InitLocationsDefaultTags()
        {
            foreach (var locationTagPair in DefaultMap.DefaultLocationTags)
            {
                var tag = _tagService.GetLocationTag(locationTagPair.Item2);
                if (tag is not null)
                {
                    AddTag(locationTagPair.Item1, tag);
                }
                else
                {
                    _logger.LogError($"Can't obtain and add tag {locationTagPair.Item2} to the location {locationTagPair.Item1} ");
                }
            }
        }
    }
}

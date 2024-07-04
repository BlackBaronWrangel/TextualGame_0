using GlobalServices.Enums;

namespace GlobalServices.Entities
{
    public static class DefaultMap
    {
        public enum LocationId
        {
            Castle,
            Forest,
            Village,
            Lake,
            Swamp,
        };

        public static HashSet<(LocationId, LocationId)> DefaultConnections = new HashSet<(LocationId, LocationId)>(){
                (LocationId.Castle, LocationId.Village),
                (LocationId.Castle, LocationId.Forest),
                (LocationId.Village,LocationId.Lake),
                (LocationId.Swamp,LocationId.Lake),
                (LocationId.Forest,LocationId.Swamp),
        };

        public static HashSet<(LocationId, TagId.Location)> DefaultLocationTags = new HashSet<(LocationId, TagId.Location)>(){
                (LocationId.Castle, TagId.Location.Indoor),
                (LocationId.Castle, TagId.Location.Neutral),

                (LocationId.Village, TagId.Location.Outdoor),
                (LocationId.Village, TagId.Location.Neutral),

                (LocationId.Forest, TagId.Location.Outdoor),
                (LocationId.Forest, TagId.Location.Scary),

                (LocationId.Lake, TagId.Location.Outdoor),
                (LocationId.Lake, TagId.Location.Neutral),

                (LocationId.Swamp, TagId.Location.Outdoor),
                (LocationId.Swamp, TagId.Location.Scary),
        };
    }
}

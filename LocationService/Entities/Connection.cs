using Newtonsoft.Json;

namespace GlobalServices.Entities
{
    public class Connection
    {
        [JsonProperty]
        public string LocationId { get; protected set; } = string.Empty;
        [JsonProperty]
        public double Distance { get; protected set; } = 0; //Distance in hours spent to move. Like 0.5 means 30 minutes. 
        [JsonConstructor]
        public Connection() { }
        public Connection(string locationId, double distance) 
        {
            LocationId = locationId;
            Distance = distance;
        }
        public override string ToString() => $"{LocationId} : {Distance}";
    }
}

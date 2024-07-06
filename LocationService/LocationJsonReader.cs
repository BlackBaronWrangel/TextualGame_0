using GlobalServices.Entities;
using Newtonsoft.Json;

namespace GlobalServices
{
    public class LocationJsonReader
    {
        private const string _locationsJsonPath = "Resources/Locations.json";
        public static HashSet<Location>? ReadLocationsFromJson() => ReadJsonAsCollection<Location>(_locationsJsonPath);
        private static HashSet<T>? ReadJsonAsCollection<T>(string jsonPath)
        {
            string jsonText = File.ReadAllText(jsonPath);
            HashSet<T>? set = JsonConvert.DeserializeObject<HashSet<T>>(jsonText);
            return set; 
        }
    }
}

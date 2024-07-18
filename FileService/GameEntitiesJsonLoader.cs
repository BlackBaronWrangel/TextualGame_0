using Newtonsoft.Json;

namespace GlobalServices
{
    public class GameEntitiesJsonLoader
    {
        //private const string _defaultResourcesFolder = "Resources";
        //private const string _locationsJsonPath = $"{_defaultResourcesFolder}/Locations.json";
        //private const string _charactersJsonPath = $"{_defaultResourcesFolder}/NamedCharacters.json";
        //private const string _scenesJsonFolderPath = $"{_defaultResourcesFolder}/Scenes";
        public static IEnumerable<T>? ReadJsonAsCollection<T>(string jsonPath)
        {
            string jsonText = File.ReadAllText(jsonPath);
            HashSet<T>? set = JsonConvert.DeserializeObject<HashSet<T>>(jsonText);
            return set;
        }
        public static T? ReadJsonSingleEntity<T>(string filePath)
        {
            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<T>(json);
        }
        public static IEnumerable<T> ReadFolderAsCollection<T>(string directoryPath)
        {
            var jsonFiles = Directory.GetFiles(directoryPath, "*.json");
            var entities = new HashSet<T>();

            foreach (var filePath in jsonFiles)
            {
                var entity = ReadJsonSingleEntity<T>(filePath);
                if (entity != null)
                    entities.Add(entity);
            }
            return entities;
        }
    }
}

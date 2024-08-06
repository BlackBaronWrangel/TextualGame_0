using Newtonsoft.Json;

namespace GlobalServices
{
    public class GameEntitiesJsonLoader
    {
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
            var entities = new List<T>();

            foreach (var filePath in jsonFiles)
            {
                var jsonContent = File.ReadAllText(filePath);
                var entries = JsonConvert.DeserializeObject<IEnumerable<T>>(jsonContent);
                if (entries != null && entries.Any())
                    entities.AddRange(entries);
            }
            return entities;
        }
    }
}

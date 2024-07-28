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
            var entities = new HashSet<T>();

            foreach (var filePath in jsonFiles)
            {
                var entries = ReadJsonAsCollection<T>(filePath);
                if (entries is not null && entries.Count()>0)
                    foreach (var entry in entries)
                        entities.Add(entry);
            }
            return entities;
        }
    }
}

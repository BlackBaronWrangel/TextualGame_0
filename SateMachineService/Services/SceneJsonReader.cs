using GlobalServices.Entities;
using Newtonsoft.Json;

namespace GlobalServices
{
    public class SceneJsonReader
    {
        private const string _defaultJsonScenesPath = "Resources/Scenes";

        public static IEnumerable<JsonScene> ReadScenesFromJson(string directoryPath = _defaultJsonScenesPath)
        {
            var sceneFiles = Directory.GetFiles(directoryPath, "*.json");
            var scenes = new HashSet<JsonScene>();

            foreach (var filePath in sceneFiles)
            {
                var scene = ReadJsonScene(filePath);
                if (scene != null)
                {
                    scenes.Add(scene);
                }
            }
            return scenes;
        }

        public static JsonScene? ReadSceneFromJson(string sceneName)
        {
            string scenePath = Path.Combine(_defaultJsonScenesPath, $"{sceneName}.json");
            return ReadJsonScene(scenePath);
        }

        public static JsonScene? ReadJsonScene(string filePath)
        {
            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<JsonScene>(json);
        }
    }
}

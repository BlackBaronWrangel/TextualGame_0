using GlobalServices.Entities;
using Newtonsoft.Json;

namespace GlobalServices
{
    public class CharacterJsonReader
    {
        private const string _charactersJsonPath = "Resources/NamedCharacters.json";
        public static HashSet<Character>? ReadCharactersFromJson() => ReadJsonAsCollection<Character>(_charactersJsonPath);
        private static HashSet<T>? ReadJsonAsCollection<T>(string jsonPath)
        {
            string jsonText = File.ReadAllText(jsonPath);
            HashSet<T>? set = JsonConvert.DeserializeObject<HashSet<T>>(jsonText);
            return set; 
        }
    }
}

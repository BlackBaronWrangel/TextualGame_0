using GlobalServices.Interfaces;
using GlobalServices.Tags;
using Newtonsoft.Json;

namespace GlobalServices
{
    public class TagJsonReader
    {
        private const string _locationTagsJsonPath = "Resources/LocationTags.json";
        private const string _eventTagsJsonPath = "Resources/EventTags.json";
        private const string _characterTagsJsonPath = "Resources/CharacterTags.json";
        private const string _itemTagsJsonPath = "Resources/ItemTags.json";
        public static HashSet<TagBase>? ReadLocationTagsFromJson() => ReadJsonAsCollection<TagBase>(_locationTagsJsonPath);
        public static HashSet<TagBase>? ReadEventTagsFromJson() => ReadJsonAsCollection<TagBase>(_eventTagsJsonPath);
        public static HashSet<TagBase>? ReadCharacterTagsFromJson() => ReadJsonAsCollection<TagBase>(_characterTagsJsonPath);
        public static HashSet<TagBase>? ReadItemTagsFromJson() => ReadJsonAsCollection<TagBase>(_itemTagsJsonPath);
        private static HashSet<T>? ReadJsonAsCollection<T>(string jsonPath)
        {
            try
            {
                string jsonText = File.ReadAllText(jsonPath);
                HashSet<T>? set = JsonConvert.DeserializeObject<HashSet<T>>(jsonText);
                return set;
            }
            catch
            {
                return null;
            }
        }
    }
}

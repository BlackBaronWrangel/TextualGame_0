using GlobalServices.Entities;
using GlobalServices.Enums;
using GlobalServices.Interfaces;

namespace GlobalServices
{
    public class CharacterService : ICharacterService
    {
        private const string _charactersJsonPath = "Resources/NamedCharacters.json";

        ILogger _logger;
        ITagService _tagService;
        ILocationService _locationService;
        IItemService _itemService;
        public HashSet<Character> Characters { get; set; } = new HashSet<Character>();

        public CharacterService(IItemService itemService ,ILogger logger, ITagService tagService, ILocationService locationService)
        {
            _logger = logger;
            _tagService = tagService;
            _locationService = locationService;
            _itemService = itemService;

            InitCharacters();
        }

        public Character? GetCharacter(string characterId)
        {
            var character = Characters.FirstOrDefault(c => c.Id == characterId);
            return character;
        }
        public Character? GetPlayer()
        {
            return Characters.Where(c => c.ControlType == CharacterControlType.Player).FirstOrDefault();
        }
        public Character? GetCharacterByName(string characterName)
        {
            return Characters.Where(c => c.Name == characterName).FirstOrDefault();
        }
        public void RemoveCharacter(string characterId)
        {
            var character = GetCharacter(characterId);
            if (character is null)
            {
                _logger.LogWarning($"Can't remove character {characterId}. Character not found.");
                return;
            }
            Characters.Remove(character);
            _tagService.UnregisterITaggable(character);
            _logger.LogInfo($"Removed {character}");
        }

        public Character CreateRandomCharacter(CharacterType characterType, CharacterPersistence characterPersistence)
        {
            Character character = new Character(
                name: String.Empty,
                type: characterType,
                persistence: characterPersistence,
                bodyType: GetRandomEnumValue(CharacterBodyType.Other),
                species: GetRandomEnumValue<CharacterSpecies>(),
                gender: GetRandomEnumValue(CharacterGender.Other)
                );
            RegisterCharacter(character);
            return character;
        }
        public Character CreateDefaultCharacter()
        {
            Character character = new Character();
            RegisterCharacter(character);
            return character;
        }
        public Character CreateMainCharacter(string name, CharacterBodyType bodyType, CharacterGender characterGender, CharacterSpecies characterSpecies)
        {
            Character character = new Character(
                name,
                CharacterType.Civillian,
                CharacterPersistence.Permanent,
                bodyType,
                characterSpecies,
                characterGender
                );
            character.ControlType = CharacterControlType.Player;
            RegisterCharacter(character);
            return character;
        }
        public Character CreateRandomMonster()
        {
            var character = CreateRandomCharacter(
                CharacterType.Monster,
                CharacterPersistence.Temporary
                );

            return character;
        }
        public Character CreateRandomPermanentCivilian()
        {
            var character = CreateRandomCharacter(
                CharacterType.Civillian,
                CharacterPersistence.Permanent
                );
            return character;
        }
        public Character CreateRandomTemporalCivilian()
        {
            var character = CreateRandomCharacter(
                CharacterType.Civillian,
                CharacterPersistence.Temporary
                );
            return character;
        }
        public void MoveCharacter(string characterId, string locationId)
        {
            var character = GetCharacter(characterId);
            var loc = _locationService.GetLocation(locationId);
            if (loc is null || character is null)
            {
                _logger.LogWarning($"Failed to move {characterId} to location {locationId}");
                return;
            }
            character.Location = loc.Id;
            _logger.LogInfo($"{character} moved to location {locationId}");
        }
        public void AssignItem(string itemId, string characterId)
        {
            var character = GetCharacter(characterId);
            var item = _itemService.GetItem(itemId);
            if ((item is not null) && (character is not null))
            {
                var previousOwner = Characters.FirstOrDefault(c => c.Items.Contains(item));
                if (previousOwner is not null) UnAssignItem(itemId, previousOwner.Id);
                character.Items.Add(item);
                _logger.LogInfo($"{item} assigned to character {character}");
            }
            else
            {
                _logger.LogError($"Can't assign {item} to character {character}");
            }
        }
        public void UnAssignItem(string itemId, string characterId)
        {
            var character = GetCharacter(characterId);
            var item = _itemService.GetItem(itemId);

            if ((item is null) || (character is null))
            {
                _logger.LogError($"Can't remove {item} from {character}");
            }
            else
            {
                if (character.Items.Contains(item))
                {
                    character.Items.Remove(item); 
                    _logger.LogInfo($"{item} unassigned from {character}");
                }
                else
                {
                    _logger.LogWarning($"{character} doesn't have {item} to remove.");
                }                    
            }
        }
        public void RemoveTag(string characterId, string tag)
        {
            var character = GetCharacter(characterId);
            if (character is not null)
            {
                character.RemoveTag(tag);
            }
            else
            {
                _logger.LogError($"Can't remove tag {tag} from the character {characterId}");
            }
        }
        public void AddTag(string characterId, string tag)
        {
            var character = Characters.FirstOrDefault(c => c.Id == characterId);
            if (character is not null)
            {
                character.AddTag(tag);
            }
            else
            {
                _logger.LogError($"Can't add tag {tag} to the character {characterId}");
            }
        }
        private void InitCharacters()
        {
            try
            {
                var characters = GameEntitiesJsonLoader.ReadJsonAsCollection<Character>(_charactersJsonPath);
                if (characters is null)
                {
                    throw new("Json characters are empty.");
                }
                foreach (var character in characters)
                {
                    character.Persistence = CharacterPersistence.Permanent;
                    RegisterCharacter(character);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Can't read characters from Json. Error: {ex.Message}");
            }
        }
        private T? GetRandomEnumValue<T>(params T[] excludeValues) where T : Enum
        {
            try
            {
                Random random = new Random();
                var values = Enum.GetValues(typeof(T)).Cast<T>().ToList();
                foreach (var value in excludeValues)
                {
                    values.Remove(value);
                }
                if (!values.Any())
                {
                    throw new ArgumentException("No values available for random selection after excluding specified values.");
                }
                return values[random.Next(values.Count)];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return default;
            }
        }
        private void RegisterCharacter(Character character)
        {
            Characters.Add(character);
            _tagService.RegisterITaggable(character);
            _logger.LogInfo($"Registered {character}");
        }
    }
}
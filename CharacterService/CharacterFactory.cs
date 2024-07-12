using GlobalServices.Entities;
using GlobalServices.Enums;
using GlobalServices.Interfaces;

namespace GlobalServices
{
    public class CharacterFactory : ICharacterFactory
    {
        private ILogger _logger;
        private ITagService _tagService;
        public HashSet<Character> Characters { get; protected set; } = new();

        public CharacterFactory(ITagService tagService, ILogger logger)
        {
            _logger = logger;
            _tagService = tagService;
        }

        public void AddCharacter(Character character)
        {
            Characters.Add(character);
            _tagService.RegisterITaggable(character);
        }
        public Character GenerateCharacter(string name, CharacterType type, CharacterPersistence persistence, CharacterBodyType bodyType, CharacterSpecies species, CharacterGender gender)
        {
            Character character = new Character(
                name,
                type,
                persistence,
                bodyType,
                species,
                gender
                );
            RegisterCharacter(character);
            return character;
        }
        public Character GenerateRandomCharacter(CharacterType characterType, CharacterPersistence characterPersistence)
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
        public Character GenerateDefaultCharacter()
        {
            Character character = new Character();
            RegisterCharacter(character);
            return character;
        }

        public Character? GetCharacterById(string id)
        {
            return Characters.Where(c => c.Id == id).FirstOrDefault();
        }

        public void RemoveCharacter(Character character)
        {
            Characters.Remove(character);
            _tagService.UnregisterITaggable(character);
            _logger.LogInfo($"Removed {character}");
        }

        public void RemoveCharacter(string characterId)
        {
            var character = GetCharacterById(characterId);
            if (character is not null)
            {
                RemoveCharacter(character);
            }
            else
            {
                _logger.LogError($"Can't get character by Id {characterId} to remove.");
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
            _logger.LogInfo($"Created {character}");
        }
    }
}

using GlobalServices.Entities;
using GlobalServices.Enums;

namespace GlobalServices.Interfaces
{
    public interface ICharacterFactory
    {
        HashSet<Character> Characters { get; }
        Character? GetCharacterById(string id);
        void AddCharacter(Character character);
        Character GenerateCharacter(string name, CharacterType type, CharacterPersistence persistence, CharacterBodyType bodyType, CharacterSpecies species, CharacterGender gender);
        Character GenerateRandomCharacter(CharacterType characterType, CharacterPersistence characterPersistence);
        Character GenerateDefaultCharacter();
        void RemoveCharacter(Character character);
        void RemoveCharacter(string characterId);

    }
}

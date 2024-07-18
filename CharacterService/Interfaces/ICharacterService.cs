using GlobalServices.Entities;
using GlobalServices.Enums;

namespace GlobalServices.Interfaces
{
    public interface ICharacterService
    {
        HashSet<Character> Characters { get; }
        Character? GetCharacter(string CharacterId);
        Character? GetPlayer();
        Character? GetCharacterByName(string CharacterName);
        Character CreateRandomCharacter(CharacterType characterType, CharacterPersistence characterPersistence);
        Character CreateDefaultCharacter();
        Character CreateMainCharacter(string name, CharacterBodyType bodyType, CharacterGender characterGender, CharacterSpecies characterSpecies );
        Character CreateRandomMonster();
        Character CreateRandomPermanentCivilian();
        Character CreateRandomTemporalCivilian();
        void RemoveCharacter(string CharacterId);
        void MoveCharacter(string characterId, string locationId);
        void AssignItem(string itemid, string characterId);
        void UnAssignItem(string itemid, string characterId);
        void AddTag(string CharacterId, ITag tag);
        void AddTag(string CharacterId, TagId.CharacterTagId tag);
        void RemoveTag(string CharacterId, ITag tag);
        void RemoveTag(string CharacterId, TagId.CharacterTagId tag);
    }
}

using GlobalServices.Entities;
using GlobalServices.Enums;
using LocationId = GlobalServices.Entities.DefaultMap.LocationId;

namespace GlobalServices.Interfaces
{
    public interface ICharacterService
    {
        List<Character> Characters { get; }
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
        void MoveCharacter(Character character, LocationId locationId);
        void AddTag(string CharacterId, ITag tag);
        void AddTag(string CharacterId, TagId.Character tag);
        void RemoveTag(string CharacterId, ITag tag);
        void RemoveTag(string CharacterId, TagId.Character tag);
    }
}

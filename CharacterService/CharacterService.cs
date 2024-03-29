﻿using GlobalServices;
using GlobalServices.Entities;
using GlobalServices.Enums;
using GlobalServices.Interfaces;
using System.Xml.Linq;
using static GlobalServices.Entities.DefaultMap;
using LocationId = GlobalServices.Entities.DefaultMap.LocationId;

namespace GlobalServices
{
    public class CharacterService : ICharacterService
    {
        ICharacterFactory _characterFactory;
        ILogger _logger;
        ITagService _tagService;
        ILocationService _locationService;
        public List<Character> Characters { get => _characterFactory.Characters; }

        public CharacterService(ICharacterFactory characterFactory, ILogger logger, ITagService tagService, ILocationService locationService)
        {
            _characterFactory = characterFactory;
            _logger = logger;
            _tagService = tagService;
            _locationService = locationService;
        }
        public void AddTag(string characterId, ITag tag)
        {
            var character = _characterFactory.GetCharacterById(characterId);
            if (character is not null)
            {
                character.AddTag(tag);
            }
            else
            {
                _logger.LogError($"Can't add tag {tag} to the character {characterId}");
            }
        }

        public void AddTag(string characterId, TagId.Character tagId)
        {
            var tag = _tagService.GetCharacterTag(tagId);
            if (tag is not null)
            {
                AddTag(characterId, tag);
            }
            else
            {
                _logger.LogError($"Can't add tag {tag} to the character {characterId}");
            }
        }

        public Character? GetCharacter(string characterId)
        {
            var character = _characterFactory.GetCharacterById(characterId);
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
            _characterFactory.RemoveCharacter(characterId);
        }
        public void RemoveTag(string characterId, ITag tag)
        {
            var character = _characterFactory.GetCharacterById(characterId);
            if (character is not null)
            {
                character.RemoveTag(tag);
            }
            else
            {
                _logger.LogError($"Can't remove tag {tag} from the character {characterId}");
            }
        }
        public void RemoveTag(string characterId, TagId.Character tagId)
        {
            var tag = _tagService.GetCharacterTag(tagId);
            if (tag is not null)
            {
                RemoveTag(characterId, tag);
            }
            else
            {
                _logger.LogError($"Can't remove tag {tag} from the character {characterId}");
            }
        }

        public Character CreateRandomCharacter(CharacterType characterType, CharacterPersistence characterPersistence)
        {
            return _characterFactory.GenerateRandomCharacter(characterType, characterPersistence);
        }

        public Character CreateDefaultCharacter()
        {
            return _characterFactory.GenerateDefaultCharacter();
        }

        public Character CreateMainCharacter(string name, CharacterBodyType bodyType, CharacterGender characterGender, CharacterSpecies characterSpecies)
        {
            var character = _characterFactory.GenerateCharacter(
                name,
                CharacterType.Civillian,
                CharacterPersistence.Permanent,
                bodyType,
                characterSpecies,
                characterGender
                );
            character.ControlType = CharacterControlType.Player;
            return character;
        }

        public Character CreateRandomMonster()
        {
            var character = _characterFactory.GenerateRandomCharacter(
                CharacterType.Monster,
                CharacterPersistence.Temporary
                );
            return character;
        }
        public Character CreateRandomPermanentCivilian()
        {
            var character = _characterFactory.GenerateRandomCharacter(
                CharacterType.Civillian,
                CharacterPersistence.Permanent
                );
            return character;
        }
        public Character CreateRandomTemporalCivilian()
        {
            var character = _characterFactory.GenerateRandomCharacter(
                CharacterType.Civillian,
                CharacterPersistence.Temporary
                );
            return character;
        }

        public void MoveCharacter(Character character, LocationId locationId)
        {
            character.Location = _locationService.GetLocation(locationId);
            _logger.LogInfo($"Character {character} moved to location {locationId}");
        }
    }
}
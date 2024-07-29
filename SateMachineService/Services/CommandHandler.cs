using AutoMapper;
using GlobalServices.Entities;
using GlobalServices.Enums;
using GlobalServices.Interfaces;
using System.Text.RegularExpressions;

namespace GlobalServices
{
    public class CommandHandler : ICommandHandler
    {
        private IEventService _eventService;
        private ILogger _logger;
        private ITagService _tagService;
        private ICharacterService _characterService;
        private IItemService _itemService;
        private IMapper _mapper;
        public CommandHandler(
            ILogger logger,
            IEventService eventService,
            ITagService tagService,
            ICharacterService characterService,
            IItemService itemService,
            IMapper mapper
            )
        {
            _logger = logger;
            _eventService = eventService;
            _tagService = tagService;
            _characterService = characterService;
            _itemService = itemService;
            _mapper = mapper;
        }
        public void ExecuteEventCommand(Event gameEvent, string command)
        {
            var match = Regex.Match(command.Trim(), @"(\w+)\(([^)]*)\)");
            if (match.Success)
            {
                var commandName = match.Groups[1].Value;
                var args = match.Groups[2].Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                switch (commandName)
                {
                    case "AddRandomCharactersCount":
                        AddRandomCharactersCount(gameEvent, args);
                        break;
                    case "AddRandomCharacter":
                        AddRandomCharacter(gameEvent, args);
                        break;
                    case "AddRandomItems":
                        AddRandomItems(gameEvent, args);
                        break;
                    default:
                        _logger.LogError($"Unknown command {command}");
                        break;
                }
            }
        }
        private void AddRandomCharactersCount(Event gameEvent, string[]? args)
        {
            if (args is null)
            {
                _logger.LogWarning($"Event command parsing error. Arguments are null");
                return;
            }

            int min = 0;
            int max = 0;

            try
            {
                min = int.Parse(args[0]);
                max = int.Parse(args[1]) + 1; //including upper limit

                var charactersNum = new Random().Next(min, max);
                for (int i = 0; i < charactersNum; i++)
                    AddRandomCharacter(gameEvent, args.Skip(2).ToArray());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Event command parsing error. Wrong arguments. error: {ex.Message}");
            }
        }

        private void AddRandomCharacter(Event gameEvent, string[]? args)
        {
            if (args is null)
            {
                _logger.LogWarning($"Event command parsing error. Arguments are null");
                return;
            }

            CharacterType characterType = CharacterType.Other;
            if (!Enum.TryParse(args[0], out characterType))
                _logger.LogWarning($"Event command parsing error. Can't read character type to create. Using default instead.");
            var character = _characterService.CreateRandomCharacter(characterType,CharacterPersistence.Temporary);
            character.Location = gameEvent.LocationId;
            gameEvent.CharacterIds.Add(character.Id);
        }
        private void AddRandomItems(Event gameEvent, string[]? args)
        {
            if (args is null)
            {
                _logger.LogWarning($"Event command parsing error. Arguments are null");
                return;
            }

            int min = 0;
            int max = 0;
            try
            {
                min = int.Parse(args[0]);
                max = int.Parse(args[1]) + 1; //including upper limit
                var charactersNum = new Random().Next(min, max);
                for (int i = 0; i < charactersNum; i++)
                {
                    var item = _itemService.CreateDefaultItem(); //ToDo: create and replace with CreateRandomItem(ItemType type)
                    gameEvent.ItemIds.Add(item.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Event command parsing error. Wrong arguments. error: {ex.Message}");
            }
        }
    }

}

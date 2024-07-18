using AutoMapper;
using GlobalServices.Entities;
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
                    case "AddRandomCharacters":
                        AddRandomCharacters(gameEvent, args);
                        break;
                    case "AddRandomMonsters":
                        AddRandomMonsters(gameEvent, args);
                        break;
                    case "AddRandomItems":
                        AddRandomItems(gameEvent, args);
                        break;
                    default:
                        Console.WriteLine("Unknown command");
                        break;
                }
            }
        }
        private void AddRandomCharacters(Event gameEvent, string[]? args)
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
                    var character = _characterService.CreateRandomTemporalCivilian();
                    gameEvent.CharacterIds.Add(character.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Event command parsing error. Wrong arguments. error: {ex.Message}");
            }
        }

        private void AddRandomMonsters(Event gameEvent, string[]? args)
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
                    var character = _characterService.CreateRandomMonster();
                    gameEvent.CharacterIds.Add(character.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Event command parsing error. Wrong arguments. error: {ex.Message}");
            }
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

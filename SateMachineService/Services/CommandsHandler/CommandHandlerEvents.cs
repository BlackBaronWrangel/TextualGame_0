using GlobalServices.Entities;
using GlobalServices.Enums;
using GlobalServices.Interfaces;
using System.Reflection;
using System.Text.RegularExpressions;

namespace GlobalServices
{
    public partial class CommandHandler : ICommandHandler
    {
        public bool TryExecuteSceneConditionCommand(string command)
        {
            var match = Match(command);
            if (match.Success)
            {
                var commandName = match.Groups[1].Value;
                var args = match.Groups[2].Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                switch (commandName)
                {
                    case "IfPlayerHasItem":
                        return IfPlayerHasItem(args);
                    default:
                        _logger.LogError($"Unknown command {command}");
                        return false;
                }
            }
            return false;
        }
        public void TryExecuteEventCommand(Event gameEvent, string command)
        {
            var match = Match(command);
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
                    case "AddRandomItemsCount":
                        AddRandomItemsCount(gameEvent, args);
                        break;
                    default:
                        _logger.LogError($"Unknown command {command}");
                        break;
                }
            }
        }
        public void TryExecuteEventContextCommand(Event gameEvent, object context, string command)
        {
            var match = Match(command);
            if (match.Success)
            {
                var commandName = match.Groups[1].Value;
                var args = match.Groups[2].Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                switch (commandName)
                {
                    case "SelectRandomNextEvent2":
                        AddRandomNextEvent2(gameEvent, context, args);
                        break;
                    default:
                        _logger.LogError($"Unknown command {command}");
                        break;
                }
            }
        }

        private Match Match(string command) => Regex.Match(command.Trim(), @"@(\w+)\(([^)]*)\)");
        private bool IfPlayerHasItem(string[]? args)
        {
            var expectedArgsCount = 1;
            string methodName = MethodBase.GetCurrentMethod()?.Name ?? "UnknownMethod";
            if (args == null || args.Length == 0 || args.Length != expectedArgsCount)
            {
                _logger.LogWarning($"Invalid arguments for {methodName}. Ignoring command.");
                return false;
            }

            try
            {
                var itemId = args[0];
                var player = _characterService.GetPlayer();
                if (string.IsNullOrEmpty(itemId))
                {
                    _logger.LogWarning($"Invalid arguments for {methodName}. ItemId is null or empty.");
                    return false;
                }
                if (player is null)
                {
                    _logger.LogError($"Error in {methodName}. can't get player.");
                    return false;
                }
                if (player.Items.Any(i => i.Id == itemId))
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Event command parsing error for method {methodName}. Wrong arguments. error: {ex.Message}");
                return false;
            }
        }
        private void AddRandomNextEvent2(Event gameEvent, object context, string[]? args)
        {
            var expectedArgsCount = 3;
            string methodName = MethodBase.GetCurrentMethod()?.Name ?? "UnknownMethod";
            var kvp = context as KeyValuePair<string, string>?;
            if (kvp is null)
            {
                _logger.LogWarning($"Invalid arguments for {methodName}. Can't cast context to KeyValuePair. Ignoring command.");
                return;
            }
            if (args == null || args.Length == 0 || args.Length != expectedArgsCount)
            {
                _logger.LogWarning($"Invalid arguments for {methodName}. Ignoring command.");
                return;
            }

            try
            {
                string eventOption1 = args[0].ToString().Trim();
                string eventOption2 = args[1].ToString().Trim();
                double probability = double.Parse(args[2]);

                if (probability < 0 || probability > 1)
                {
                    _logger.LogWarning($"Invalid probability value: {probability} for next events of event: {gameEvent.Id}. Must be between 0 and 1.");
                    return;
                }

                Random random = new Random();
                string selectedEventId = random.NextDouble() < probability ? eventOption1 : eventOption2;

                var key = kvp.Value.Key;
                if (gameEvent.PossibleNextEvents.ContainsKey(key))
                    gameEvent.PossibleNextEvents[key] = selectedEventId;
                else
                    _logger.LogWarning($"Can't rewrite PossibleNextEvent value in method: {methodName}. Ignoring command.");
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Event command parsing error for method {methodName}. Wrong arguments. error: {ex.Message}");
            }
        }
        private void AddRandomCharactersCount(Event gameEvent, string[]? args)
        {
            var expectedArgsCount = 3;
            string methodName = MethodBase.GetCurrentMethod()?.Name ?? "UnknownMethod";
            if (args == null || args.Length == 0 || args.Length != expectedArgsCount)
            {
                _logger.LogWarning($"Invalid arguments for {methodName}. Ignoring command.");
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
                _logger.LogError($"Event command parsing error for method {methodName}. Wrong arguments. error: {ex.Message}");
            }
        }
        private void AddRandomCharacter(Event gameEvent, string[]? args)
        {
            var expectedArgsCount = 1;
            string methodName = MethodBase.GetCurrentMethod()?.Name ?? "UnknownMethod";
            if (args == null || args.Length == 0 || args.Length != expectedArgsCount)
            {
                _logger.LogWarning($"Invalid arguments for {methodName}. Ignoring command.");
                return;
            }

            CharacterType characterType = CharacterType.Other;
            if (!Enum.TryParse(args[0], out characterType))
                _logger.LogWarning($"Event command parsing error. Can't read character type to create. Using default instead.");
            var character = _characterService.CreateRandomCharacter(characterType, CharacterPersistence.Temporary);
            character.Location = gameEvent.LocationId;
            gameEvent.CharacterIds.Add(character.Id);
        }
        private void AddRandomItemsCount(Event gameEvent, string[]? args)
        {
            var expectedArgsCount = 3;
            string methodName = MethodBase.GetCurrentMethod()?.Name ?? "UnknownMethod";
            if (args == null || args.Length == 0 || args.Length != expectedArgsCount)
            {
                _logger.LogWarning($"Invalid arguments for {methodName}. Ignoring command.");
                return;
            }

            int min = 0;
            int max = 0;
            ItemType itemType = ItemType.Other;

            try
            {
                min = int.Parse(args[0]);
                max = int.Parse(args[1]) + 1; //including upper limit
                itemType = (ItemType)Enum.Parse(typeof(ItemType), args[2], true);

                var itemsNum = new Random().Next(min, max);
                for (int i = 0; i < itemsNum; i++)
                {
                    var item = _itemService.CreateRandomItem(itemType); //ToDo: create and replace with CreateRandomItem(ItemType type)
                    gameEvent.ItemIds.Add(item.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Event command parsing error for method {methodName}. Wrong arguments. error: {ex.Message}");
            }
        }
    }
}

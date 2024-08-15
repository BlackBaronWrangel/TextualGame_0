using GlobalServices.Entities;
using GlobalServices.Enums;
using GlobalServices.Interfaces;

namespace GlobalServices
{
    public class ConfrontationSM : ISubStateMachine
    {
        private ICharacterService _characterService;
        private ILogger _logger;

        public int CurrentTurn { get; protected set; }
        public SubStateStatus Status {get;set;}
        public List<ICharacterWrapper> Characters { get; set; }
        public Event ParentEvent { get; set; }
        public List<string> SsmLogs { get ;}

        public event EventHandler StateChanged = delegate { };


        public ConfrontationSM(Event gameEvent, ICharacterService characterService, ILogger logger)
        {
            _characterService = characterService;
            _logger = logger;

            ParentEvent = gameEvent;
            Status = SubStateStatus.Running;
            CurrentTurn = 0;    
            SsmLogs = new List<string>();
            Characters = LoadCharacters(gameEvent);
        }

        public void NextTurn()
        {
            CurrentTurn++;
            foreach (var chr in Characters)
            {
                var character = chr as ConfrontationCharacter;
                if (Dice.Roll(19, 20) 
                    && character is not null
                    && character.Status is not CharacterConfrontationStatus.Yield)
                {
                    //SetRandomIntent(chr)
                    ProcessCharacterStatus(character);
                    FulfillIntent(character);
                    character.Intent = null;
                }
            }
            OnStateChanged();
        }


        private List<ICharacterWrapper> LoadCharacters(Event gameEvent)
        {
            List<ICharacterWrapper> characterList = new();

            var gameEventCharacterIds = new List<string>(gameEvent.CharacterIds);
            gameEventCharacterIds.Insert(0,_characterService.GetPlayer()!.Id);

            foreach (var characterid in gameEventCharacterIds)
            {
                var baseCharacter = _characterService.GetCharacter(characterid);
                if (baseCharacter is not null)
                    characterList.Add(new ConfrontationCharacter(baseCharacter));
            }

            return characterList;
        }
        private void FulfillIntent(ConfrontationCharacter? character)
        {
            if (character?.Intent is null) return;
            Action<ConfrontationCharacter> intentAction = character.Intent.IntentType switch
            {
                IntentType.Strike => DoStrike,
                IntentType.Throw => DoThrow,
                IntentType.Dodge => DoDodge,
                IntentType.Plead => DoPlead,
                _ => _ => { }
            };
            intentAction(character);
        }


        private void DoStrike(ConfrontationCharacter chr)
        {
            var character = chr.BaseCharacter;
            var intent = chr.Intent;

            var targetChar = Characters.FirstOrDefault(c => c.BaseCharacter.Id == intent?.Target);
            if (targetChar is null)
            {
                _logger.LogWarning($"Error in {intent?.IntentType} intent handling. Target is null.");
                return;
            }

            if (Dice.Roll(10, 20))
            {
                var damage = Dice.RollValue(50); //ToDo: rewrite to make conditional damage
                targetChar.BaseCharacter.Hp = Math.Max(0, targetChar.BaseCharacter.Hp - damage);

                var logText = $"\n{character.Species} striked {targetChar.BaseCharacter.Species} and caused {damage} damage. Current target hp = {targetChar.BaseCharacter.Hp}";
                SsmLogs.Add(logText);
                _logger.LogInfo(logText);
            }
            else 
            {
                var logText = $"\n{character.Species} attempted to strike {targetChar.BaseCharacter.Species}, but missed";
                SsmLogs.Add(logText);
                _logger.LogInfo(logText);
            }

        }
        private void DoThrow(ConfrontationCharacter chr)
        {
            var character = chr.BaseCharacter;
            var intent = chr.Intent;
        }
        private void DoDodge(ConfrontationCharacter chr)
        {
            var character = chr.BaseCharacter;
            var intent = chr.Intent;
        }
        private void DoPlead(ConfrontationCharacter chr)
        {
            var character = chr.BaseCharacter;
            var intent = chr.Intent;
        }

        private void ProcessCharacterStatus(ConfrontationCharacter chr)
        {
            if (chr.BaseCharacter.Hp <= 0)
            {
                chr.Status = CharacterConfrontationStatus.Yield;
                SsmLogs.Add($"\n{chr.BaseCharacter.Species} passed out");
            }
        }

        protected virtual void OnStateChanged() => StateChanged?.Invoke(this, EventArgs.Empty);


    }
}

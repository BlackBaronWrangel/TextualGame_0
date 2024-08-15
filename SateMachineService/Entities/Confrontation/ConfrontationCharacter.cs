using GlobalServices.Enums;
using GlobalServices.Interfaces;

namespace GlobalServices.Entities
{
    public class ConfrontationCharacter : ICharacterWrapper
    {
        public Character BaseCharacter { get; private set; }
        public CharacterConfrontationStatus Status { get; set; }

        public Intent? Intent { get; set; }

        public ConfrontationCharacter(Character character)
        {
            BaseCharacter = character;
            Status = CharacterConfrontationStatus.Active;
            Intent = null;
        }
    }
}
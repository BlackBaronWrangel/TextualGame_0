
using GlobalServices.Enums;

namespace GlobalServices.Entities
{
    public class ConfrontationCharacter
    {
        public Character BaseCharacter { get; private set; }
        public CharacterConfrontationStatus Status { get; private set; }

        public Intent? Intent { get; set; }

        public ConfrontationCharacter(Character character)
        {
            BaseCharacter = character;
            Status = CharacterConfrontationStatus.Active;
            Intent = null;
        }
    }
}
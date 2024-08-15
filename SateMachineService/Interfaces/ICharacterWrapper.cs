using GlobalServices.Entities;

namespace GlobalServices.Interfaces
{
    public interface ICharacterWrapper
    {
        public Character BaseCharacter { get; }
        public Intent? Intent { get; set; }
    }
}

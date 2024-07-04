using GlobalServices.Enums;
using GlobalServices.Interfaces;
using GlobalServices.Tags;

namespace GlobalServices.Entities
{
    public class Character : CharacterBase
    {
        private readonly Guid _id = Guid.NewGuid();

        //"Static" properties
        public string Id { get => _id.ToString();}
        public string Name { get; protected set; } = String.Empty;
        public CharacterType Type { get; protected set; } = CharacterType.Other;
        public CharacterPersistence Persistence { get; protected set; } = CharacterPersistence.Temporary;
        public CharacterBodyType BodyType { get; protected set; } = CharacterBodyType.Other;
        public CharacterSpecies Species { get; protected set; } = CharacterSpecies.Other;
        public CharacterGender Gender { get; protected set; } = CharacterGender.Other;

        //"Dynamic" properties
        public CharacterControlType ControlType { get; set; } = CharacterControlType.PlayableNpc;
        public CharacterStatus Status { get; set; } = CharacterStatus.Alive;
        public Location? Location { get; set; } = null;
        public int Hp { get; set; } = 100;
        public int Mental { get; set; } = 100;
        public int HpLimit { get; set; } = 100;
        public int MentalLimit { get; set; } = 100;
        public HashSet<Item> Items { get; set; } = new();
        public Character() : base(){}
        public Character(string name, CharacterType type, CharacterPersistence persistence, CharacterBodyType bodyType, CharacterSpecies species, CharacterGender gender) : base()
        {
            Name = name;
            Type = type;
            Persistence = persistence;
            BodyType = bodyType;
            Species = species;
            Gender = gender;
        }
        public Character(string name, CharacterType type, CharacterPersistence persistence, CharacterBodyType bodyType, CharacterSpecies species, CharacterGender gender, HashSet<ITag> tags) : base(tags)
        {
            Name = name;
            Type = type;
            Persistence = persistence;
            BodyType = bodyType;
            Species = species;
            Gender = gender;
        }
        public override string ToString()
        {
            string description = $"{Id} [{Persistence},{Type},{ControlType}]";
            return description;
        }

    }
}

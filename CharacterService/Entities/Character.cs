using GlobalServices.Enums;
using GlobalServices.Interfaces;
using System.Security.Cryptography;

namespace GlobalServices.Entities
{
    public class Character : CharacterBase
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        //"Static" properties
        public string Name { get; set; } = String.Empty;
        public CharacterType Type { get; set; } = CharacterType.Other;
        public CharacterPersistence Persistence { get; set; } = CharacterPersistence.Temporary;
        public CharacterBodyType BodyType { get; set; } = CharacterBodyType.Other;
        public CharacterSpecies Species { get; set; } = CharacterSpecies.Other;
        public CharacterGender Gender { get; set; } = CharacterGender.Other;

        //"Dynamic" properties
        public CharacterControlType ControlType { get; set; } = CharacterControlType.PlayableNpc;
        public CharacterStatus Status { get; set; } = CharacterStatus.Alive;
        public string? Location { get; set; } = string.Empty;
        public int Hp { get; set; } = 100;
        public int Mental { get; set; } = 100;
        public int HpLimit { get; set; } = 100;
        public int MentalLimit { get; set; } = 100;
        public HashSet<Item> Items { get; set; } = new();

        public HashSet<string> CustomAttributes { get; set; } = new(); //Custom attributes should contain more technical tags. Like buffs/debuffs, quest completion marks and so on.

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
        public override string ToString() => $"{GetType().Name} [{Id}, {Persistence}, {Type}, {ControlType}]";

    }
}

using GlobalServices.Enums;
using GlobalServices.Interfaces;
using Newtonsoft.Json;

namespace GlobalServices.Tags
{
    public class ItemTag : TagBase
    {
        public override TagType TagType { get => TagType.ItemTag; }
        public ItemTag(string id, string name, string description) : base()
        {
            Id = id;
            Name = name;
            Description = description;
        }
    }
}

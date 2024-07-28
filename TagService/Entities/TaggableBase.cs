using GlobalServices.Interfaces;

namespace GlobalServices.Entities
{
    public abstract class TaggableBase : ITaggable
    {
        public HashSet<string> Tags { get; set; }
        protected TaggableBase(HashSet<string> tags) => Tags = tags;
        protected TaggableBase() => Tags = new HashSet<string>();
        public void AddTag(string tag) => Tags.Add(tag);
        public void RemoveTag(string tag) => Tags.Remove(tag);
    }
}

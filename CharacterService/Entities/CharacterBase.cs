using GlobalServices.Interfaces;

namespace GlobalServices.Entities
{
    public class CharacterBase : ITaggable
    {
        public HashSet<ITag> Tags { get; set; }
        protected CharacterBase(HashSet<ITag> tags)
        {
            Tags = tags;
        }
        protected CharacterBase()
        {
            Tags = new HashSet<ITag>();
        }
        public void AddTag(ITag tag)
        {
            Tags.Add(tag);
        }
        public void RemoveTag(ITag tag)
        {
            Tags.Remove(tag);
        }
    }
}

using GlobalServices.Interfaces;

namespace GlobalServices.Entities
{
    public class ItemBase : ITaggable
    {
        public HashSet<ITag> Tags { get; set; }
        protected ItemBase(HashSet<ITag> tags)
        {
            Tags = tags;
        }
        protected ItemBase()
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

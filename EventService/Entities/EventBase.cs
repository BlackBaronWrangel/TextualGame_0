using GlobalServices.Interfaces;

namespace GlobalServices.Entities
{
    public class EventBase : ITaggable
    {
        public HashSet<ITag> Tags { get; protected set; }
        protected EventBase(HashSet<ITag> tags)
        {
            Tags = tags;
        }
        protected EventBase()
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

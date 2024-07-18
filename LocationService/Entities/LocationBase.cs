using GlobalServices.Interfaces;

namespace GlobalServices.Entities
{
    public class LocationBase : ITaggable
    {
        public HashSet<ITag> Tags { get; set; }
        protected LocationBase(HashSet<ITag> tags)
        {
            Tags = tags;
        }
        protected LocationBase()
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

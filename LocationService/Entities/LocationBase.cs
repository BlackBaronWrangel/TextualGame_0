using GlobalServices.Interfaces;

namespace GlobalServices.Entities
{
    public class LocationBase : ITaggable
    {
        public List<ITag> Tags { get; protected set; }
        protected LocationBase(List<ITag> tags)
        {
            Tags = tags;
        }
        protected LocationBase()
        {
            Tags = new List<ITag>();
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

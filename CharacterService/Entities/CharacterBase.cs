using GlobalServices.Interfaces;

namespace GlobalServices.Entities
{
    public class CharacterBase : ITaggable
    {
        public List<ITag> Tags { get; protected set; }
        protected CharacterBase(List<ITag> tags)
        {
            Tags = tags;
        }
        protected CharacterBase()
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

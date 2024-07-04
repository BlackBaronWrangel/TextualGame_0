
namespace GlobalServices.Interfaces
{
    public interface ITaggable
    {
        HashSet<ITag> Tags { get; }
        void AddTag(ITag tag);
        void RemoveTag(ITag tag);
    }
}

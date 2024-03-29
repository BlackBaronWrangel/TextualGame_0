
namespace GlobalServices.Interfaces
{
    public interface ITaggable
    {
        List<ITag> Tags { get; }
        void AddTag(ITag tag);
        void RemoveTag(ITag tag);
    }
}

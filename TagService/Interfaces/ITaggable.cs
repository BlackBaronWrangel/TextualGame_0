
namespace GlobalServices.Interfaces
{
    public interface ITaggable
    {
        HashSet<ITag> Tags { get; set; }
        void AddTag(ITag tag);
        void RemoveTag(ITag tag);
    }
}

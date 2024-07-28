using GlobalServices.Entities;

namespace GlobalServices.Interfaces
{
    public interface ITagService
    {
        HashSet<Tag> Tags { get; }

        HashSet<ITaggable> TaggableEntities { get;}

        ITag? GetTag(string tagId);

        void RegisterITaggable(ITaggable obj);
        void UnregisterITaggable(ITaggable obj);
        bool ValidateITaggables();
    }
}

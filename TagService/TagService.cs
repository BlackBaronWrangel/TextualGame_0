using GlobalServices.Entities;
using GlobalServices.Interfaces;

namespace GlobalServices
{
    public class TagService : ITagService
    {
        private const string _defaultJsonTagsPath = "Resources/Tags";

        private ILogger _logger;
        public HashSet<Tag> Tags { get; protected set; }
        public HashSet<ITaggable> TaggableEntities { get; protected set; }

        public TagService(ILogger logger)
        {
            TaggableEntities = new HashSet<ITaggable>();
            _logger = logger;

            Tags = new ();
            InitTags();
        }
        public ITag? GetTag(string id) => Tags.FirstOrDefault(tag => tag.Id == id);
        public void RegisterITaggable(ITaggable obj)
        {
            if (!TaggableEntities.Contains(obj))
            {
                TaggableEntities.Add(obj);
            }
            else
            {
                _logger.LogWarning($"Can't register new ITaggable, object {obj} is already registered.");
            }
        }
        public void UnregisterITaggable(ITaggable obj)
        {
            if (TaggableEntities.Contains(obj))
            {
                TaggableEntities.Remove(obj);
            }
            else
            {
                _logger.LogWarning($"Can't unregister  ITaggable, object {obj} isn't registered.");
            }
        }
        public bool ValidateITaggables()
        {
            bool result = true;
            foreach (ITaggable obj in TaggableEntities)
            {
                if (obj.Tags.Count() == 0)
                {
                    _logger.LogWarning($"{obj} has no tags.");
                }
            }
            return result;
        }
        private void InitTags() => Tags = GameEntitiesJsonLoader.ReadFolderAsCollection<Tag>(_defaultJsonTagsPath).ToHashSet();

    }
}
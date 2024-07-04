using GlobalServices.Enums;
using GlobalServices.Interfaces;
using GlobalServices.Resources;
using GlobalServices.Tags;
using System.Reflection;
using System.Resources;

namespace GlobalServices
{
    public class TagService : ITagService
    {

        private ILogger _logger;
        public HashSet<ITag> Tags { get; }
        public HashSet<ITag> LocationTags { get => Tags.Where(tag => tag.TagType == TagType.LocationTag).ToHashSet(); }
        public HashSet<ITag> CharacterTags { get => Tags.Where(tag => tag.TagType == TagType.CharacterTag).ToHashSet(); }
        public HashSet<ITag> EventTags { get => Tags.Where(tag => tag.TagType == TagType.EventTag).ToHashSet(); }
        public HashSet<ITaggable> TaggableEntities { get; protected set; }

        public TagService(ILogger logger)
        {
            TaggableEntities = new HashSet<ITaggable>();
            _logger = logger;

            Tags = new HashSet<ITag>();
            InitLocationsTags();
            InitCharactersTags();
            InitEventsTags();
            InitItemsTags();
            //init other tags
        }
        public ITag? GetLocationTag(TagId.Location tagId)
        {
            return LocationTags.FirstOrDefault(tag => tag.Name == tagId.ToString());
        }
        public ITag? GetCharacterTag(TagId.Character tagId)
        {
            return CharacterTags.FirstOrDefault(tag => tag.Name == tagId.ToString());
        }
        public ITag? GetEventTag(TagId.Event tagId)
        {
            return EventTags.FirstOrDefault(tag => tag.Name == tagId.ToString());
        }
        public ITag? GetItemTag(TagId.Item tagId)
        {
            return EventTags.FirstOrDefault(tag => tag.Name == tagId.ToString());
        }
        public ITag? GetTagById(string id)
        {
            return Tags.FirstOrDefault(tag => tag.Name == id);
        }
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
                    _logger.LogWarning($"ITaggable {obj} has no tags.");
                    return result;
                }
            }
            return result;
        }

        private void InitLocationsTags()
        {
            ResourceManager rm = new ResourceManager("TagService.Resources.LocationTagsDescription", Assembly.GetExecutingAssembly());
            InitTags<TagId.Location, LocationTag>(rm);
        }
        private void InitCharactersTags()
        {
            ResourceManager rm = new ResourceManager("TagService.Resources.CharacterTagsDescription", Assembly.GetExecutingAssembly());
            InitTags<TagId.Character, CharacterTag>(rm);
        }
        private void InitEventsTags()
        {
            ResourceManager rm = new ResourceManager("TagService.Resources.EventTagsDescription", Assembly.GetExecutingAssembly());
            InitTags<TagId.Event, EventTag>(rm);
        }
        private void InitItemsTags()
        {
            ResourceManager rm = new ResourceManager("TagService.Resources.ItemsTagsDescription", Assembly.GetExecutingAssembly());
            InitTags<TagId.Item, ItemTag>(rm);
        }

        private void InitTags<T1, T2>(ResourceManager rm)
        {
            foreach (var item in Enum.GetValues(typeof(T1)))
            {
                var tagId = item.ToString();
                if (string.IsNullOrEmpty(tagId))
                {
                    _logger.LogError($"Can't get Tag {item}.");
                    continue;
                }

                var resDescription = String.Empty;
                var tagDescription = String.Empty;
                try
                {
                    resDescription = rm.GetString(tagId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());
                }

                if (string.IsNullOrEmpty(resDescription)) _logger.LogError("Can't get description from resources.");
                else tagDescription = resDescription;

                if (!typeof(ITag).IsAssignableFrom(typeof(T2)))
                {
                    _logger.LogError($"The type {typeof(T2).Name} does not implement the ITag interface.");
                    continue;
                }

                var ctor = typeof(T2).GetConstructor(new[] { typeof(string), typeof(string) });
                if (ctor is not null)
                {
                    ITag tag = (ITag)(T2)ctor.Invoke(new object[] { tagId, tagDescription });
                    Tags.Add(tag);
                }
                else
                {
                    _logger.LogError($"No suitable constructor found for {typeof(T2).Name}");
                }
            }
        }

    }
}
using GlobalServices.Enums;
using GlobalServices.Interfaces;
using GlobalServices.Tags;
using System.Reflection;
using System.Resources;

namespace GlobalServices
{
    public class TagService : ITagService
    {

        private List<ITaggable> _taggableEntities;
        private ILogger _logger;
        public List<ITag> Tags { get; }
        public List<ITag> LocationTags { get => Tags.Where(tag => tag.TagType == TagType.LocationTag).ToList(); }
        public List<ITag> CharacterTags { get => Tags.Where(tag => tag.TagType == TagType.CharacterTag).ToList(); }
        public List<ITag> EventTags { get => Tags.Where(tag => tag.TagType == TagType.EventTag).ToList(); }

        public TagService(ILogger logger)
        {
            _taggableEntities = new List<ITaggable>();
            _logger = logger;

            Tags = new List<ITag>();
            InitLocationsTags();
            InitCharactersTags();
            InitEventsTags();
            //init other tags
        }
        public ITag? GetLocationTag(TagId.Location tagId)
        {
            return LocationTags.FirstOrDefault(tag => tag.Id == tagId.ToString());
        }
        public ITag? GetCharacterTag(TagId.Character tagId)
        {
            return CharacterTags.FirstOrDefault(tag => tag.Id == tagId.ToString());
        }
        public ITag? GetEventTag(TagId.Event tagId)
        {
            return EventTags.FirstOrDefault(tag => tag.Id == tagId.ToString());
        }
        public ITag? GetTagById(string id)
        {
            return Tags.FirstOrDefault(tag => tag.Id == id);
        }
        public void RegisterITaggable(ITaggable obj)
        {
            if (!_taggableEntities.Contains(obj))
            {
                _taggableEntities.Add(obj);
            }
            else
            {
                _logger.LogWarning($"Can't register new ITaggable, object {obj} is already registered.");
            }
        }
        public void UnregisterITaggable(ITaggable obj)
        {
            if (_taggableEntities.Contains(obj))
            {
                _taggableEntities.Remove(obj);
            }
            else
            {
                _logger.LogWarning($"Can't unregister  ITaggable, object {obj} isn't registered.");
            }
        }
        public bool ValidateITaggables()
        {
            bool result = true;
            foreach (ITaggable obj in _taggableEntities)
            {
                if (obj.Tags.Count() == 0)
                {
                    _logger.LogWarning($"ITaggable {obj} has no tags.");
                    result = false;
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
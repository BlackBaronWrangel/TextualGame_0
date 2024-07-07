using GlobalServices.Enums;
using GlobalServices.Interfaces;
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
        public HashSet<ITag> ItemTags { get => Tags.Where(tag => tag.TagType == TagType.ItemTag).ToHashSet(); }
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
        public ITag? GetLocationTag(TagId.LocationTagId tagId) => LocationTags.FirstOrDefault(tag => tag.Id == tagId.ToString());
        public ITag? GetCharacterTag(TagId.CharacterTagId tagId) =>  CharacterTags.FirstOrDefault(tag => tag.Id == tagId.ToString());        
        public ITag? GetEventTag(TagId.EventTagId tagId) => EventTags.FirstOrDefault(tag => tag.Id == tagId.ToString());        
        public ITag? GetItemTag(TagId.ItemTagId tagId) => EventTags.FirstOrDefault(tag => tag.Id == tagId.ToString());        
        public ITag? GetTagById(string id) => Tags.FirstOrDefault(tag => tag.Id == id);
        
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

        private void InitLocationsTags()
        {
            var tags = TagJsonReader.ReadLocationTagsFromJson();
            InitTags<TagId.LocationTagId, LocationTag>(tags);
        }
        private void InitCharactersTags()
        {
            var tags = TagJsonReader.ReadCharacterTagsFromJson();
            InitTags<TagId.CharacterTagId, CharacterTag>(tags);
        }
        private void InitEventsTags()
        {
            var tags = TagJsonReader.ReadEventTagsFromJson();
            InitTags<TagId.EventTagId, EventTag>(tags);
        }
        private void InitItemsTags()
        {
            var tags = TagJsonReader.ReadItemTagsFromJson();
            InitTags<TagId.ItemTagId, ItemTag>(tags);
        }

        /// <summary>
        /// Inits tags
        /// </summary>
        /// <typeparam name="T1">Tag name from enum</typeparam>
        /// <typeparam name="T2">Tag Type to cast</typeparam>
        /// <param name="tags">List of ITag</param>
        private void InitTags<T1, T2>(HashSet<TagBase>? tags)
        {
            if (tags is null || !tags.Any())
            {
                _logger.LogError($"Can't get tags: {typeof(T2).Name} from resources.");
                return;
            }

            foreach (var item in Enum.GetValues(typeof(T1)))
            {
                var tagid = item.ToString();
                if (string.IsNullOrEmpty(tagid))
                {
                    _logger.LogError($"Can't convert tag from Enum to string. Tags set: {typeof(T2).Name}");
                    continue;
                }

                var resName = tags.FirstOrDefault(t => t.Id == tagid)?.Name;
                var resDescription = tags.FirstOrDefault(t => t.Id == tagid)?.Description;

                var tagName = String.Empty;
                var tagDescription = String.Empty;

                if (string.IsNullOrEmpty(resDescription) || string.IsNullOrEmpty(resName))
                    _logger.LogError($"Can't get tags from resources. Tags set: {typeof(T2).Name}");
                else
                {
                    tagName = resName;
                    tagDescription = resDescription;
                }

                if (!typeof(ITag).IsAssignableFrom(typeof(T2)))
                {
                    _logger.LogError($"The type {typeof(T2).Name} does not implement the ITag interface.");
                    continue;
                }

                var ctor = typeof(T2).GetConstructor(new[] { typeof(string), typeof(string), typeof(string) });
                if (ctor is not null)
                {
                    ITag tag = (ITag)(T2)ctor.Invoke(new object[] { tagid, tagName, tagDescription });
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
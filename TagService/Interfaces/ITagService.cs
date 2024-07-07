using GlobalServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalServices.Interfaces
{
    public interface ITagService
    {
        HashSet<ITag> Tags { get; }
        HashSet<ITag> LocationTags { get; }
        HashSet<ITag> CharacterTags { get; }
        HashSet<ITag> EventTags { get; }

        HashSet<ITaggable> TaggableEntities { get;}

        ITag? GetLocationTag(TagId.LocationTagId tagId);
        ITag? GetCharacterTag(TagId.CharacterTagId tagId);
        ITag? GetEventTag(TagId.EventTagId tagId);
        ITag? GetItemTag(TagId.ItemTagId tagId);
        ITag? GetTagById(string tagName);

        void RegisterITaggable(ITaggable obj);
        void UnregisterITaggable(ITaggable obj);
        bool ValidateITaggables();
    }
}

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
        List<ITag> Tags { get; }
        List<ITag> LocationTags { get; }
        List<ITag> CharacterTags { get; }
        List<ITag> EventTags { get; }

        ITag? GetLocationTag(TagId.Location tagId);
        ITag? GetCharacterTag(TagId.Character tagId);
        ITag? GetEventTag(TagId.Event tagId);
        ITag? GetTagById(string tagName);

        void RegisterITaggable(ITaggable obj);
        void UnregisterITaggable(ITaggable obj);
        bool ValidateITaggables();
    }
}

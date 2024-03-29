using GlobalServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalServices.Entities
{
    public class ItemBase : ITaggable
    {
        public List<ITag> Tags { get; protected set; }
        protected ItemBase(List<ITag> tags)
        {
            Tags = tags;
        }
        protected ItemBase()
        {
            Tags = new List<ITag>();
        }
        public void AddTag(ITag tag)
        {
            Tags.Add(tag);
        }
        public void RemoveTag(ITag tag)
        {
            Tags.Remove(tag);
        }
    }
}

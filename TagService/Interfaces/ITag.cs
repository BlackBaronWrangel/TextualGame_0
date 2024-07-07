using GlobalServices.Enums;

namespace GlobalServices.Interfaces
{
    public interface ITag
    {
        string Id { get; }
        string Name { get; }
        TagType TagType { get; }
        string Description { get; }
        string ToString();
    }
}

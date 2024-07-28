namespace GlobalServices.Interfaces
{
    public interface ITag
    {
        string Id { get; }
        string Name { get; }
        string Description { get; }
        string ToString();
    }
}

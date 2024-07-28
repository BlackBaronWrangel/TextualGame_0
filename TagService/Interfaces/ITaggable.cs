namespace GlobalServices.Interfaces
{
    public interface ITaggable
    {
        HashSet<string> Tags { get; set; }
        void AddTag(string tagId);
        void RemoveTag(string tagId);
    }
}

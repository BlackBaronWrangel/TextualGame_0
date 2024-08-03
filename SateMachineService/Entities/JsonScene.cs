namespace GlobalServices.Entities
{
    public class JsonScene
    {
        public string Id { get; set; } = string.Empty;
        public string StartEventId { get; set; } = string.Empty;
        public string StartLocationId { get; set; } = string.Empty;
        public string EntryText { get; set; } = string.Empty;
        public List<string> StartConditions { get; set; } = new();
        public List<JsonEvent> Events { get; set; } = new();
    }
}

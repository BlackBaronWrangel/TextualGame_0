namespace DebugConsoleApp.StateMachine.Entities
{
    public class JsonEvent
    {
        public string Id { get; set; } = string.Empty;
        public string EventType { get; set; } = string.Empty;
        public string LocationId { get; set; } = string.Empty;
        public List<string> CharacterIds { get; set; }  = new ();
        public List<string> ItemIds { get; set; } = new ();
        public List<string> PossibleNextEvents { get; set; } = new();
        public List<string> Commands { get; set; } = new();
    }
}

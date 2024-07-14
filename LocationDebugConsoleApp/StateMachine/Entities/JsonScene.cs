namespace DebugConsoleApp.StateMachine.Entities
{
    public class JsonScene
    {
        public string Id { get; set; } = string.Empty;
        public string StartEventId { get; set; } = string.Empty;
        public List<JsonEvent> Events { get; set; } = new();
    }
}

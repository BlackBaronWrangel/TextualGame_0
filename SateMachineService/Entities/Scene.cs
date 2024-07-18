namespace GlobalServices.Entities
{
    public class Scene
    {
        public string Id { get; set; } = Guid.NewGuid().ToString(); //configurable. guid value by default
        public string StartEventId { get; set; } = string.Empty;
        public Scene() { }
        public override string ToString() => $"{GetType().Name} [{Id}, StartEvent: {StartEventId}]";
    }
}

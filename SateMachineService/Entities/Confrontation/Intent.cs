using GlobalServices.Enums;

namespace GlobalServices.Entities
{
    public class Intent
    {
        public IntentType IntentType { get; set; }
        public string? Object { get; set; }
        public string? Target { get; set; }
    }
}

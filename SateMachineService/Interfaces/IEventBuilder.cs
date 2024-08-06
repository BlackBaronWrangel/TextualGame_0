using GlobalServices.Entities;

namespace GlobalServices.Interfaces
{
    public interface IEventBuilder
    {
        KeyValuePair<string, string> BuildConfrontationEvent(Event currentEvent);
    }
}

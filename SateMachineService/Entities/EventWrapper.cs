using GlobalServices.Interfaces;

namespace GlobalServices.Entities
{
    public class EventWrapper
    {
        public Event BaseEvent { get; private set; }
        public ISubStateMachine? SubStateMachine { get; set; }

        public EventWrapper(Event baseEvent)
        {
            BaseEvent = baseEvent;
        }
        public EventWrapper(Event baseEvent, ISubStateMachine subStateMachine)
        {
            BaseEvent = baseEvent;
            SubStateMachine = subStateMachine;
        }
    }
}

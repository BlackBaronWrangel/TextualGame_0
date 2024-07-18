using GlobalServices.Entities;

namespace GlobalServices.Interfaces
{
    public interface ICommandHandler
    {
        void ExecuteEventCommand(Event gameEvent, string command);
    }
}

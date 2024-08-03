using GlobalServices.Entities;

namespace GlobalServices.Interfaces
{
    public interface ICommandHandler
    {
        bool TryExecuteSceneConditionCommand(string command);
        void TryExecuteEventCommand(Event gameEvent, string command);
        void TryExecuteEventContextCommand(Event gameEvent, object context, string command);
    }
}

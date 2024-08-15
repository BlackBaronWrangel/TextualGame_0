using GlobalServices.Entities;
using GlobalServices.Enums;
namespace GlobalServices.Interfaces
{
    public interface ISubStateMachine
    {
        Event ParentEvent { get; protected set; }
        List<ICharacterWrapper> Characters { get; set; }
        public int CurrentTurn { get; }
        SubStateStatus Status { get; set; }
        List<string> SsmLogs { get; }
        void NextTurn();

        event EventHandler StateChanged;
    }
}

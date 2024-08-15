using GlobalServices.Entities;

namespace GlobalServices.Interfaces
{
    public interface IStateMachine
    {
        event EventHandler StateChanged; 
        EventWrapper? CurrentState {get;}
        void RunScene(string sceneId);
        void NextState(string eventId);
    }
}

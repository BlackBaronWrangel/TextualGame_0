using GlobalServices.Entities;

namespace GlobalServices.Interfaces
{
    public interface IStateMachine
    {
        Event CurrentState {get;}
        void RunScene(string sceneId);
        void NextState(string eventId);
    }
}

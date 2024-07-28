using GlobalServices.Entities;

namespace GlobalServices.Interfaces
{
    public interface IStateMachine
    {
        public event EventHandler StateChanged;
        Event? CurrentState {get;}
        void RunScene(string sceneId);
        void NextState(string eventId);
    }
}

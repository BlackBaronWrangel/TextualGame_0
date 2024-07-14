using AutoMapper;
using DebugConsoleApp.StateMachine.Entities;
using GlobalServices.Entities;

namespace GlobalServices
{    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<JsonEvent, Event>();
            CreateMap<JsonScene, Scene>();
        }
    }
}
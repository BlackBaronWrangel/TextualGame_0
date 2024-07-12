using DebugConsoleApp.StateMachine.Entities;
using GlobalServices.Entities;
using GlobalServices.Enums;

namespace DebugConsoleApp.StateMachine.Scenes
{
    public partial class DefinedScenesRepo
    {
        private static Scene TestingScene_0()
        {
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            {
                var id = "test_start_1";
                var locId = "loc_swamp";
                var chars = new HashSet<string> {
                    _characterService.CreateRandomPermanentCivilian().Id,
                    _characterService.CreateRandomPermanentCivilian().Id,
                    _characterService.CreateRandomPermanentCivilian().Id,
                    _characterService.CreateRandomPermanentCivilian().Id,
                    _characterService.CreateRandomPermanentCivilian().Id,
                    _characterService.CreateRandomPermanentCivilian().Id,
                    _characterService.CreateRandomPermanentCivilian().Id,
                    _characterService.CreateRandomPermanentCivilian().Id,
                    _characterService.CreateRandomPermanentCivilian().Id,
                    _characterService.CreateRandomPermanentCivilian().Id,
                    _characterService.CreateRandomPermanentCivilian().Id
                };
                var items = new HashSet<string> {
                    _itemService.CreateDefaultItem().Id,
                    _itemService.CreateDefaultItem().Id,
                    _itemService.CreateDefaultItem().Id,
                    _itemService.CreateDefaultItem().Id,
                    _itemService.CreateDefaultItem().Id
            };
                var events = new HashSet<string>
                {
                    "test_start_2",
                    "test_start_2_alt",
                    "test_start_3",
                };
                _eventService!.CreateEvent(id, locId, EventType.Default, chars, items, events);
            }
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            {
                var id = "test_start_2";
                var locId = "loc_meadow";
                var chars = new HashSet<string> {
                    _characterService.CreateRandomPermanentCivilian().Id
                };
                var items = new HashSet<string> {
                    _itemService.CreateDefaultItem().Id
                };
                var events = new HashSet<string>
                {
                    "test_start_1",
                    "test_start_2_alt"
                };
                _eventService!.CreateEvent(id, locId, EventType.Default, chars, items, events);
            }            
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            {
                var id = "test_start_2_alt";
                var locId = "loc_meadow";
                var chars = new HashSet<string> {
                    _characterService.CreateRandomMonster().Id
                };
                var items = new HashSet<string> {
                    _itemService.CreateDefaultItem().Id
                };
                var events = new HashSet<string>
                {
                    "test_start_3"
                };
                _eventService!.CreateEvent(id, locId, EventType.Default, chars, items, events);
            }            
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            {
                var id = "test_start_3";
                var locId = "loc_forest";
                var chars = new HashSet<string> {
                    _characterService.CreateRandomMonster().Id,
                    _characterService.CreateRandomMonster().Id,
                    _characterService.CreateRandomTemporalCivilian().Id
                };
                var items = new HashSet<string> {
                };
                var events = new HashSet<string>
                {
                    "loc_swamp"
                };
                _eventService!.CreateEvent(id, locId, EventType.Default, chars, items, events);
            }

            return new Scene()
            {
                Id = "TestingScene_0",
                StartEventId = "test_start_1"
            };
        }
    }
}

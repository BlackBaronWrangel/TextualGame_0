using AutoMapper;
using GlobalServices.Interfaces;

namespace GlobalServices
{
    public partial class CommandHandler : ICommandHandler
    {
        private IEventService _eventService;
        private ILogger _logger;
        private ITagService _tagService;
        private ICharacterService _characterService;
        private IItemService _itemService;
        private IMapper _mapper;
        public CommandHandler(
            ILogger logger,
            IEventService eventService,
            ITagService tagService,
            ICharacterService characterService,
            IItemService itemService,
            IMapper mapper
            )
        {
            _logger = logger;
            _eventService = eventService;
            _tagService = tagService;
            _characterService = characterService;
            _itemService = itemService;
            _mapper = mapper;
        }

    }

}

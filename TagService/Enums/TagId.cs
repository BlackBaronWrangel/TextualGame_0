

namespace GlobalServices.Enums
{
    public static class TagId
    {
        public enum Location
        {
            // Location type
            Indoor,
            Outdoor,
            // Location mood modifier
            Scary,
            Neutral,
            Peaceful,
        }

        public enum Character
        {
            // Character mood against player.
            Friendly,
            Neutral,
            Unfriendly,
            Hostile,
        }
        public enum Event
        {
            tag0,
            tag1,
            tag2,
            tag3,
        }

    }
}

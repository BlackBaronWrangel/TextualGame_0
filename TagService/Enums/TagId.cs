

namespace GlobalServices.Enums
{
    public static class TagId
    {
        public enum LocationTagId
        {
            // Location type
            Indoor,
            Outdoor,
            // Location mood modifier
            Scary,
            Neutral,
            Peaceful,
        }
        public enum CharacterTagId
        {
            // Character mood against player.
            Friendly,
            Neutral,
            Unfriendly,
            Hostile,
        }
        public enum EventTagId
        {
            Ending,
        }
        public enum ItemTagId
        {
            HealthRegeneration,
        }
    }
}

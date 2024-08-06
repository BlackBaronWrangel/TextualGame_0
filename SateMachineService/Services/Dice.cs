namespace GlobalServices
{
    public static class Dice
    {
        private static Random random = new Random();

        public static bool Roll(int chances, int max)
        {
            if (chances < 1 || max < 1 || chances > max)
                return false;

            int rollResult = random.Next(1, max + 1);
            return rollResult <= chances;
        }
    }
}
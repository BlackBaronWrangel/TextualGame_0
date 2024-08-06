namespace GlobalServices
{
    public class RandomSelector
    {
        public static T? Select<T>(IEnumerable<T> values)
        {
            if (values == null || !values.Any())
                return default(T);

            int index = new Random().Next(values.Count());
            return values.ElementAt(index);
        }
    }
}

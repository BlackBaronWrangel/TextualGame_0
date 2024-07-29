namespace GlobalServices
{
    public class EnumsHelper
    {

        public static T? GetRandomEnumValue<T>(params T[] excludeValues) where T : Enum
        {
            Random random = new Random();
            var values = Enum.GetValues(typeof(T)).Cast<T>().ToList();
            foreach (var value in excludeValues)
                values.Remove(value);
            if (!values.Any())
                return default;
            return values[random.Next(values.Count)];
        }
    }
}

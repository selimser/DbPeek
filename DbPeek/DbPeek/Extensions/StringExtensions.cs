namespace DbPeek.Extensions
{
    public static class StringExtensions
    {
        public static bool ContainsAny(this string targetString, params string[] searchItems)
        {
            foreach (var needle in searchItems)
            {
                if (targetString.Contains(needle))
                {
                    return true;
                }
            }

            return false;
        }
    }
}

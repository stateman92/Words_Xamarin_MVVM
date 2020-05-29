namespace Words_MVVM.Extensions
{
    public static class StringExtensions
    {
        // Return the original string's substring before the first space.
        public static string RemoveAfterSpace(this string str)
        {
            return str.RemoveAfterChar(' ');
        }

        // Return the original string's substring before the first input character.
        public static string RemoveAfterChar(this string str, char ch)
        {
            return str.Split(ch)[0];
        }

        // Return whether the string is null or empty.
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        // Return whether the string is not null nor empty.
        public static bool IsNotNullNorEmpty(this string str)
        {
            return !str.IsNullOrEmpty();
        }
    }
}

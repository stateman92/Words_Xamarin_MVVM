namespace Words_MVVM.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Return the original string's substring before the first space.
        /// </summary>
        public static string RemoveAfterSpace(this string str)
        {
            return str.RemoveAfterChar(' ');
        }

        /// <summary>
        /// Return the original string's substring before the first input character.
        /// </summary>
        public static string RemoveAfterChar(this string str, char ch)
        {
            return str.Split(ch)[0];
        }

        /// <summary>
        /// Return whether the string is null or empty.
        /// </summary>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// Return whether the string is not null nor empty.
        /// </summary>
        public static bool IsNotNullNorEmpty(this string str)
        {
            return !str.IsNullOrEmpty();
        }
    }
}

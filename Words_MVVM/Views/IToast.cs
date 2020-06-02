namespace Words_MVVM.Views
{
    public interface IToast
    {
        /// <summary>
        /// Show a short Toast-message.
        /// </summary>
        /// <param name="message">The string that will be displayed.</param>
        void Short(string message);

        /// <summary>
        /// Show a long Toast-message.
        /// </summary>
        /// <param name="message">The string that will be displayed.</param>
        void Long(string message);
    }
}

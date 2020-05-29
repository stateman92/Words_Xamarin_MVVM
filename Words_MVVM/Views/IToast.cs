namespace Words_MVVM.Views
{
    public interface IToast
    {
        // Show a short Toast-message.
        void Short(string message);

        // Show a long Toast-message.
        void Long(string message);
    }
}

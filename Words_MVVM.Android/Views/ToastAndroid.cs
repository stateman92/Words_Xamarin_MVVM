using Android.App;
using Android.Widget;

using Words_MVVM.Views;
using Words_MVVM.Droid.Views;

[assembly: Xamarin.Forms.Dependency(typeof(ToastAndroid))]
namespace Words_MVVM.Droid.Views
{
    public class ToastAndroid : IToast
    {
        /// <summary>
        /// Show a long Toast-message.
        /// </summary>
        /// <param name="message">The string that will be displayed.</param>
        public void Long(string message)
        {
            Show(message, ToastLength.Long);
        }

        /// <summary>
        /// Show a short Toast-message.
        /// </summary>
        /// <param name="message">The string that will be displayed.</param>
        public void Short(string message)
        {
            Show(message, ToastLength.Short);
        }

        /// <summary>
        /// Show a Toast-message.
        /// </summary>
        /// <param name="message">The string that will be displayed.</param>
        /// <param name="toastLength">The ToastLength enum, that holds the Toast showing time.</param>
        private void Show(string message, ToastLength toastLength)
        {
            Toast.MakeText(Application.Context, message, toastLength).Show();
        }
    }
}

using Android.App;
using Android.Widget;

using Words_MVVM.Views;
using Words_MVVM.Droid.Views;

[assembly: Xamarin.Forms.Dependency(typeof(ToastAndroid))]
namespace Words_MVVM.Droid.Views
{
    public class ToastAndroid : IToast
    {
        // Show a long Toast-message.
        public void Long(string message)
        {
            Show(message, ToastLength.Long);
        }

        // Show a short Toast-message.
        public void Short(string message)
        {
            Show(message, ToastLength.Short);
        }

        private void Show(string message, ToastLength toastLength)
        {
            Toast.MakeText(Application.Context, message, toastLength).Show();
        }
    }
}

using UIKit;
using Foundation;

using Words_MVVM.Views;
using Words_MVVM.iOS.Views;

[assembly: Xamarin.Forms.Dependency(typeof(ToastiOS))]
namespace Words_MVVM.iOS.Views
{
    public class ToastiOS : IToast
    {
        private UIAlertController Alert;
        private NSTimer AlertTimer;

        // Time of the Toast-like message.
        private const double ShortToastLength = 0.8;

        // Show a long Toast-message.
        public void Long(string message)
        {
            Show(message, 2 * ShortToastLength);
        }

        // Show a short Toast-message.
        public void Short(string message)
        {
            Show(message, ShortToastLength);
        }

        private void Show(string message, double toastLength)
        {
            // Dismiss the previous object (if it was presented).
            Dismiss();
            Alert = UIAlertController.Create(null, message, UIAlertControllerStyle.Alert);
            AlertTimer = NSTimer.CreateScheduledTimer(toastLength, (obj) => Dismiss());
            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(Alert, true, null);
        }

        // Dismiss the alert after the specified time.
        private void Dismiss()
        {
            Alert?.DismissViewController(true, null);
            AlertTimer?.Dispose();
        }
    }
}

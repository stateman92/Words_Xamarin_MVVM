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

        /// <summary>
        /// Time of the Toast-like message.
        /// </summary>
        private const double ShortToastLength = 0.8;

        /// <summary>
        /// Show a long Toast-message.
        /// </summary>
        /// <param name="message">The string that will be displayed.</param>
        public void Long(string message)
        {
            Show(message, 2 * ShortToastLength);
        }

        /// <summary>
        /// Show a short Toast-message.
        /// </summary>
        /// <param name="message">The string that will be displayed.</param>
        public void Short(string message)
        {
            Show(message, ShortToastLength);
        }

        /// <summary>
        /// Show a Toast-message.
        /// </summary>
        /// <param name="message">The string that will be displayed.</param>
        /// <param name="toastLength">The timeduration after the Toast will de dismissed.</param>
        private void Show(string message, double toastLength)
        {
            // Dismiss the previous object (if it was presented).
            Dismiss();
            Alert = UIAlertController.Create(null, message, UIAlertControllerStyle.Alert);
            AlertTimer = NSTimer.CreateScheduledTimer(toastLength, (obj) => Dismiss());
            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(Alert, true, null);
        }

        /// <summary>
        /// Dismiss the alert after the specified time.
        /// </summary>
        private void Dismiss()
        {
            Alert?.DismissViewController(true, null);
            AlertTimer?.Dispose();
        }
    }
}

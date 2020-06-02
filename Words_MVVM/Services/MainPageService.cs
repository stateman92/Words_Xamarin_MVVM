using System.Threading.Tasks;

using Xamarin.Forms;

using Words_MVVM.Views;

namespace Words_MVVM.Services
{
    public class MainPageService : IMainPageService
    {
        /// <summary>
        /// Push a page.
        /// </summary>
        /// <param name="page">The Page that will be pushed.</param>
        public async Task PushAsync(Page page)
        {
            await Application.Current.MainPage.Navigation.PushAsync(page);
        }

        /// <summary>
        /// Show an alert.
        /// </summary>
        /// <param name="message">The string that will be displayed.</param>
        public void ShowAlert(string message)
        {
            // It is platformspecific, so I ask the DependencyService for an IToast object.
            DependencyService.Get<IToast>().Short(message);
        }

        /// <summary>
        /// Store a value persistently with the given key.
        /// </summary>
        /// <param name="key">The stored object's key.</param>
        /// <param name="value">The value that need to be stored.</param>
        public void CreateOrUpdatePersistentValue(string key, string value)
        {
            Application.Current.Properties[key] = value;
        }

        /// <summary>
        /// Retrieve a value from a persistent storage with the given key.
        /// </summary>
        /// <remarks>
        /// If no value stored with the given key, it returns an empty string.
        /// </remarks>
        /// <param name="key">The searched key.</param>
        public string ReadPersistentValue(string key)
        {
            try
            {
                return Application.Current.Properties[key] as string;
            }
            catch
            {
                return "";
            }
        }
    }
}

using System.Threading.Tasks;

using Xamarin.Forms;

using Words_MVVM.Views;

namespace Words_MVVM.Services
{
    public class MainPageService : IMainPageService
    {
        // Push a page.
        public async Task PushAsync(Page page)
        {
            await Application.Current.MainPage.Navigation.PushAsync(page);
        }

        // Show an alert.
        public void ShowAlert(string message)
        {
            // It is platformspecific, so I ask the DependencyService for an IToast object.
            DependencyService.Get<IToast>().Short(message);
        }

        // Store a value persistently with the given key.
        public void CreateOrUpdatePersistentValue(string key, string value)
        {
            Application.Current.Properties[key] = value;
        }

        // Retrieve a value from a persistent storage with the given key.
        // If no value stored with the given key, it returns an empty string.
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

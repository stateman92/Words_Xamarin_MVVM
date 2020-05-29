using System.Threading.Tasks;

using Xamarin.Forms;

namespace Words_MVVM.Services
{
    public interface IMainPageService
    {
        // Push a Page.
        Task PushAsync(Page page);

        // Show an alert.
        void ShowAlert(string message);

        // Store a value persistently with the given key.
        void CreateOrUpdatePersistentValue(string key, string value);

        // Retrieve a value from a persistent storage with the given key.
        string ReadPersistentValue(string key);
    }
}

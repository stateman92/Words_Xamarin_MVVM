using System.Threading.Tasks;

using Xamarin.Forms;

namespace Words_MVVM.Services
{
    public interface IMainPageService
    {
        /// <summary>
        /// Push a Page.
        /// </summary>
        /// <param name="page">The Page that will be pushed.</param>
        Task PushAsync(Page page);

        /// <summary>
        /// Show an alert.
        /// </summary>
        /// <param name="message">The string that will be displayed.</param>
        void ShowAlert(string message);

        /// <summary>
        /// Store a value persistently with the given key.
        /// </summary>
        /// <param name="key">The stored object's key.</param>
        /// <param name="value">The value that need to be stored.</param>
        void CreateOrUpdatePersistentValue(string key, string value);

        /// <summary>
        /// Retrieve a value from a persistent storage with the given key.
        /// </summary>
        /// <param name="key">The searched key.</param>
        string ReadPersistentValue(string key);
    }
}

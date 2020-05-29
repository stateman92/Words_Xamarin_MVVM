using Xamarin.Forms;

using Words_MVVM.Services;
using Words_MVVM.ViewModels;

namespace Words_MVVM.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            BindingContext = new MainViewModel(new MainPageService(), new ApiCommunicationService());

            NavigationPage.SetHasNavigationBar(this, true);
        }
    }
}

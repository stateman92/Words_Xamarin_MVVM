using System;
using System.Linq;
using System.Globalization;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Xamarin.Forms;

using Words_MVVM.Views;
using Words_MVVM.Models;
using Words_MVVM.Services;
using Words_MVVM.Extensions;

namespace Words_MVVM.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {
        /// <summary>
        /// Constants in order to make less typo.
        /// </summary>
        private const string WordKey = "WordKey";
        private const string FromLanguageKey = "FromLanguageKey";
        private const string ToLanguageKey = "ToLanguageKey";

        private const string English = "English";
        private const string NoElements = "No elements available.";
        private const string ServerNotAvailable = "Server not available. Please try again later.";
        private const string UnknownWord = "Yandex does not know this word.";

        private bool DoneWithRestore = false;

        private readonly IMainPageService MainPageService;
        private readonly IApiCommunicationService ApiCommunicationService;
        private Dictionary<string, string> LongAndShortLanguageNames { get; set; } = new Dictionary<string, string>();
        private List<string> OriginalLanguagePairs { get; set; } = new List<string>();

        private void ChangedLanguages()
        {
            Changed(nameof(FromLanguage));
            Changed(nameof(ToLanguage));
        }

        /// <summary>
        /// A wrapper, that makes a PropertyChanged call more convenient.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        private void Changed(string propertyName)
        {
            OnPropertyChanged(propertyName);
        }

        /// <summary>
        /// The commands.
        /// </summary>
        public ICommand SwitchLanguagesCommand { get; private set; }
        public ICommand CompletedTextCommand { get; private set; }
        public ICommand ItemSelectedCommand { get; private set; }
        public ICommand RefreshCommand { get; private set; }
        public ICommand ExamplesSelectedCommand { get; private set; }
        public ICommand MeaningsSelectedCommand { get; private set; }

        /// <summary>
        /// Constructor, setup the MainPageService and Commands.
        /// After that it downloads the languages.
        /// </summary>
        /// <param name="mainPageService">An object that implements the IMainPageService interface.</param>
        /// <param name="apiCommunicationService">An object that implements the IApiCommunicationService interface.</param>
        public MainViewModel(IMainPageService mainPageService, IApiCommunicationService apiCommunicationService)
        {
            MainPageService = mainPageService;
            ApiCommunicationService = apiCommunicationService;

            SwitchLanguagesCommand = new Command(SwitchLanguages);
            CompletedTextCommand = new Command(CompletedText);
            ItemSelectedCommand = new Command<Translation>(async translation => await ItemSelected(translation));
            RefreshCommand = new Command(Refresh);
            ExamplesSelectedCommand = new Command<Translation>(async translation => await ExamplesSelected(translation));
            MeaningsSelectedCommand = new Command<Translation>(async translation => await MeaningsSelected(translation));

            GetLanguages();
        }

        /// <summary>
        /// Retrieve the previously saved data (searched word and the two languages).
        /// </summary>
        private async void RestorePreviousData()
        {
            Word = Get(WordKey);
            Changed(nameof(Word));
            var storedFromLanguage = Get(FromLanguageKey);
            var storedToLanguage = Get(ToLanguageKey);

            // If it succeeded, it will download the data of the searched word.
            if (storedFromLanguage.IsNotNullNorEmpty() && storedToLanguage.IsNotNullNorEmpty() && word.IsNotNullNorEmpty())
            {
                fromLanguage = storedFromLanguage;
                toLanguage = storedToLanguage;
                ChangedLanguages();
                await GetDefinition();
            }
            // Else it just set the FromLanguage (in order to set up the UI properly,
            // with this the ToLanguage will be set too).
            else
            {
                DoneWithRestore = true;
                FromLanguage = FromLanguage;
            }
            ChangedLanguages();
            DoneWithRestore = true;
        }

        /// <summary>
        /// Create a NavigationPage that wraps up the given Page with BindingContext Translation.
        /// The Title of the NavigationPage is optional.
        /// </summary>
        /// <param name="translation">The Translation that's details will be displayed.</param>
        /// <param name="page">The Page that will be displayed.</param>
        /// <param name="title">Optional parameter, it will be the title of the next Page.</param>
        private NavigationPage CreateNavPage(Translation translation, Page page, string title = "")
        {
            page.BindingContext = translation;
            return new NavigationPage(page)
            {
                BarBackgroundColor = Color.FromHex(MainColor),
                BarTextColor = Color.White,
                Title = title
            };
        }

        /// <summary>
        /// Outsource the common part of the three Push Commands.
        /// </summary>
        /// <param name="translation">The Translation that's details will be displayed.</param>
        /// <param name="successfullyPushed">A function, that will be called if a push can be make with success.</param>
        private async Task Push(Translation translation, Func<Task<Boolean>> successfullyPushed)
        {
            if (translation != null)
            {
                SelectedItem = null;
                Changed(nameof(SelectedItem));
                if (await successfullyPushed())
                {
                    MakeAlert(NoElements);
                }
            }
            return;
        }
    }

    /// <summary>
    /// Wrapper part.
    /// It just makes the functions call through the MainPageService more convenient.
    /// </summary>
    public partial class MainViewModel
    {
        private void MakeAlert(string message)
        {
            MainPageService.ShowAlert(message);
        }

        private string Get(string key)
        {
            return MainPageService.ReadPersistentValue(key);
        }

        private void Store(string key, string value)
        {
            MainPageService.CreateOrUpdatePersistentValue(key, value);
        }

        private async Task Push(NavigationPage navigationPage)
        {
            await MainPageService.PushAsync(navigationPage);
        }
    }

    /// <summary>
    /// The HTTP part, where the requests happen.
    /// </summary>
    public partial class MainViewModel
    {
        /// <summary>
        /// It stores that whether the app currently searching for a word.
        /// </summary>
        private bool IsBusy { get; set; } = false;

        /// <summary>
        /// It downloads (and stores) the available languages from the API.
        /// </summary>
        private async void GetLanguages()
        {
            try
            {
                OriginalLanguagePairs = await ApiCommunicationService.GetLanguages();

                // The first part of every language pair (e.g. "hu").
                var languages = new ObservableCollection<string>();
                OriginalLanguagePairs.ForEach(languagePair =>
                    languages.Add(languagePair.RemoveAfterChar('-'))
                );

                // The filtered list of every language.
                var uniqueLanguages = new HashSet<string>(languages);

                foreach (string language in uniqueLanguages)
                {
                    if (language.Length == 2)
                    {
                        try
                        {
                            // Stores the long name (e.g. "Hungarian") and the short name (e.g. "hu") too.
                            var longNameOfLanguage = new CultureInfo(language).DisplayName;
                            FromList.Add(longNameOfLanguage.RemoveAfterSpace());
                            LongAndShortLanguageNames.Add(longNameOfLanguage.RemoveAfterSpace(), language);
                        }
                        catch { }
                    }
                };

                FromList.Sort();

                fromLanguage = FromList[0];

                // If available, starts with English.
                FromLanguage = English;

                // Try to restore the previously (if it was not the first launch) datas.
                RestorePreviousData();
            }
            catch
            {
                // If something went wrong, make an alert.
                MakeAlert(ServerNotAvailable);
            }
            finally
            {
                // I make it not refreshing in the "finally" block,
                // so it happens all the time, it cannot be "stucked."
                IsRefreshing = false;
                Changed(nameof(IsRefreshing));
            }
        }

        /// <summary>
        /// It downloads (and stores) the definition (meanings, etc.) of the searched word.
        /// </summary>
        private async Task GetDefinition()
        {
            // If it is already working, do not disturb with another request.
            if (!IsBusy && Word.IsNotNullNorEmpty())
            {
                IsBusy = true;

                Translations.Clear();
                Changed(nameof(Translations));
                IsRefreshing = true;
                Changed(nameof(IsRefreshing));

                try
                {
                    RootDefinition = await ApiCommunicationService.GetDefinition(Word, LongAndShortLanguageNames[FromLanguage], LongAndShortLanguageNames[ToLanguage]);

                    // Stores only the Translation parts, the others are unnecessary.
                    foreach (var definition in RootDefinition.Def)
                    {
                        foreach (var translation in definition.Tr)
                        {
                            Translations.Add(translation);
                        }
                    }
                    Changed(nameof(Translations));

                    // If the request was successful, but no definition arrived,
                    // then Yandex does not know this word.
                    if (Translations.Count < 1)
                    {
                        MakeAlert(UnknownWord);
                    }
                }
                catch (Exception e)
                {
                    // If something went wrong, make an alert.
                    MakeAlert(e.Message);
                }
                finally
                {
                    // I make it not refreshing and not busy in the "finally" block,
                    // so it happens all the time, it cannot be "stucked."
                    IsRefreshing = false;
                    Changed(nameof(IsRefreshing));
                    IsBusy = false;
                }
            }
        }
    }

    /// <summary>
    /// Persistently stored part (FromLanguage, ToLanguage, Word).
    /// </summary>
    public partial class MainViewModel
    {
        /// <summary>
        /// The selected language that the word needs to translate from.
        /// </summary>
        private string fromLanguage;
        public string FromLanguage
        {
            get => fromLanguage;
            set
            {
                if (FromList.Contains(value))
                {
                    fromLanguage = value;

                    // If it is actually not empty, store is persistently.
                    if (fromLanguage.IsNotNullNorEmpty())
                    {
                        Store(FromLanguageKey, fromLanguage);
                    }

                    // Rebuild the languages that the word can be translated to.
                    ToList.Clear();
                    foreach (string originalPairs in OriginalLanguagePairs)
                    {
                        // Get the short name of the fromLanguage.
                        var success = LongAndShortLanguageNames.TryGetValue(fromLanguage, out string shortName);

                        // Variable originalPairs stores e.g. "hu-ru"
                        if (success && originalPairs.Substring(0, 2).Equals(shortName))
                        {
                            try
                            {
                                // Get the originalPairs' toLanguage' fromLanguage pair from the originally stored strings.
                                var displayNameOfLanguage = LongAndShortLanguageNames.FirstOrDefault(x => x.Value.Equals(originalPairs.Substring(3, 2))).Key;

                                // If the found language is not equals with the fromLanguage, then add it to the ToList.
                                if (!displayNameOfLanguage.Equals(fromLanguage))
                                {
                                    ToList.Add(displayNameOfLanguage);
                                }
                            }
                            catch { }
                        }
                    }
                    ToList.Sort();
                    if (DoneWithRestore)
                    {
                        toLanguage = ToList[0];
                    }
                    ChangedLanguages();
                }
            }
        }

        /// <summary>
        /// The selected language that the word needs to translate to.
        /// </summary>
        private string toLanguage = "";
        public string ToLanguage
        {
            get => toLanguage;
            set
            {
                if (value != null && ToList.Contains(value))
                {
                    toLanguage = value;

                    // If it is actually not empty, store is persistently.
                    if (toLanguage.IsNotNullNorEmpty())
                    {
                        Store(ToLanguageKey, toLanguage);
                    }
                    ChangedLanguages();
                }
            }
        }

        /// <summary>
        /// The searched word.
        /// </summary>
        private string word = "";
        public string Word
        {
            get => word;
            set
            {
                word = value;

                // If it is actually not empty, store is persistently.
                if (word.IsNotNullNorEmpty())
                {
                    Store(WordKey, word);
                }
            }
        }

        /// <summary>
        /// Collection of languages on the side "translate from."
        /// </summary>
        public ObservableCollection<string> FromList { get; private set; } = new ObservableCollection<string>();

        /// <summary>
        /// Collection of languages on the side "translate to."
        /// </summary>
        public ObservableCollection<string> ToList { get; private set; } = new ObservableCollection<string>();

        /// <summary>
        /// The actual Definition of the searched word.
        /// </summary>
        public RootDefinition RootDefinition { get; set; } = new RootDefinition();

        /// <summary>
        /// Collection of the translation of the searched word.
        /// </summary>
        public ObservableCollection<Translation> Translations { get; private set; } = new ObservableCollection<Translation>();

        /// <summary>
        /// The item that the user selected.
        /// </summary>
        private Translation selectedItem;
        public Translation SelectedItem
        {
            get => selectedItem;
            set
            {
                selectedItem = value;

                // ListView has no Command parameter, so
                // after SelectedItem changed (from ListView with Binding),
                // it fires the ItemSelected Command.
                ItemSelectedCommand.Execute(SelectedItem);
            }
        }
        public bool IsRefreshing { get; set; }
        public string MainColor { get; set; } = "#1976D2";
    }

    /// <summary>
    /// The commands part, all commands' functions stored here. 
    /// </summary>
    public partial class MainViewModel
    {
        /// <summary>
        /// After clicked Meanings, open a new Page.
        /// </summary>
        /// <param name="translation">The Translation that's details will be displayed.</param>
        private async Task MeaningsSelected(Translation translation)
        {
            await Push(translation, async () =>
            {
                var returnCondition = !(translation.Mean != null && translation.Mean.Length > 0);
                if (!returnCondition)
                {
                    await Push(CreateNavPage(translation, new MeaningsPage(), "Meanings"));
                }
                return returnCondition;
            });
        }

        /// <summary>
        /// After clicked on the item or Synonyms, open a new Page.
        /// </summary>
        /// <param name="translation">The Translation that's details will be displayed.</param>
        private async Task ItemSelected(Translation translation)
        {
            await Push(translation, async () =>
            {
                var returnCondition = !(translation.Syn != null && translation.Syn.Length > 0);
                if (!returnCondition)
                {
                    await Push(CreateNavPage(translation, new SynonymsPage(), "Synonyms"));
                }
                return returnCondition;
            });
        }

        /// <summary>
        /// After clicked Examples, open a new Page.
        /// </summary>
        /// <param name="translation">The Translation that's details will be displayed.</param>
        private async Task ExamplesSelected(Translation translation)
        {
            await Push(translation, async () =>
            {
                var returnCondition = !(translation.Ex != null && translation.Ex.Length > 0);
                if (!returnCondition)
                {
                    await Push(CreateNavPage(translation, new ExamplesPage(), "Examples"));
                }
                return returnCondition;
            });
        }

        /// <summary>
        /// Pull-to-refresh functionality.
        /// </summary>
        private async void Refresh()
        {
            await GetDefinition();
        }

        /// <summary>
        /// Change the languages.
        /// After that download the definition too.
        /// </summary>
        private async void SwitchLanguages()
        {
            var tempLanguage = FromLanguage;
            FromLanguage = ToLanguage;
            ToLanguage = tempLanguage;

            await GetDefinition();
        }

        /// <summary>
        /// After clicked Go, it download the definition.
        /// </summary>
        private async void CompletedText()
        {
            if (Word.IsNotNullNorEmpty())
            {
                Word = Word.RemoveAfterSpace();
                Changed(nameof(Word));
                await GetDefinition();
            }
        }
    }
}

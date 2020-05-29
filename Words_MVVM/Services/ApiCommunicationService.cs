using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

using Newtonsoft.Json;

using Words_MVVM.Models;

namespace Words_MVVM.Services
{
    public class ApiCommunicationService : IApiCommunicationService
    {
        // WARNING!
        // BEFORE USE, PLEASE INSERT AN API KEY HERE!
        private const string ApiKey = "<insert API key here>";
        private const string BaseApiUrl = "https://dictionary.yandex.net/api/v1/dicservice.json/";

        // The URL of the languages.
        private string LanguageApiUrl
        {
            get => BaseApiUrl +
                   "getLangs?key=" + ApiKey;
        }

        // The URL of the words.
        private string WordApiUrl
        {
            get => BaseApiUrl +
                   "lookup?key=" + ApiKey +
                   "&lang=" + FromLanguage +
                   "-" + ToLanguage +
                   "&text=" + Word;
        }

        private string FromLanguage { get; set; }
        private string ToLanguage { get; set; }
        private string Word { get; set; }

        // Download the available languages.
        public async Task<List<string>> GetLanguages()
        {
            var content = await new HttpClient().GetStringAsync(LanguageApiUrl);

            // The original downloaded data (e.g. "hu-ru").
            return JsonConvert.DeserializeObject<List<string>>(content);
        }

        // Download the data of a given string Word in the given languages.
        public async Task<RootDefinition> GetDefinition(string Word, string FromLanguage, string ToLanguage)
        {
            this.Word = Word;
            this.FromLanguage = FromLanguage;
            this.ToLanguage = ToLanguage;

            var content = await new HttpClient().GetStringAsync(WordApiUrl);
            return JsonConvert.DeserializeObject<RootDefinition>(content);
        }
    }
}

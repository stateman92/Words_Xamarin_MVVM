using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

using Newtonsoft.Json;

using Words_MVVM.Models;

namespace Words_MVVM.Services
{
    public class ApiCommunicationService : IApiCommunicationService
    {
#error BEFORE USE, PLEASE INSERT AN API KEY HERE!
        private const string ApiKey = "<insert API key here>";
        private const string BaseApiUrl = "https://dictionary.yandex.net/api/v1/dicservice.json/";

        /// <summary>
        /// The URL of the languages.
        /// </summary>
        private string LanguageApiUrl
        {
            get => BaseApiUrl +
                   "getLangs?key=" + ApiKey;
        }

        /// <summary>
        /// The URL of the words.
        /// </summary>
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

        /// <summary>
        /// Download the available languages.
        /// </summary>
        public async Task<List<string>> GetLanguages()
        {
            var content = await new HttpClient().GetStringAsync(LanguageApiUrl);

            // The original downloaded data (e.g. "hu-ru").
            return JsonConvert.DeserializeObject<List<string>>(content);
        }

        /// <summary>
        /// Download the data of a given string Word in the given languages.
        /// </summary>
        /// <param name="Word">The word that wanted to be translated.</param>
        /// <param name="FromLanguage">The language from the user wants to translate the word.</param>
        /// <param name="ToLanguage">The language to the user wants to translate the word.</param>
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

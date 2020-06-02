using System.Threading.Tasks;
using System.Collections.Generic;

using Words_MVVM.Models;

namespace Words_MVVM.Services
{
    public interface IApiCommunicationService
    {
        /// <summary>
        /// Download the available languages.
        /// </summary>
        Task<List<string>> GetLanguages();

        /// <summary>
        /// Download the data of a given string Word in the given languages.
        /// </summary>
        /// <param name="Word">The word that wanted to be translated.</param>
        /// <param name="FromLanguage">The language from the user wants to translate the word.</param>
        /// <param name="ToLanguage">The language to the user wants to translate the word.</param>
        Task<RootDefinition> GetDefinition(string Word, string FromLanguage, string ToLanguage);
    }
}

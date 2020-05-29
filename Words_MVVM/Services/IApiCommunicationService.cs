using System.Threading.Tasks;
using System.Collections.Generic;

using Words_MVVM.Models;

namespace Words_MVVM.Services
{
    public interface IApiCommunicationService
    {
        // Download the available languages.
        Task<List<string>> GetLanguages();

        // Download the data of a given string Word in the given languages.
        Task<RootDefinition> GetDefinition(string Word, string FromLanguage, string ToLanguage);
    }
}

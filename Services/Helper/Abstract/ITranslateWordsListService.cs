using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Helper.Abstract
{
    public interface ITranslateWordsListService
    {
          Task<List<string>> translateWords(List<string> concatenatedWords, string fromLanguage, string toLanguage);
    }
}
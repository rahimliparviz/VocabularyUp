using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Helper.Abstract
{
    public interface ITranslateWordsListService
    {
          Task<List<string>> translateWords(string concatenatedWords);
    }
}
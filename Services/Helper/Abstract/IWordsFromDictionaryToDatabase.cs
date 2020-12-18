using Microsoft.AspNetCore.Http;

namespace Services.Helper.Abstract
{
    public interface IWordsFromDictionaryToDatabase
    {
        void AddWordsFile(IFormFile file);
    }
}
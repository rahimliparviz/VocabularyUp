using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Services.Helper.Abstract;

namespace Services.Helper.Concrete
{
    public class WordsFromDictionaryToDatabase: IWordsFromDictionaryToDatabase
    {

        public async void AddWordsFile(IFormFile file)
        {

            String result;
            using var reader = new StreamReader(file.OpenReadStream());
            result = await reader.ReadToEndAsync();
            Console.WriteLine(result);
        }
    }
}
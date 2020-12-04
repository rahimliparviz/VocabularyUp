using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Services.Helper.Abstract;

namespace Services.Helper.Concrete
{
    public class TranslateWordsListService :ITranslateWordsListService
    {
        public async Task<List<string>> translateWords(string concatenatedWords)
        {

            // var concatenatedWords = "hello,horse,snake";

            string url = String.Format
            ("https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}",
                "en", "az", Uri.EscapeUriString(concatenatedWords));
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            string result = await httpClient.GetStringAsync(url);
            var df = result.Split('"')[1].Replace(",","").Split(" ").ToList();
            return df;
        }
    }
}
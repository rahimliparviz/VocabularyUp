using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Services.Extentions;
using Services.Helper.Abstract;

namespace Services.Helper.Concrete
{
    public class TranslateWordsListService :ITranslateWordsListService
    {
        public async Task<List<string>> translateWords(List<string> newWords,string fromLanguage,string toLanguage)
        {

            int takenWordsCount = 20;

            var loopCount = Math.Ceiling((double)newWords.Count / takenWordsCount);
            var translatedWords = new List<string>();



            // foreach (var item in phrases
            //     .Select((o, i) => new {Value = o, Index = i})
            //     .Where(p => p.Index % 2 != 0)
            //     .OrderByDescending(p => p.Index))
            // {
            //     if (item.Index + 1 == phrases.Count) phrases.Add("22");
            //     else phrases.Insert(item.Index + 1, "22");
            // } 
            //
            //
            
            

            for (int i = 0; i < loopCount; i++)
            {
                var skip = takenWordsCount * i;
                // var concatenatedWords = String.Join(",", newWords.Skip(skip).Take(takenWordsCount));
                var concatenatedWords = String.Join(",", newWords.Skip(skip).Take(takenWordsCount));



                string url = String.Format
                ("https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}",
                    fromLanguage, toLanguage, Uri.EscapeUriString(concatenatedWords));
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type",
                    "application/json; charset=utf-8");
                string result = await httpClient.GetStringAsync(url);
                // var df = result.Split('"')[1].Replace(",", "").Split(" ").ToList();
                var df = result.Split('"')[1].Split(",").ToList();
                // return df;
                translatedWords.AddRange(df);
            }

            return translatedWords;
            // var concatenatedWords = String.Join(",", newWords);
            //
            //
            //
            // string url = String.Format
            // ("https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}",
            //     fromLanguage, toLanguage, Uri.EscapeUriString(concatenatedWords));
            // HttpClient httpClient = new HttpClient();
            // httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            // string result = await httpClient.GetStringAsync(url);
            // var df = result.Split('"')[1].Replace(",","").Split(" ").ToList();
            // return df;
        }
    }
}
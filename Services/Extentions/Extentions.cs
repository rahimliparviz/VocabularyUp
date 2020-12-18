using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Internal;

namespace Services.Extentions
{
    public static class ReadFileAsStringAsync
    {
        public static async Task<List<string>> ReadAsStringAsync(this IFormFile file)
        {
            


            List<string> exceptionalWords = new List<string>()
            {
                "is", "are", "am", "to", "for", "from"," ","",
                "of","at","oh","the","a","an","was","were","dont",
                "be","ima","im","and"
            };
            
            
        
            
            // Build a list of auxiliary words.
            var result = new StringBuilder();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
         
                
                while (reader.Peek() >= 0)
                {
                    var word = await reader.ReadLineAsync();
                    
                    
                    
                    string phrase = Regex.Replace(word, @"[ ](?=[ ])|[^A-Za-z ]+", "");

                    if (!String.IsNullOrEmpty(phrase))
                    {
                        result.AppendLine(phrase);
                    }
                }
            }

            var text = result.ToString().Trim();
            List<string> phrases = text.Replace("\r\n", " ").ToLower().Split(' ').Distinct().Except(exceptionalWords).ToList();
            

            return phrases;
        }

        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> self)
            => self.Select((item, index) => (item, index));


        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int chunksize)
        {
            while (source.Any())
            {
                yield return source.Take(chunksize);
                source = source.Skip(chunksize);
            }
        }

        public static IEnumerable<IEnumerable<T>> ChunkTrivialBetter<T>(this IEnumerable<T> source, int chunksize)
        {
            var pos = 0;
            while (source.Skip(pos).Any())
            {
                yield return source.Skip(pos).Take(chunksize);
                pos += chunksize;
            }
        }

    }


}
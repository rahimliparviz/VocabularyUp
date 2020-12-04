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

              List<string> phrases = new List<string>();
        
            
            // Build a list of auxiliary words.
            var result = new StringBuilder();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                {
                    var word = await reader.ReadLineAsync();
                    string phrase = Regex.Replace(word, @"[ ](?=[ ])|[^A-Za-z0-9 ]+", "");

                    if (!String.IsNullOrEmpty(phrase))result.AppendLine(phrase);
                }
            }

            var text = result.ToString().Trim();
            phrases = text.Replace("\r\n", " ").ToLower().Split(' ').Distinct().ToList();

            return phrases;
        }
    }
    
}
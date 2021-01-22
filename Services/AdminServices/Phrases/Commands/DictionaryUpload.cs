using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Data;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;


namespace Services.Phrases.Commands
{
    public class DictionaryUpload
    {
        public class Command : IRequest
        {
            public IFormFile File { get; set; }
            public Guid FromLanguageId { get; set; }
            public Guid ToLanguageId { get; set; }
        }


        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;


            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                

                String allText;
                using var reader = new StreamReader(request.File.OpenReadStream());
                allText = await reader.ReadToEndAsync();
                

                var wordsPair = allText
                    .Replace("\r\n", "")
                    .Replace("\t", "")
                    .Split('#');
                foreach (var wordPair in wordsPair)
                {

                    if (wordPair.Length > 0)
                    {


                        ;
                        var wt = wordPair.Split('*').ToArray();
                        var word = wt[0];
                        var translatedWord = wt[1];





                        var newPhrase = new Phrase
                        {
                            Word = word,
                            LanguageId = request.ToLanguageId,
                            CreatedAt = DateTime.Now,
                            Translations = new List<Translation>()
                        };
                        var translation = new Translation
                        {
                            LanguageId = request.FromLanguageId,
                            Word = translatedWord,
                            CreatedAt = DateTime.Now
                        };
                        newPhrase.Translations.Add(translation);
                        await _context.Phrases.AddAsync(newPhrase, cancellationToken);
                    }

                }

                await _context.SaveChangesAsync(cancellationToken);








                throw new Exception("Problem saving changes");
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Data;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Services.DTO;
using Services.Extentions;

namespace Services.UserServices.Actions.Queries
{
    public class PhrasesFromFile
    {
        public class Command : IRequest<List<PhrasesWithTranslationDto>>
        {
            public IFormFile File { get; set; }
            public Guid FromLanguageId { get; set; }
            public Guid ToLanguageId { get; set; }
        }
   

    public class Handler : IRequestHandler<Command, List<PhrasesWithTranslationDto>>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }
        
        public async Task<List<PhrasesWithTranslationDto>> Handle(Command request, CancellationToken cancellationToken)
        {

            var phrases =await request.File.ReadAsStringAsync();


            //ToDo get current user
            var userId = "ef84dac8-84a0-42a6-ba94-8840d9078af3";


            var userPhrases = _context.UserPhrases
                .Include(x=>x.Phrase)
                .Where(c=>c.Phrase.LanguageId == request.FromLanguageId && c.UserId == userId)
                .Select(d=>d.Phrase)
                .ToList();

            //Databasada olan amma userin bilmediyi sozler
            var existedPhrases = _context.Phrases
                .Include(p => p.Translations)
                .Where(i => i.LanguageId == request.FromLanguageId && phrases.Contains(i.Word))
                .ToList();

            

            var wordsToLearn =
                existedPhrases.Except(userPhrases)
                    .Select(a => new PhrasesWithTranslationDto
                    {
                        Phrase = a.Word,
                        PhraseId = a.Id,
                        Translation = a.Translations.First(l => l.LanguageId == request.ToLanguageId).Word
                    }).ToList();
            return wordsToLearn;
    
        }
    }
    }
}
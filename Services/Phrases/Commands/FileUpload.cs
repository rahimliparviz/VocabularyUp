using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Data;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Services.DTO;
using Services.Extentions;
using Services.Helper.Abstract;

namespace Services.Phrases.Commands
{
    public class FileUpload
    {
        public class Command : IRequest<List<PhrasesWithTranslationsDto>>
        {
            public IFormFile File { get; set; }
            public Guid FromLanguageId { get; set; }
            public Guid ToLanguageId { get; set; }
        }
   

    public class Handler : IRequestHandler<Command, List<PhrasesWithTranslationsDto>>
    {
        private readonly DataContext _context;
        private readonly ITranslateWordsListService _translatedWordsListService;
        private readonly IMapper _mapper;

        public Handler(DataContext context,ITranslateWordsListService translatedWordsListService,IMapper mapper)
        {
            _context = context;
            _translatedWordsListService = translatedWordsListService;
            _mapper = mapper;
        }
        
        public async Task<List<PhrasesWithTranslationsDto>> Handle(Command request, CancellationToken cancellationToken)
        {

            var phrases =await request.File.ReadAsStringAsync();


            //ToDo get current user
            var userId = "ef84dac8-84a0-42a6-ba94-8840d9078af3";
            // var fromLanguageName = (await _context.Languages.FindAsync(request.FromLanguageId)).Name;
            // var toLanguageName = (await _context.Languages.FindAsync(request.ToLanguageId)).Name;
            var fromAndToLanguages =await _context.Languages.Where(l =>
                l.Id == request.FromLanguageId || l.Id == request.ToLanguageId).ToListAsync(cancellationToken: cancellationToken);
             var fromLanguageName = fromAndToLanguages.First(l=>l.Id == request.FromLanguageId).Name;
             var toLanguageName = fromAndToLanguages.First(l => l.Id == request.ToLanguageId).Name;
            
            var userPhrases = _context.UserPhrases
                .Include(x=>x.Phrase)
                .Where(c=>c.Phrase.LanguageId == request.FromLanguageId && c.UserId == userId)
                .Select(d=>d.Phrase)
                .ToList();

            var existedPhrases = _context.Phrases.Where(i => i.LanguageId == request.FromLanguageId && phrases.Contains(i.Word)).ToList();
            
            var newWords = 
                phrases.Except(existedPhrases.Select(p => p.Word).ToList())
                .OrderBy(w => w)
                .ToList();



           

            
            var wordsToAdd = await _translatedWordsListService.translateWords(newWords,fromLanguageName,toLanguageName);
            
            List<Phrase> phrasesToAdd = new List<Phrase>();

            foreach (var (word,index) in wordsToAdd.WithIndex())
            {
                var translatedWord = newWords[index];

                if (translatedWord != word)
                {
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
                    phrasesToAdd.Add(newPhrase);
                }
            
            }

            // await _context.Phrases.AddRangeAsync(phrasesToAdd, cancellationToken);
            // await _context.SaveChangesAsync(cancellationToken);


            var wordsToLearn =  _mapper.Map<List<Phrase>, List<PhrasesWithTranslationsDto>>(
                existedPhrases.Except(userPhrases).Concat(phrasesToAdd).ToList());
            return wordsToLearn;
    
            throw new Exception("Problem saving changes");
        }
    }
    }
}
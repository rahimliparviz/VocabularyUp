using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Data;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Services.Extentions;
using Services.Helper.Abstract;

namespace Services.Phrases.Commands
{
    public class FileUpload
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
        private readonly ITranslateWordsListService _translatedWordsListService;

        public Handler(DataContext context,ITranslateWordsListService translatedWordsListService)
        {
            _context = context;
            _translatedWordsListService = translatedWordsListService;
        }
        
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {

            var phrases =await request.File.ReadAsStringAsync();


            //ToDo get current user
            var userId = "ef84dac8-84a0-42a6-ba94-8840d9078af3";
            // var user = await _context.Users.FirstOrDefaultAsync(u=>u.Id == userId);

            // var userPhrases = _context.UserPhrases.Where(u=>u.UserId == userId)
            //     .Select(p=>p.Phrase).ToList();

            var userPhrases = _context.UserPhrases
                .Include(x=>x.Phrase)
                .Where(c=>c.Phrase.LanguageId == request.FromLanguageId && c.UserId == userId)
                .Select(d=>d.Phrase)
                .ToList();
            // var userPhrasess =user.UserPhrases.Select(up=>up.Phrase).ToList();
            //
            // var existedWords = _context.Phrases.Where(item => phrases.Contains(item.Word)).ToList();
            // var newWords = _context.Phrases.Where(item => !phrases.Contains(item.Word)).ToList();

            
            var existedPhrases = _context.Phrases.Where(i => i.LanguageId == request.FromLanguageId && phrases.Contains(i.Word)).ToList();
            // var list3 = phrases.Except<st>(existedWords);

            var wordsToLearn = existedPhrases.Except(userPhrases).ToList();
            
            var newWords = phrases.Except(existedPhrases.Select(p => p.Word).ToList()).ToList();

            
            var concatenatedWords = String.Join(',', newWords);

            // var input = "hello,horse,snake,";
            

            
            var wordsToAdd = await _translatedWordsListService.translateWords(concatenatedWords);
            
            List<Phrase> phrasesToAdd = new List<Phrase>();

            foreach (var word in wordsToAdd)
            {
                phrasesToAdd.Add(new Phrase(){Word = word,LanguageId = request.ToLanguageId,CreatedAt = DateTime.Now});
            }

            await _context.Phrases.AddRangeAsync(phrasesToAdd, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            
            
            return Unit.Value;
    
            throw new Exception("Problem saving changes");
        }
    }
    }
}
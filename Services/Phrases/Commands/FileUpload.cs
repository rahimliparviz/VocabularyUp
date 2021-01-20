using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Data;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Services.DTO;
using Services.Extentions;

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
        private readonly IMapper _mapper;

        public Handler(DataContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<List<PhrasesWithTranslationsDto>> Handle(Command request, CancellationToken cancellationToken)
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


            

            var wordsToLearn =  _mapper.Map<List<Phrase>, List<PhrasesWithTranslationsDto>>(
                existedPhrases.Except(userPhrases)
                    .Where(l=>l.Translations.Any(g=>g.LanguageId == request.ToLanguageId))
                    .ToList());
            return wordsToLearn;
    
        }
    }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.DTO;

namespace Services.UserServices.Actions.Queries
{
    public class GetUserPhrases
    {
        public class Query : IRequest<List<PhrasesWithTranslationDto>>
        {
            public Guid FromLanguageId { get; set; }
            public Guid ToLanguageId { get; set; }
        }



        public class Handler : IRequestHandler<Query, List<PhrasesWithTranslationDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<PhrasesWithTranslationDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                //TODO- get current user id
                var userId = "ef84dac8-84a0-42a6-ba94-8840d9078af3";
                //TODO- "en" ve "az" i default congih file dan getir

                var phrasesCartDtos=await  _context.UserPhrases
                    .Include(a => a.Phrase.Translations)
                    .Where(p=>p.NumberOfRemainingRepetitions == 0)
                    .Where(u => u.UserId == userId)
                    .Where(l => l.FromLanguageId == request.FromLanguageId && l.ToLanguageId == request.ToLanguageId)
                    .Select(a => new PhrasesWithTranslationDto
                    {
                        Phrase = a.Phrase.Word,
                        PhraseId = a.PhaseId,
                        Translation = a.Phrase.Translations.First(l => l.LanguageId == request.ToLanguageId).Word
                    })
                    .Take(50)
                    .ToListAsync();

                
                return phrasesCartDtos;
            }
        }



    }
}
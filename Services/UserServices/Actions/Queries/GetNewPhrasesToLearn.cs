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
    public class GetNewPhrasesToLearn
    {
        public class Query : IRequest<List<PhrasesWithTranslationDto>>
        {
            public Guid FromLanguageId { get; set; }
            public Guid ToLanguageId { get; set; }
            public int  PhrasesCount { get; set; }
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
                
       

                var phrasesCartDtos =  from P in _context.Phrases
                    where P.UserPhrases.All(a => a.UserId != userId)
                    where P.LanguageId == request.FromLanguageId
                    select  new PhrasesWithTranslationDto
                    {
                        Phrase = P.Word,
                        PhraseId = P.Id,
                        Translation = P.Translations.First(l => l.LanguageId == request.ToLanguageId).Word
                    }
                    ;

                
                return await phrasesCartDtos.OrderBy(x => x.PhraseId).Take(request.PhrasesCount).ToListAsync(cancellationToken: cancellationToken);
            }
        }



    }
}
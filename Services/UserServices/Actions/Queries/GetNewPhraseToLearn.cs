using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Data;
using Domain;
using Infrastructure.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.DTO;

namespace Services.Translations.Queries
{
    public class GetNewPhraseToLearn
    {
        public class Query : IRequest<List<PhraseCartDto>>
        {
            public Guid FromLanguageId { get; set; }
            public Guid ToLanguageId { get; set; }
        }



        public class Handler : IRequestHandler<Query, List<PhraseCartDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<PhraseCartDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                //TODO- get current user id
                var userId = "ef84dac8-84a0-42a6-ba94-8840d9078af3";
                
       

                var phrasesCartDtos = from P in _context.Phrases
                    where P.UserPhrases.All(a => a.PhaseId != P.Id)
                    where P.LanguageId == request.FromLanguageId
                    select  new PhraseCartDto
                    {
                        Phrase = P.Word,
                        PhraseId = P.Id,
                        Translation = P.Translations.First(l => l.LanguageId == request.ToLanguageId).Word
                    };

                
                return phrasesCartDtos.ToList();
            }
        }



    }
}
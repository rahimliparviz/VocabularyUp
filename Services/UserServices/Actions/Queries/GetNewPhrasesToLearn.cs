using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Data;
using Infrastructure.Errors;
using Infrastructure.Interfaces;
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
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                _context = context;
                _userAccessor = userAccessor;
            }

            public async Task<List<PhrasesWithTranslationDto>> Handle(Query request, CancellationToken cancellationToken)
            {
              
                var userId = _userAccessor.GetCurrentUserId() ??
                               throw new RestException(HttpStatusCode.Unauthorized);
                
       

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
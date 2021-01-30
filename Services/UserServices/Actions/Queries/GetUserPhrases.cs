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
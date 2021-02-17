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
            public string Filter { get; set; }
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

              
                var queryable =  _context.UserPhrases
                    .Include(a => a.Phrase.Translations)
                    .Where(u => u.UserId == userId)
                    .Where(l => l.FromLanguageId == request.FromLanguageId && l.ToLanguageId == request.ToLanguageId)
                    .AsQueryable();

                if (request.Filter == "forgotten")
                {
                    queryable = queryable.Where(x =>x.NumberOfRemainingRepetitions > 0);
                }

                if (request.Filter == "known")
                {
                    queryable = queryable.Where(x => x.NumberOfRemainingRepetitions == 0);
                }




               var phrases = await queryable.Select(a => new PhrasesWithTranslationDto
                    {
                        Phrase = a.Phrase.Word,
                        PhraseId = a.PhaseId,
                        Translation = a.Phrase.Translations.First(l => l.LanguageId == request.ToLanguageId).Word,
                        NumberOfRemainingRepetitions=a.NumberOfRemainingRepetitions
                    })
                    .Take(50)
                    .ToListAsync(cancellationToken: cancellationToken);

                
                return phrases;
            }
        }



    }
}
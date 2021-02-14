using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Data;
using Infrastructure.Errors;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.DTO;

namespace Services.UserServices.Actions.Queries
{
    public class UserProfile
    {
        public class Query : IRequest<UserProfileDto>
        {
            public Guid FromLanguageId { get; set; }
            public Guid ToLanguageId { get; set; }
        }



        public class Handler : IRequestHandler<Query, UserProfileDto>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                _context = context;
                _userAccessor = userAccessor;
            }

            public async Task<UserProfileDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var userId = _userAccessor.GetCurrentUserId() ??
                             throw new RestException(HttpStatusCode.Unauthorized);

                var userPhrases=await  _context.UserPhrases
                    .Where(u => u.UserId == userId)
                    .Where(l => l.FromLanguageId == request.FromLanguageId && l.ToLanguageId == request.ToLanguageId)
                    .ToListAsync(cancellationToken: cancellationToken);

                var forgottenPhrasesCount = userPhrases.Count(p => p.NumberOfRemainingRepetitions != 0);
                var knownPhrasesCount = userPhrases.Count(p => p.NumberOfRemainingRepetitions == 0);



                var userProfileInfo = new UserProfileDto
                {
                    ForgottenPhrasesCount = forgottenPhrasesCount, 
                    KnownPhrasesCount = knownPhrasesCount
                };

                return userProfileInfo;
            }
        }



    }
}
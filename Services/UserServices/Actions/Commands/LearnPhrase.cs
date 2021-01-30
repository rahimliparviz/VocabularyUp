using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Data;
using FluentValidation;
using Infrastructure.Errors;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Services.UserServices.Actions.Commands
{
    public class LearnPhrase
    {
        public class Command : IRequest
        {
            public Guid PhraseId { get; set; }
            public Guid FromLanguageId { get; set; }
            public Guid ToLanguageId { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.PhraseId).NotEmpty();
                RuleFor(x => x.FromLanguageId).NotEmpty();
                RuleFor(x => x.ToLanguageId).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context,IUserAccessor userAccessor)
            {
                _context = context;
                _userAccessor = userAccessor;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var userId = _userAccessor.GetCurrentUserId() ??
                             throw new RestException(HttpStatusCode.Unauthorized);


                var isUserPhraseExist = await _context.UserPhrases
                    .Where(up =>
                        up.PhaseId == request.PhraseId &&
                        up.UserId == userId &&
                        up.FromLanguageId == request.FromLanguageId &&
                        up.ToLanguageId == request.ToLanguageId
                    )
                    .FirstOrDefaultAsync(cancellationToken: cancellationToken);

                if (isUserPhraseExist != null)
                {
                    //TODO - asagidaki "3" magic numberdi onu constanta cevir
                    isUserPhraseExist.NumberOfRemainingRepetitions =
                        isUserPhraseExist.NumberOfRemainingRepetitions == 0
                            ? 3
                            : isUserPhraseExist.NumberOfRemainingRepetitions - 1;
                    isUserPhraseExist.LastSeen = DateTime.Now;
                }


                var success = await _context.SaveChangesAsync(cancellationToken) > 0;

                if (success) return Unit.Value;

                throw new Exception("Problem saving changes");
            }
        }
    }
}
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Data;
using Domain;
using FluentValidation;
using Infrastructure.Errors;
using Infrastructure.Interfaces;
using MediatR;

namespace Services.Phrases.Commands
{
    public class AlreadyKnownPhrase
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

                var userPhrase = new UserPhrase
                {
                    UserId = userId,
                    FromLanguageId = request.FromLanguageId,
                    ToLanguageId = request.ToLanguageId,
                    PhaseId = request.PhraseId,
                    LastSeen = DateTime.Now,
                    NumberOfRemainingRepetitions = 0
                };
                
                await _context.UserPhrases.AddAsync(userPhrase, cancellationToken);


                var success = await _context.SaveChangesAsync(cancellationToken) > 0;

                if (success) return Unit.Value;

                throw new Exception("Problem saving changes");
            }
        }
    }
}
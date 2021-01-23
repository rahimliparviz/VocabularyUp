using System;
using System.Threading;
using System.Threading.Tasks;
using Data;
using Domain;
using FluentValidation;
using MediatR;

namespace Services.Phrases.Commands
{
    public class AlreadyKnownPhrase
    {
        public class Command : IRequest
        {
            public Guid PhraseId { get; }
            public Guid FromLanguageId { get; }
            public Guid ToLanguageId { get; }
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

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                //TODO- get current user id
                var userId = "ef84dac8-84a0-42a6-ba94-8840d9078af3";


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
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Data;
using FluentValidation;
using Infrastructure.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Services.Phrases.Commands
{
    public class ForgetTranslation
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


                var isUserPhraseExist = await _context.UserPhrases
                    .Where(up =>
                        up.PhaseId == request.PhraseId &&
                        up.UserId == userId &&
                        up.FromLanguageId == request.FromLanguageId &&
                        up.ToLanguageId == request.ToLanguageId
                    )
                    .FirstOrDefaultAsync(cancellationToken: cancellationToken);


                if (isUserPhraseExist == null)
                    throw new RestException(HttpStatusCode.BadRequest, new {UserPhrases = "Not exists"});


                //TODO - asagidaki "3" magic numberdi onu constanta cevir
                isUserPhraseExist.NumberOfRemainingRepetitions = 3;
                isUserPhraseExist.LastSeen = DateTime.Now;


                var success = await _context.SaveChangesAsync(cancellationToken) > 0;

                if (success) return Unit.Value;

                throw new Exception("Problem saving changes");
            }
        }
    }
}
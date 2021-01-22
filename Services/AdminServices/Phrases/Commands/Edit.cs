using System;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Data;
using Domain;
using FluentValidation;
using Infrastructure.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Services.Phrases.Commands
{
    public class Edit
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
            public string Word { get; set; }
            public Guid LanguageId { get; set; }

        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Word).NotEmpty();
                RuleFor(x => x.LanguageId).NotEmpty();
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
                var phrase = await _context.Phrases.FindAsync(request.Id);

                _ = phrase ?? throw new RestException(HttpStatusCode.BadRequest, new {Phrase = "Not found"});

                var isPhraseAlreadyExist = await _context.Phrases.AnyAsync(
                    x =>
                        x.Word == request.Word &&
                        x.LanguageId == request.LanguageId);

                if (isPhraseAlreadyExist)
                    throw new RestException(HttpStatusCode.BadRequest, new {Phrase = "Already exists"});


                phrase.Word = request.Word ;
                phrase.LanguageId = request.LanguageId;

                var success = await _context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;

                throw new Exception("Problem saving changes");
            }
        }
    }
}
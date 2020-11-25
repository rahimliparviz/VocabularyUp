using System;
using System.Net;
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
    public class Create
    {
        public class Command:IRequest
        {
            public string Word { get; set; }
            public Guid LanguageId { get; set; }
        }
        
        public class CommandValidator:AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Word).NotEmpty();
                RuleFor(x => x.LanguageId).NotEmpty();
            }
        }
        
        public  class Handler:IRequestHandler<Command>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var isPhraseAlreadyExist = await _context.Phrases.AnyAsync(
                    x => 
                    x.Word == request.Word &&
                    x.LanguageId == request.LanguageId);

                if (isPhraseAlreadyExist) throw new RestException(HttpStatusCode.Conflict, new {Phrase = "Already exists"});


                var phrase = new Phrase
                {
                    Word = request.Word,
                    LanguageId = request.LanguageId,
                    CreatedAt = DateTime.Now
                };

                await _context.Phrases.AddAsync(phrase);

                var success = await _context.SaveChangesAsync() > 0;

                if (success) return Unit.Value;

                throw new Exception("Problem saving changes");
            }
        }
    }
}
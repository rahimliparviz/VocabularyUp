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

namespace Services.Translations.Commands
{
    public class Create
    {
        public class Command:IRequest
        {
            public string Word { get; set; }
            public Guid PhraseId { get; set; }
            public Guid LanguageId { get; set; }
        }
        
        public class CommandValidator:AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Word).NotEmpty();
                RuleFor(x => x.PhraseId).NotEmpty();
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
                var isTranslationAlreadyExist = await _context.Translations.AnyAsync(
                    x => 
                    x.PhraseId == request.PhraseId &&
                    x.LanguageId == request.LanguageId
                    );

                if (isTranslationAlreadyExist) throw new RestException(HttpStatusCode.Conflict, new {Translation = "Already exists"});


                var translation = new Translation
                {
                    Word = request.Word,
                    PhraseId = request.PhraseId,
                    LanguageId = request.LanguageId,
                    CreatedAt = DateTime.Now
                };

                await _context.Translations.AddAsync(translation);

                var success = await _context.SaveChangesAsync() > 0;

                if (success) return Unit.Value;

                throw new Exception("Problem saving changes");
            }
        }
    }
}
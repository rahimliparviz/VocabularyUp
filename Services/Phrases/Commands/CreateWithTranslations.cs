using System;
using System.Collections.Generic;
using MediatR;
using Services.DTO;

namespace Services.Phrases.Commands
{
    public class CreateWithTranslations
    {
        public class Command : IRequest
        {
            public Guid PhraseLanguageId { get; set; }
            public string Phrase { get; set; }
            public IEnumerable<TranslationPostDto> Translations { get; set; }
            // public PhraseTranslationPostDto PhraseTranslationPostDto { get; set; }
        }

        // public class CommandValidator : AbstractValidator<Command>
        // {
        //     public CommandValidator()
        //     {
        //         RuleFor(x => x.Word).NotEmpty();
        //         RuleFor(x => x.LanguageId).NotEmpty();
        //     }
        // }

        // public class Handler : IRequestHandler<Command>
        // {
        //     private readonly DataContext _context;
        //
        //     public Handler(DataContext context)
        //     {
        //         _context = context;
        //     }
        //
        //     public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        //     {
        //         var isPhraseAlreadyExist = await _context.Phrases.AnyAsync(
        //             x =>
        //                 x.Word == request.Word &&
        //                 x.LanguageId == request.LanguageId);
        //
        //         if (isPhraseAlreadyExist)
        //             throw new RestException(HttpStatusCode.Conflict, new {Phrase = "Already exists"});
        //
        //
        //         var phrase = new Phrase
        //         {
        //             Word = request.Word,
        //             LanguageId = request.LanguageId,
        //             CreatedAt = DateTime.Now
        //         };
        //
        //         await _context.Phrases.AddAsync(phrase);
        //
        //         var success = await _context.SaveChangesAsync() > 0;
        //
        //         if (success) return Unit.Value;
        //
        //         throw new Exception("Problem saving changes");
        //     }
        // }
    }
}
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Data;
using Domain;
using Infrastructure.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.DTO;

namespace Services.Phrases.Commands
{
    public class CreateWithTranslations
    {
        public class Command : IRequest
        {
            public Guid PhraseLanguageId { get; set; }
            public string Phrase { get; set; }
            public List<TranslationPostDto> Translations { get; set; }
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

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context,IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
        
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var isPhraseAlreadyExist = await _context.Phrases.AnyAsync(
                    x =>
                        x.Word == request.Phrase &&
                        x.LanguageId == request.PhraseLanguageId);
        
                if (isPhraseAlreadyExist)
                    throw new RestException(HttpStatusCode.Conflict, new {Phrase = "Already exists"});


                var translations = _mapper.Map<List<TranslationPostDto>, List<Translation>>(request.Translations);
                var phrase = new Phrase
                {
                    Word = request.Phrase,
                    LanguageId = request.PhraseLanguageId,
                    Translations = translations,
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
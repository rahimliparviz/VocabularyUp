using System;
using System.Linq;
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
    public class LearnNewPhrase
    {
        public class Command:IRequest
        {
            public Guid PhraseId { get;}
            public Guid FromLanguageId { get;}
            public Guid ToLanguageId { get; }
            public Guid UserId { get;}
            public bool IsLearingPhrase { get;}
            
        }
        
        public class CommandValidator:AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.PhraseId).NotEmpty();
                RuleFor(x => x.FromLanguageId).NotEmpty();
                RuleFor(x => x.ToLanguageId).NotEmpty();
                RuleFor(x => x.UserId).NotEmpty();
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

                //TODO- get current user id
                var userId = "ef84dac8-84a0-42a6-ba94-8840d9078af3";

                if (request.IsLearingPhrase)
                {
                    var isUserPhraseExist = await _context.UserPhrases
                        .Where(up => 
                            up.PhaseId == request.PhraseId &&
                            up.UserId == request.UserId.ToString() &&
                            up.FromLanguageId == request.FromLanguageId &&
                            up.ToLanguageId == request.ToLanguageId 
                            )
                        .FirstOrDefaultAsync(cancellationToken: cancellationToken);

                    if(isUserPhraseExist != null)
                    {
                        //TODO - asagidaki "3" magic numberdi onu constanta cevir
                        isUserPhraseExist.NumberOfRemainingRepetitions =
                            isUserPhraseExist.NumberOfRemainingRepetitions == 0 ? 3 : isUserPhraseExist.NumberOfRemainingRepetitions - 1;
                        isUserPhraseExist.LastSeen = DateTime.Now;
                    }
                    else
                    {
                        var userPhrase = new UserPhrase
                        {
                            UserId = userId,
                            FromLanguageId = request.FromLanguageId,
                            ToLanguageId = request.ToLanguageId,
                            PhaseId = request.PhraseId,
                            LastSeen = DateTime.Now,
                        };

                        await _context.UserPhrases.AddAsync(userPhrase, cancellationToken);

                    }
                    
                    
                }
                else
                {
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

                }
                
                
                


                var success = await _context.SaveChangesAsync(cancellationToken) > 0;

                if (success) return Unit.Value;

                throw new Exception("Problem saving changes");
            }
        }
    }
}
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

namespace Services.Languages.Commands
{
    public class Create
    {
        public class Command:IRequest
        {
            public string Name { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Name).NotEmpty();
            }
        }
        
        public class Handler:IRequestHandler<Command>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var isLanguageAlreadyExist =await _context.Languages.AnyAsync(l=>l.Name == request.Name);

                if (isLanguageAlreadyExist) throw new RestException(HttpStatusCode.Conflict, new {Language = "Already exists"});
                

                var language = new Language
                {
                    Name = request.Name,
                    CreatedAt = DateTime.Now
                };

                await _context.Languages.AddAsync(language);

                var success = await _context.SaveChangesAsync() > 0;

                if (success) return Unit.Value;

                throw new Exception("Problem saving changes");
            }
        }
    }
}
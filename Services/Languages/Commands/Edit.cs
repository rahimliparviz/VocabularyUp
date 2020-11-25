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

namespace Services.Languages.Commands
{
    public class Edit
    {
        public class Command:IRequest
        {
            public Guid Id { get; set; }
            public string Name { get; set; }

        }
        
        public class CommandValidator:AbstractValidator<Command>
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
               var language =await _context.Languages.FindAsync(request.Id);

               _ = language ?? throw new RestException(HttpStatusCode.BadRequest,new {Language = "Not found"});

               var isLanguageAlreadyExist = await _context.Languages.AnyAsync(l=>l.Id != request.Id && l.Name == request.Name);

               if (isLanguageAlreadyExist) throw new RestException(HttpStatusCode.BadRequest, new {Language = "Not found"});
               

               language.Name = request.Name;

               var success = await _context.SaveChangesAsync() > 0;
               if (success) return Unit.Value;

               throw new Exception("Problem saving changes");
            }
        }
    }
}
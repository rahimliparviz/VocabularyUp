using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Data;
using Infrastructure.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Services.Translations.Commands
{
    public class Delete
    {
        public class Command:IRequest
        {
            public Guid Id { get; set; }
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
                var translation = await _context.Translations.FindAsync(request.Id);

                _ = translation ?? throw new RestException(HttpStatusCode.NotFound, new {Translation = "Not Found"});

                _context.Translations.Remove(translation);
                var success = _context.SaveChanges() > 0;
                if (success)
                {
                    return Unit.Value;
                }

                throw new Exception("Problem on deleting");

            }
        }
    }
}
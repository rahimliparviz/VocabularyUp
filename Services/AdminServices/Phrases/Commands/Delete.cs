using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Data;
using Infrastructure.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Services.Phrases.Commands
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
                var phrase = await _context.Phrases.FindAsync(request.Id);

                _ = phrase ?? throw new RestException(HttpStatusCode.NotFound, new {Phrase = "Not Found"});

                _context.Phrases.Remove(phrase);
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
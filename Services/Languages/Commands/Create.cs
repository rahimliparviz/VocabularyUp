using System.Threading;
using System.Threading.Tasks;
using Data;
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
        
        public class Handler:IRequestHandler<Command>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var language =await _context.Languages.AnyAsync(l=>l.Name == request.Name);
                throw new System.NotImplementedException();
            }
        }
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;
using Data;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Services.Languages.Queries
{
    public class Details
    {
        public class Query : IRequest<Language> {
            public Guid Id { get; set; }
        }
        
        
        public class Handler:IRequestHandler<Query,Language>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }
            public async Task<Language> Handle(Query request, CancellationToken cancellationToken)
            {
                var language = await _context.Languages.FirstOrDefaultAsync(l => l.Id == request.Id);

                _ = language ?? throw new Exception();
                
                return language;
            }
        }
    }
}
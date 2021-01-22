using System;
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

namespace Services.Phrases.Queries
{
    public class Details
    {
        public class Query : IRequest<PhraseGetDto> {
            public Guid Id { get; set; }
        }
        
        
        public class Handler:IRequestHandler<Query, PhraseGetDto>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<PhraseGetDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var phrase = await _context.Phrases.FirstOrDefaultAsync(l => l.Id == request.Id);

                _ = phrase ?? throw new RestException(HttpStatusCode.NotFound,new {Phrase = "Not Found"});
                
                return _mapper.Map<Phrase,PhraseGetDto>(phrase);
            }
        }
    }
}
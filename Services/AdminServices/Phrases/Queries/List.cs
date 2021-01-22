using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Data;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.DTO;

namespace Services.Phrases.Queries
{
    public class List
    {
        public class Query : IRequest<List<PhraseGetDto>> {}
        
        
        public class Handler:IRequestHandler<Query, List<PhraseGetDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context,IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<List<PhraseGetDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var phrases = await _context.Phrases.ToListAsync();
                
                return _mapper.Map<List<Phrase>, List<PhraseGetDto>>(phrases);
            }
        }
    }
}
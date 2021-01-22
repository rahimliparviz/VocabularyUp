using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Data;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.DTO;

namespace Services.Translations.Queries
{
    public class List
    {
        public class Query : IRequest<List<TranslationGetDto>> {}
        
        
        public class Handler:IRequestHandler<Query, List<TranslationGetDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<List<TranslationGetDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var translations = await _context.Translations.ToListAsync();
                

                return _mapper.Map<List<Translation>, List<TranslationGetDto>>(translations);
            }
        }
    }
}
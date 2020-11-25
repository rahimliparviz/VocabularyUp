using System;
using System.Collections.Generic;
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

namespace Services.Languages.Queries
{
    public class List
    {
        public class Query : IRequest<List<LanguageGetDto>> {}
        
        
        public class Handler:IRequestHandler<Query, List<LanguageGetDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context,IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<List<LanguageGetDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var languages = await _context.Languages.ToListAsync();
               
                return _mapper.Map<List<Language>, List<LanguageGetDto>>(languages);
            }
        }
    }
}
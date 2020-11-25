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

namespace Services.Languages.Queries
{
    public class Details
    {
        public class Query : IRequest<LanguageGetDto> {
            public Guid Id { get; set; }
        }
        
        
        public class Handler:IRequestHandler<Query,LanguageGetDto>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context,IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<LanguageGetDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var language = await _context.Languages.FirstOrDefaultAsync(l => l.Id == request.Id);

                _ = language ?? throw new RestException(HttpStatusCode.NotFound,new {Language = "Not Found"});
                
                return _mapper.Map<Language,LanguageGetDto>(language);
                
                
            }
        }
    }
}
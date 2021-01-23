﻿using System;
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

namespace Services.Translations.Queries
{
    public class GetUserPhrasesPerLanguage
    {
        public class Query : IRequest<TranslationGetDto>
        {
            public Guid FromLanguageId { get; set; }
            public Guid ToLanguageId { get; set; }
        }



        public class Handler : IRequestHandler<Query, TranslationGetDto>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<TranslationGetDto> Handle(Query request, CancellationToken cancellationToken)
            {


                var articles = await _context.Articles
                    .Include(a => a.ArticleTags)
                    .ThenInclude(at => at.Tag)
                    .Select(a => new
                    {
                        Id = a.Id,
                        ArticleName = a.ArticleName,
                        Tags = a.ArticleTags.Select(at => at.Tag).ToList()
                    })
                    .ToListAsync();
                
                var translation = await _context.Translations.FirstOrDefaultAsync(l => l.Id == request.Id);

                _ = translation ?? throw new RestException(HttpStatusCode.NotFound, new {Translation = "Not Found"});

                return _mapper.Map<Translation, TranslationGetDto>(translation);
            }
        }



    }
}
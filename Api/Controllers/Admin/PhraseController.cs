using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Services.DTO;
using Services.Phrases.Queries;
using Services.Phrases.Commands;

namespace Api.Controllers.Admin
{
    public class PhraseController : BaseController{
        
        [HttpGet]
        public async Task<List<PhraseGetDto>> List()
        {
            return await Mediator.Send(new List.Query());
        }
        
        [HttpGet("{id}")]
        public async Task<PhraseGetDto> Details(Guid id)
        {
            return await Mediator.Send(new Details.Query {Id = id});
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Create([FromForm] Create.Command command)
        {
            return await Mediator.Send(command);
        }

      
        [HttpPost("phrase-with-translation")]
        public async Task<ActionResult<Unit>> CreateWithTranslations(CreateWithTranslations.Command command)
        {
            return await Mediator.Send(command);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Edit(Guid id, [FromForm] Edit.Command command)
        {
            command.Id = id;
            return await Mediator.Send(command);
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(Guid id)
        {
            return await Mediator.Send(new Delete.Command {Id = id});
        }

        [HttpPost("upload")]
        public async Task<ActionResult<Unit>> Add([FromForm] FileUpload.Command command)
        {
            return await Mediator.Send(command);
        }
    }
}
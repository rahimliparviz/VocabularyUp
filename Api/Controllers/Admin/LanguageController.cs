using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Services.DTO;
using Services.Languages.Commands;
using Services.Languages.Queries;

namespace Api.Controllers.Admin
{
  
    public class LanguageController : BaseController
    {
   
 
        [HttpGet]
        public async Task<ActionResult<List<LanguageGetDto>>> List()
        {
            return await Mediator.Send(new List.Query());
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<LanguageGetDto>> Details(Guid id)
        {
            return await Mediator.Send(new Details.Query {Id = id});
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Create(Create.Command command)
        {
            return await Mediator.Send(command);
        }

  
        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Edit(Guid id,Edit.Command command)
        {
            command.Id = id;
            return await Mediator.Send(command);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(Guid id)
        {
            return await Mediator.Send(new Delete.Command{Id = id});
        }
    }
}
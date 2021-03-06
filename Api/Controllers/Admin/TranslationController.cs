﻿// using System;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using MediatR;
// using Microsoft.AspNetCore.Mvc;
// using Services.DTO;
// using Services.Translations.Commands;
// using Services.Translations.Queries;
//
//
// namespace Api.Controllers.Admin
// {
//     public class TranslationController : BaseController{
//         
//         [HttpGet]
//         public async Task<List<TranslationGetDto>> List()
//         {
//             return await Mediator.Send(new List.Query());
//         }
//         
//         [HttpGet("{id}")]
//         public async Task<TranslationGetDto> Details(Guid id)
//         {
//             return await Mediator.Send(new Details.Query {Id = id});
//         }
//
//         [HttpPost]
//         public async Task<ActionResult<Unit>> Create([FromForm] Create.Command command)
//         {
//             return await Mediator.Send(command);
//         }
//
//         [HttpPut("{id}")]
//         public async Task<ActionResult<Unit>> Edit(Guid id, [FromForm] Edit.Command command)
//         {
//             command.Id = id;
//             return await Mediator.Send(command);
//         }
//         
//         [HttpDelete("{id}")]
//         public async Task<ActionResult<Unit>> Delete(Guid id)
//         {
//             return await Mediator.Send(new Delete.Command {Id = id});
//         }
//     }
// }
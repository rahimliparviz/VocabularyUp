using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Services.DTO;
using Services.Phrases.Commands;
using Services.UserServices.Actions.Commands;
using Services.UserServices.Actions.Queries;

namespace Api.Controllers
{
    public class UserController:BaseController
    {
        [HttpGet("phrases-to-learn")]
        public async Task<ActionResult<List<PhrasesWithTranslationDto>>> PhrasesToLearn([FromBody] GetNewPhrasesToLearn.Query query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("user-phrases")]
        public async Task<ActionResult<List<PhrasesWithTranslationDto>>> UserPhrases([FromBody] GetUserPhrases.Query query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost("already-known-phrase")]
        public async Task<ActionResult<Unit>> AlreadyKnownPhrase([FromBody] AlreadyKnownPhrase.Command command)
        {
            return await Mediator.Send(command);
        }

        [HttpPost("forget-translation")]
        public async Task<ActionResult<Unit>> ForgetTranslation([FromBody] ForgetTranslation.Command command)
        {
            return await Mediator.Send(command);
        }

        [HttpPost("learn-phrase")]
        public async Task<ActionResult<Unit>> LearnPhrase([FromBody] LearnPhrase.Command command)
        {
            return await Mediator.Send(command);
        }

        [HttpPost("phrases-from-file")]
        public async Task<ActionResult<List<PhrasesWithTranslationDto>>> Add([FromForm] PhrasesFromFile.Command command)
        {
            return await Mediator.Send(command);
        }


    }
}
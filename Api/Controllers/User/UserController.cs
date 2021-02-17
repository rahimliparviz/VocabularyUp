using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Services.DTO;
using Services.Phrases.Commands;
using Services.UserServices.Actions.Commands;
using Services.UserServices.Actions.Queries;

namespace Api.Controllers.User
{
    public class UserController:BaseController
    {
        [HttpGet("phrases-to-learn")]
        public async Task<ActionResult<List<PhrasesWithTranslationDto>>> PhrasesToLearn(Guid fromLanguageId,
            Guid toLanguageId,
            int phrasesCount)
        {
            return await Mediator.Send(new GetNewPhrasesToLearn.Query()
                {PhrasesCount = phrasesCount, FromLanguageId = fromLanguageId, ToLanguageId = toLanguageId});
        }

        [HttpGet("user-phrases")]
        // public async Task<ActionResult<List<PhrasesWithTranslationDto>>> UserPhrases([FromBody] GetUserPhrases.Query query)
        public async Task<ActionResult<List<PhrasesWithTranslationDto>>> UserPhrases(
            Guid fromLanguageId,
            Guid toLanguageId, 
            string filter)
        {
            // return await Mediator.Send(query);
            return await Mediator.Send(new GetUserPhrases.Query(){Filter = filter,FromLanguageId = fromLanguageId,ToLanguageId = toLanguageId});
        }

        [HttpPost("user-profile")]
        public async Task<ActionResult<UserProfileDto>> UserProfile(
            [FromBody] UserProfile.Query query)
        {
            return await Mediator.Send(query);
        }

        /// <summary>
        /// Eger usere gosterilen sozu bilirse,user bu actionu secir
        /// </summary>
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
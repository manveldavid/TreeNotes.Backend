using Application.Actions.Note.Commands.Create;
using Application.Actions.Note.Commands.Delete;
using Application.Actions.Note.Commands.Update;
using Application.Actions.Note.Queries.Childs;
using Application.Actions.Note.Queries.FromDate;
using Application.Actions.Note.Queries.FromDescription;
using Application.Actions.Note.Queries.FromId;
using Application.Actions.Note.Queries.FromTitle;
using Application.Actions.Note.Queries.ParentCheck;
using Application.Actions.Note.Queries.Parents;
using Application.Actions.Note.Queries.RootNotes;
using Application.Actions.User.Queries.UserFromPassCode;
using Application.Common.Exceptions;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using WebAPI.Common;

namespace WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class NotesController : TreeNotesControllerBase
    {
        /// <summary>Get root note list</summary>
        /// <param name="token">JWT { sub: passcode }</param>
        /// <returns>Return ICollection of TreeNote</returns>
        /// <remarks>
        /// Sample request:
        /// GET /notes/root?token
        /// </remarks>
        /// <response code = "200">Success</response>
        /// <response code = "404">If user not found</response>
        /// <response code = "401">If user token is wrong</response>
        /// <response code = "400">If request does not contain required parameter</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("root")]
        [Authorize]
        public async Task<ActionResult<ICollection<TreeNote>>> RootNotes([FromQuery] string token)
        {
            var jwt = new JwtSecurityToken(token);
            var claim = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
            if (claim == null) { return BadRequest(); }
            var passcode = claim.Value ?? string.Empty;
            var userQuery = new UserFromPassCodeQuery
            {
                PassCode = passcode ?? throw new InvalidRequestException(nameof(passcode))
            };
            var user = await Mediator.Send(userQuery);

            var noteQuery = new RootNotesQuery
            {
                UserId = user.Id,
            };
            var result = await Mediator.Send(noteQuery);
            return Ok(result);
        }

        /// <summary>Get root note list</summary>
        /// <param name="token">JWT { sub: passcode }</param>
        /// <returns>Return ICollection of TreeNote</returns>
        /// <remarks>
        /// Sample request:
        /// GET /notes?token
        /// </remarks>
        /// <response code = "200">Success</response>
        /// <response code = "404">If user not found</response>
        /// <response code = "401">If user token is wrong</response>
        /// <response code = "400">If request does not contain required parameter</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("")]
        [Authorize]
        public async Task<ActionResult<ICollection<TreeNote>>> AllNotes([FromQuery] string token)
        {
            var jwt = new JwtSecurityToken(token);
            var claim = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
            if (claim == null) { return BadRequest(); }
            var passcode = claim.Value ?? string.Empty;
            var userQuery = new UserFromPassCodeQuery
            {
                PassCode = passcode ?? throw new InvalidRequestException(nameof(passcode))
            };
            var user = await Mediator.Send(userQuery);

            var noteQuery = new FromDateQuery
            {
                UserId = user.Id,
                Date = DateTime.MinValue
            };
            var result = await Mediator.Send(noteQuery);
            return Ok(result);
        }

        /// <summary>Get note context</summary>
        /// <param name="id">Note id (Guid)</param>
        /// <param name="token">JWT { sub: passcode }</param>
        /// <returns>Return TreeNote</returns>
        /// <remarks>
        /// Sample request:
        /// GET /notes/{id}?token
        /// </remarks>
        /// <response code = "200">Success</response>
        /// <response code = "404">If user or note not found</response>
        /// <response code = "403">If access to the note is denied</response>
        /// <response code = "401">If user token is wrong</response>
        /// <response code = "400">If request does not contain required parameter</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<TreeNote>> FromId([FromRoute] string id, [FromQuery] string token)
        {
            var jwt = new JwtSecurityToken(token);
            var claim = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
            if (claim == null) { return BadRequest(); }
            var passcode = claim.Value ?? string.Empty;
            var userQuery = new UserFromPassCodeQuery
            {
                PassCode = passcode ?? throw new InvalidRequestException(nameof(passcode))
            };
            var user = await Mediator.Send(userQuery);
            Guid NoteId = Guid.Empty;
            if(!Guid.TryParse(id, out NoteId)) { throw new InvalidRequestException(nameof(id)); }
            var noteQuery = new FromIdQuery
            {
                UserId = user.Id,
                NoteId = NoteId
            };
            var result = await Mediator.Send(noteQuery);
            return Ok(result);
        }

        /// <summary>Get note childs</summary>
        /// <param name="id">Note id (Guid)</param>
        /// <param name="token">JWT { sub: passcode }</param>
        /// <returns>Return ICollection of TreeNote</returns>
        /// <remarks>
        /// Sample request:
        /// GET /notes/{id}/childs?token
        /// </remarks>
        /// <response code = "200">Success</response>
        /// <response code = "404">If user or note not found</response>
        /// <response code = "401">If user token is wrong</response>
        /// <response code = "400">If request does not contain required parameter</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("{id}/childs")]
        [Authorize]
        public async Task<ActionResult<ICollection<TreeNote>>> Childs([FromRoute] string id, [FromQuery] string token)
        {
            var jwt = new JwtSecurityToken(token);
            var claim = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
            if (claim == null) { return BadRequest(); }
            var passcode = claim.Value ?? string.Empty;
            var userQuery = new UserFromPassCodeQuery
            {
                PassCode = passcode ?? throw new InvalidRequestException(nameof(passcode))
            };
            var user = await Mediator.Send(userQuery);
            Guid NoteId = Guid.Empty;
            if (!Guid.TryParse(id, out NoteId)) { throw new InvalidRequestException(nameof(id)); }
            var noteQuery = new ChildsQuery
            {
                UserId = user.Id,
                NoteId = NoteId
            };
            var result = await Mediator.Send(noteQuery);
            return Ok(result);
        }

        /// <summary>Get note parents</summary>
        /// <param name="id">Note id (Guid)</param>
        /// <param name="token">JWT { sub: passcode }</param>
        /// <returns>Return ICollection of TreeNote</returns>
        /// <remarks>
        /// Sample request:
        /// GET /notes/{id}/parents?token
        /// </remarks>
        /// <response code = "200">Success</response>
        /// <response code = "404">If user or note not found</response>
        /// <response code = "403">If access to the note is denied</response>
        /// <response code = "401">If user token is wrong</response>
        /// <response code = "400">If request does not contain required parameter</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("{id}/parents")]
        [Authorize]
        public async Task<ActionResult<ICollection<TreeNote>>> Parents([FromRoute] string id, [FromQuery] string token)
        {
            var jwt = new JwtSecurityToken(token);
            var claim = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
            if (claim == null) { return BadRequest(); }
            var passcode = claim.Value ?? string.Empty;
            var userQuery = new UserFromPassCodeQuery
            {
                PassCode = passcode ?? throw new InvalidRequestException(nameof(passcode))
            };
            var user = await Mediator.Send(userQuery);
            Guid NoteId = Guid.Empty;
            if (!Guid.TryParse(id, out NoteId)) { throw new InvalidRequestException(nameof(id)); }
            var noteQuery = new ParentsQuery
            {
                UserId = user.Id,
                NoteId = NoteId
            };
            var result = await Mediator.Send(noteQuery);
            return Ok(result);
        }

        /// <summary>Checks if note is checked</summary>
        /// <param name="id">Note id (Guid)</param>
        /// <param name="token">JWT { sub: passcode }</param>
        /// <returns>Return true/false(bool)</returns>
        /// <remarks>
        /// Sample request:
        /// GET /notes/{id}/check?token
        /// </remarks>
        /// <response code = "200">Success</response>
        /// <response code = "404">If user or note not found</response>
        /// <response code = "403">If access to the note is denied</response>
        /// <response code = "401">If user token is wrong</response>
        /// <response code = "400">If request does not contain required parameter</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("{id}/check")]
        [Authorize]
        public async Task<ActionResult<TreeNote>> IsParentCheck([FromRoute] string id, [FromQuery] string token)
        {
            var jwt = new JwtSecurityToken(token);
            var claim = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
            if (claim == null) { return BadRequest(); }
            var passcode = claim.Value ?? string.Empty;
            var userQuery = new UserFromPassCodeQuery
            {
                PassCode = passcode ?? throw new InvalidRequestException(nameof(passcode))
            };
            var user = await Mediator.Send(userQuery);
            Guid NoteId = Guid.Empty;
            if (!Guid.TryParse(id, out NoteId)) { throw new InvalidRequestException(nameof(id)); }
            var noteQuery = new ParentCheckQuery
            {
                UserId = user.Id,
                NoteId = NoteId
            };
            var result = await Mediator.Send(noteQuery);
            return Ok(result);
        }

        /// <summary>Get all notes with fragment in their title or description</summary>
        /// <param name="fragment">Fragment of text (string)</param>
        /// <param name="token">JWT { sub: passcode }</param>
        /// <returns>Return ICollection of TreeNote</returns>
        /// <remarks>
        /// Sample request:
        /// GET /notes/fragment?fragment&token
        /// </remarks>
        /// <response code = "200">Success</response>
        /// <response code = "404">If user or note not found</response>
        /// <response code = "401">If user token is wrong</response>
        /// <response code = "400">If request does not contain required parameter</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("fragment")]
        [Authorize]
        public async Task<ActionResult<ICollection<TreeNote>>> Fragment([FromQuery] string fragment, [FromQuery] string token)
        {
            var jwt = new JwtSecurityToken(token);
            var claim = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
            if (claim == null) { return BadRequest(); }
            var passcode = claim.Value ?? string.Empty;
            var userQuery = new UserFromPassCodeQuery
            {
                PassCode = passcode ?? throw new InvalidRequestException(nameof(passcode))
            };
            var user = await Mediator.Send(userQuery);

            var fromTItle = new FromTitleQuery
            {
                UserId = user.Id,
                Fragment = fragment ?? throw new InvalidRequestException(nameof(fragment))
            };
            var resultFromTitle = await Mediator.Send(fromTItle);
            resultFromTitle = resultFromTitle.ToList();

            var fromDescription = new FromDescriptionQuery
            {
                UserId = user.Id,
                Fragment = fragment ?? throw new InvalidRequestException(nameof(fragment))
            };
            var resultFromDescription = await Mediator.Send(fromDescription);
            resultFromDescription = resultFromDescription.ToList();

            return Ok(resultFromTitle.Concat(resultFromDescription));
        }

        /// <summary>Get all notes with fragment in their title or description</summary>
        /// <param name="ticks">DateTime in ticks</param>
        /// <param name="token">JWT { sub: passcode }</param>
        /// <returns>Return ICollection of TreeNote</returns>
        /// <remarks>
        /// Sample request:
        /// GET /notes/ticks?ticks&token
        /// </remarks>
        /// <response code = "200">Success</response>
        /// <response code = "404">If user or note not found</response>
        /// <response code = "401">If user token is wrong</response>
        /// <response code = "400">If request does not contain required parameter</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("ticks")]
        [Authorize]
        public async Task<ActionResult<ICollection<TreeNote>>> Ticks([FromQuery] string ticks, [FromQuery] string token)
        {
            var jwt = new JwtSecurityToken(token);
            var claim = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
            if (claim == null) { return BadRequest(); }
            var passcode = claim.Value ?? string.Empty;
            var userQuery = new UserFromPassCodeQuery
            {
                PassCode = passcode ?? throw new InvalidRequestException(nameof(passcode))
            };
            var user = await Mediator.Send(userQuery);
            long ticksValue = DateTime.MaxValue.Ticks;
            if(!long.TryParse(ticks, out ticksValue)) { throw new InvalidRequestException(nameof(ticks));  }
            var query = new FromDateQuery
            {
                UserId = user.Id,
                Date = new DateTime(ticksValue)
            };
            var result = await Mediator.Send(query);

            return Ok(result.ToList());
        }

        /// <summary>Creates note</summary>
        /// <param name="request">User request DTO</param>
        /// <param name="token">JWT { sub: passcode }</param>
        /// <returns>Return noteId (Guid)</returns>
        /// <remarks>
        /// Sample request:
        /// POST /notes?token
        /// {
        ///     "parent": "parentNoteId (guid)", //is note required
        ///     "user" : "invitedUserId (Guid)", //is note required
        ///     "description" : "noteDescription", //is note required
        ///     "title" : "noteDescription", //is note required
        ///     "number" : "noteNumber", //is note required
        ///     "weight" : "noteWeight", //is note required
        ///     "check" : "noteCheck", //is note required
        ///     "share" : "noteShare" //is note required
        /// }
        /// </remarks>
        /// <response code = "200">Success</response>
        /// <response code = "404">If user not found</response>
        /// <response code = "401">If user token is wrong</response>
        /// <response code = "400">If request does not contain required parameter</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("")]
        [Authorize]
        public async Task<ActionResult<Guid>> Create([FromQuery] string token, [FromBody] UserRequestBodyDto request)
        {
            var jwt = new JwtSecurityToken(token);
            var claim = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
            if (claim == null) { return BadRequest(); }
            var passcode = claim.Value ?? string.Empty;
            var userQuery = new UserFromPassCodeQuery
            {
                PassCode = passcode ?? throw new InvalidRequestException(nameof(passcode))
            };
            var user = await Mediator.Send(userQuery);

            var noteCommand = new CreateCommand
            {
                UserId = user.Id,
                Parent = request.Parent ?? Guid.Empty,
            };
            var noteId = await Mediator.Send(noteCommand);

            var updateCommand = new UpdateCommand
            {
                UserId = user.Id,
                NoteId = noteId,

                Description = request.Description,
                Title = request.Title,
                User = request.User,
                Number = request.Number,
                Weight = request.Weight,
                Share = request.Share,
                Check = request.Check
            };
            await Mediator.Send(updateCommand);
            return Ok(noteId);
        }

        /// <summary>Creates note</summary>
        /// <param name="request">User request DTO</param>
        /// <param name="id">Note id (Guid)</param>
        /// <param name="token">JWT { sub: passcode }</param>
        /// <returns>Return noteId (Guid)</returns>
        /// <remarks>
        /// Sample request:
        /// POST /notes/{id}?token
        /// {
        ///     "user" : "invitedUserId (Guid)", //is note required
        ///     "description" : "noteDescription", //is note required
        ///     "title" : "noteDescription", //is note required
        ///     "number" : "noteNumber", //is note required
        ///     "weight" : "noteWeight", //is note required
        ///     "check" : "noteCheck", //is note required
        ///     "share" : "noteShare" //is note required
        /// }
        /// </remarks>
        /// <response code = "200">Success</response>
        /// <response code = "404">If user not found</response>
        /// <response code = "401">If user token is wrong</response>
        /// <response code = "400">If request does not contain required parameter</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("{id}")]
        [Authorize]
        public async Task<ActionResult<Guid>> CreateEntire([FromQuery] string token, [FromRoute] string id, [FromBody] UserRequestBodyDto request)
        {
            var jwt = new JwtSecurityToken(token);
            var claim = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
            if (claim == null) { return BadRequest(); }
            var passcode = claim.Value ?? string.Empty;
            var userQuery = new UserFromPassCodeQuery
            {
                PassCode = passcode ?? throw new InvalidRequestException(nameof(passcode))
            };
            var user = await Mediator.Send(userQuery);
            Guid NoteId = Guid.Empty;
            if (!Guid.TryParse(id, out NoteId)) { throw new InvalidRequestException(nameof(id)); }

            var noteCommand = new CreateCommand
            {
                UserId = user.Id,
                Parent = NoteId,
            };
            var noteId = await Mediator.Send(noteCommand);

            var updateCommand = new UpdateCommand
            {
                UserId = user.Id,
                NoteId = noteId,

                Description = request.Description,
                Title = request.Title,
                User = request.User,
                Number = request.Number,
                Weight = request.Weight,
                Share = request.Share,
                Check = request.Check
            };
            await Mediator.Send(updateCommand);
            return Ok(noteId);
        }

        /// <summary>Updates note</summary>
        /// <param name="request">User request DTO</param>
        /// <param name="id">Note id (Guid)</param>
        /// <param name="token">JWT { sub: passcode }</param>
        /// <returns>Return NoContent</returns>
        /// <remarks>
        /// Sample request:
        /// PATCH /notes/{id}?token
        /// {
        ///     "parent" : "parentNoteId (Guid)", //is note required
        ///     "user" : "invitedUserId (Guid)", //is note required
        ///     "description" : "noteDescription", //is note required
        ///     "title" : "noteDescription", //is note required
        ///     "number" : "noteNumber", //is note required
        ///     "weight" : "noteWeight", //is note required
        ///     "check" : "noteCheck", //is note required
        ///     "share" : "noteShare" //is note required
        /// }
        /// </remarks>
        /// <response code = "204">Success</response>
        /// <response code = "404">If user or note not found</response>
        /// <response code = "403">If access to the note is denied</response>
        /// <response code = "401">If user token is wrong</response>
        /// <response code = "400">If request does not contain required parameter</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPatch("{id}")]
        [Authorize]
        public async Task<ActionResult> Update([FromQuery] string token, [FromRoute] string id, [FromBody] UserRequestBodyDto request)
        {
            var jwt = new JwtSecurityToken(token);
            var claim = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
            if (claim == null) { return BadRequest(); }
            var passcode = claim.Value ?? string.Empty;
            var userQuery = new UserFromPassCodeQuery
            {
                PassCode = passcode ?? throw new InvalidRequestException(nameof(passcode))
            };
            var user = await Mediator.Send(userQuery);
            Guid NoteId = Guid.Empty;
            if (!Guid.TryParse(id, out NoteId)) { throw new InvalidRequestException(nameof(id)); }

            var noteCommand = new UpdateCommand
            {
                UserId = user.Id,
                NoteId = NoteId,

                Description = request.Description,
                Title = request.Title,
                Parent = request.Parent,
                User = request.User,
                Number = request.Number,
                Weight = request.Weight,
                Share = request.Share,
                Check = request.Check
            };
            await Mediator.Send(noteCommand);
            return NoContent();
        }

        /// <summary>Deletes note</summary>
        /// <param name="id">Note id (Guid)</param>
        /// <param name="token">JWT { sub: passcode }</param>
        /// <returns>Return NoContent</returns>
        /// <remarks>
        /// Sample request:
        /// DELETE /notes/{id}?token
        /// </remarks>
        /// <response code = "204">Success</response>
        /// <response code = "404">If user or note not found</response>
        /// <response code = "403">If access to the note is denied or user password is wrong</response>
        /// <response code = "400">If request does not contain required parameter</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<Guid>> Delete([FromRoute] string id, [FromQuery] string token)
        {
            var jwt = new JwtSecurityToken(token);
            var claim = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
            if (claim == null) { return BadRequest(); }
            var passcode = claim.Value ?? string.Empty;
            var userQuery = new UserFromPassCodeQuery
            {
                PassCode = passcode ?? throw new InvalidRequestException(nameof(passcode))
            };
            var user = await Mediator.Send(userQuery);
            Guid NoteId = Guid.Empty;
            if (!Guid.TryParse(id, out NoteId)) { throw new InvalidRequestException(nameof(id)); }

            var noteCommand = new DeleteCommand
            {
                UserId = user.Id,
                NoteId = NoteId
            };
            await Mediator.Send(noteCommand);
            return NoContent();
        }
    }
}

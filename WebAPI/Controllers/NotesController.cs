using Application.Actions.Note.Commands.Create;
using Application.Actions.Note.Commands.Delete;
using Application.Actions.Note.Commands.Update;
using Application.Actions.Note.Queries.Childs;
using Application.Actions.Note.Queries.FromDescription;
using Application.Actions.Note.Queries.FromId;
using Application.Actions.Note.Queries.FromTitle;
using Application.Actions.Note.Queries.ParentCheck;
using Application.Actions.Note.Queries.Parents;
using Application.Actions.Note.Queries.RootNotes;
using Application.Actions.User.Queries.UserId;
using Application.Common.Exceptions;
using Domain;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common;

namespace WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class NotesController : TreeNotesControllerBase
    {
        /// <summary>Get root note list</summary>
        /// <param name="request">User request DTO</param>
        /// <returns>Return ICollection of TreeNote</returns>
        /// <remarks>
        /// Sample request:
        /// POST /notes/rootNotes
        /// {
        ///     "login": "userLogin",
        ///     "password": "userPassword",
        /// }
        /// </remarks>
        /// <response code = "200">Success</response>
        /// <response code = "404">If user not found</response>
        /// <response code = "403">If user password is wrong</response>
        /// <response code = "400">If request does not contain required parameter</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("rootNotes")]
        public async Task<ActionResult<ICollection<TreeNote>>> RootNotes([FromBody] UserRequestBodyDto request)
        {
            var userQuery = new UserIdQuery
            {
                Login = request.Login ?? throw new InvalidRequestException(nameof(request.Login)),
                Password = request.Password ?? throw new InvalidRequestException(nameof(request.Password))
            };
            var userId = await Mediator.Send(userQuery);

            var noteQuery = new RootNotesQuery
            {
                UserId = userId,
            };
            var result = await Mediator.Send(noteQuery);
            return Ok(result);
        }

        /// <summary>Get note context</summary>
        /// <param name="request">User request DTO</param>
        /// <returns>Return TreeNote</returns>
        /// <remarks>
        /// Sample request:
        /// POST /notes/context
        /// {
        ///     "login": "userLogin",
        ///     "password": "userPassword",
        ///     "noteId": "id (Guid)"
        /// }
        /// </remarks>
        /// <response code = "200">Success</response>
        /// <response code = "404">If user or note not found</response>
        /// <response code = "403">If access to the note is denied or user password is wrong</response>
        /// <response code = "400">If request does not contain required parameter</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("context")]
        public async Task<ActionResult<TreeNote>> FromId([FromBody] UserRequestBodyDto request)
        {
            var userQuery = new UserIdQuery
            {
                Login = request.Login ?? throw new InvalidRequestException(nameof(request.Login)),
                Password = request.Password ?? throw new InvalidRequestException(nameof(request.Password))
            };
            var userId = await Mediator.Send(userQuery);

            var noteQuery = new FromIdQuery
            {
                UserId = userId,
                NoteId = request.NoteId ?? throw new InvalidRequestException(nameof(request.NoteId))
            };
            var result = await Mediator.Send(noteQuery);
            return Ok(result);
        }

        /// <summary>Get note childs</summary>
        /// <param name="request">User request DTO</param>
        /// <returns>Return ICollection of TreeNote</returns>
        /// <remarks>
        /// Sample request:
        /// POST /notes/childs
        /// {
        ///     "login": "userLogin",
        ///     "password": "userPassword",
        ///     "noteId": "id (Guid)"
        /// }
        /// </remarks>
        /// <response code = "200">Success</response>
        /// <response code = "404">If user or note not found</response>
        /// <response code = "403">If user password is wrong</response>
        /// <response code = "400">If request does not contain required parameter</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("childs")]
        public async Task<ActionResult<ICollection<TreeNote>>> Childs([FromBody] UserRequestBodyDto request)
        {
            var userQuery = new UserIdQuery
            {
                Login = request.Login ?? throw new InvalidRequestException(nameof(request.Login)),
                Password = request.Password ?? throw new InvalidRequestException(nameof(request.Password))
            };
            var userId = await Mediator.Send(userQuery);

            var noteQuery = new ChildsQuery
            {
                UserId = userId,
                NoteId = request.NoteId ?? throw new InvalidRequestException(nameof(request.NoteId))
            };
            var result = await Mediator.Send(noteQuery);
            return Ok(result);
        }

        /// <summary>Get note parents</summary>
        /// <param name="request">User request DTO</param>
        /// <returns>Return ICollection of TreeNote</returns>
        /// <remarks>
        /// Sample request:
        /// POST /notes/parents
        /// {
        ///     "login": "userLogin",
        ///     "password": "userPassword",
        ///     "noteId": "id (Guid)"
        /// }
        /// </remarks>
        /// <response code = "200">Success</response>
        /// <response code = "404">If user or note not found</response>
        /// <response code = "403">If access to the note is denied or user password is wrong</response>
        /// <response code = "400">If request does not contain required parameter</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("parents")]
        public async Task<ActionResult<ICollection<TreeNote>>> Parents([FromBody] UserRequestBodyDto request)
        {
            var userQuery = new UserIdQuery
            {
                Login = request.Login ?? throw new InvalidRequestException(nameof(request.Login)),
                Password = request.Password ?? throw new InvalidRequestException(nameof(request.Password))
            };
            var userId = await Mediator.Send(userQuery);

            var noteQuery = new ParentsQuery
            {
                UserId = userId,
                NoteId = request.NoteId ?? throw new InvalidRequestException(nameof(request.NoteId))
            };
            var result = await Mediator.Send(noteQuery);
            return Ok(result);
        }

        /// <summary>Checks if note is checked</summary>
        /// <param name="request">User request DTO</param>
        /// <returns>Return true/false(bool)</returns>
        /// <remarks>
        /// Sample request:
        /// POST /notes/parentCheck
        /// {
        ///     "login": "userLogin",
        ///     "password": "userPassword",
        ///     "noteId": "id (Guid)"
        /// }
        /// </remarks>
        /// <response code = "200">Success</response>
        /// <response code = "404">If user or note not found</response>
        /// <response code = "403">If access to the note is denied or user password is wrong</response>
        /// <response code = "400">If request does not contain required parameter</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("parentCheck")]
        public async Task<ActionResult<TreeNote>> IsParentCheck([FromBody] UserRequestBodyDto request)
        {
            var userQuery = new UserIdQuery
            {
                Login = request.Login ?? throw new InvalidRequestException(nameof(request.Login)),
                Password = request.Password ?? throw new InvalidRequestException(nameof(request.Password))
            };
            var userId = await Mediator.Send(userQuery);

            var noteQuery = new ParentCheckQuery
            {
                UserId = userId,
                NoteId = request.NoteId ?? throw new InvalidRequestException(nameof(request.NoteId))
            };
            var result = await Mediator.Send(noteQuery);
            return Ok(result);
        }

        /// <summary>Get all notes with fragment in their title or description</summary>
        /// <param name="request">User request DTO</param>
        /// <returns>Return ICollection of TreeNote</returns>
        /// <remarks>
        /// Sample request:
        /// POST /notes/fragment
        /// {
        ///     "login": "userLogin",
        ///     "password": "userPassword",
        ///     "fragment": "string"
        /// }
        /// </remarks>
        /// <response code = "200">Success</response>
        /// <response code = "404">If user or note not found</response>
        /// <response code = "403">If user password is wrong</response>
        /// <response code = "400">If request does not contain required parameter</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("fragment")]
        public async Task<ActionResult<ICollection<TreeNote>>> FormTitle([FromBody] UserRequestBodyDto request)
        {
            var userQuery = new UserIdQuery
            {
                Login = request.Login ?? throw new InvalidRequestException(nameof(request.Login)),
                Password = request.Password ?? throw new InvalidRequestException(nameof(request.Password))
            };
            var userId = await Mediator.Send(userQuery);

            var fromTItle = new FromTitleQuery
            {
                UserId = userId,
                Fragment = request.Fragment ?? throw new InvalidRequestException(nameof(request.Fragment))
            };
            var resultFromTitle = await Mediator.Send(fromTItle);
            resultFromTitle = resultFromTitle.ToList();

            var fromDescription = new FromDescriptionQuery
            {
                UserId = userId,
                Fragment = request.Fragment ?? throw new InvalidRequestException(nameof(request.Fragment))
            };
            var resultFromDescription = await Mediator.Send(fromDescription);
            resultFromDescription = resultFromDescription.ToList();

            return Ok(resultFromTitle.Concat(resultFromDescription));
        }

        /// <summary>Creates note</summary>
        /// <param name="request">User request DTO</param>
        /// <returns>Return noteId (Guid)</returns>
        /// <remarks>
        /// Sample request:
        /// POST /notes/create
        /// {
        ///     "login": "userLogin",
        ///     "password": "userPassword",
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
        /// <response code = "403">If user password is wrong</response>
        /// <response code = "400">If request does not contain required parameter</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("create")]
        public async Task<ActionResult<Guid>> Create([FromBody] UserRequestBodyDto request)
        {
            var userQuery = new UserIdQuery
            {
                Login = request.Login ?? throw new InvalidRequestException(nameof(request.Login)),
                Password = request.Password ?? throw new InvalidRequestException(nameof(request.Password))
            };
            var userId = await Mediator.Send(userQuery);

            var noteCommand = new CreateCommand
            {
                UserId = userId,
                Parent = request.Parent ?? Guid.Empty,
            };
            var noteId = await Mediator.Send(noteCommand);

            var updateCommand = new UpdateCommand
            {
                UserId = userId,
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
        /// <returns>Return NoContent</returns>
        /// <remarks>
        /// Sample request:
        /// POST /notes/update
        /// {
        ///     "login": "userLogin",
        ///     "password": "userPassword",
        ///     "noteId": "noteId (Guid)",
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
        /// <response code = "403">If access to the note is denied or user password is wrong</response>
        /// <response code = "400">If request does not contain required parameter</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("update")]
        public async Task<ActionResult> Update([FromBody] UserRequestBodyDto request)
        {
            var userQuery = new UserIdQuery
            {
                Login = request.Login ?? throw new InvalidRequestException(nameof(request.Login)),
                Password = request.Password ?? throw new InvalidRequestException(nameof(request.Password))
            };
            var userId = await Mediator.Send(userQuery);

            var noteCommand = new UpdateCommand
            {
                UserId = userId,
                NoteId = request.NoteId ?? throw new InvalidRequestException(nameof(request.NoteId)),

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
        /// <param name="request">User request DTO</param>
        /// <returns>Return NoContent</returns>
        /// <remarks>
        /// Sample request:
        /// POST /notes/delete
        /// {
        ///     "login": "userLogin",
        ///     "password": "userPassword",
        ///     "noteId": "id (Guid)"
        /// }
        /// </remarks>
        /// <response code = "204">Success</response>
        /// <response code = "404">If user or note not found</response>
        /// <response code = "403">If access to the note is denied or user password is wrong</response>
        /// <response code = "400">If request does not contain required parameter</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("delete")]
        public async Task<ActionResult<Guid>> Delete([FromBody] UserRequestBodyDto request)
        {
            var userQuery = new UserIdQuery
            {
                Login = request.Login ?? throw new InvalidRequestException(nameof(request.Login)),
                Password = request.Password ?? throw new InvalidRequestException(nameof(request.Password))
            };
            var userId = await Mediator.Send(userQuery);

            var noteCommand = new DeleteCommand
            {
                UserId = userId,
                NoteId = request.NoteId ?? throw new InvalidRequestException(nameof(request.NoteId))
            };
            await Mediator.Send(noteCommand);
            return NoContent();
        }
    }
}

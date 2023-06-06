using Application.Actions.User.Commands.Delete;
using Application.Actions.User.Commands.ChangeLogin;
using Application.Actions.User.Commands.ChangePassword;
using Application.Actions.User.Commands.Create;
using Application.Actions.User.Queries.UserExist;
using Application.Actions.User.Queries.UserId;
using Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common;

namespace WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class UsersController:TreeNotesControllerBase
    {
        /// <summary>Get user id from login and password</summary>
        /// <param name="request">User request DTO</param>
        /// <returns>Return user id (Guid)</returns>
        /// <remarks>
        /// Sample request:
        /// POST /users/userId
        /// {
        ///     "login": "userLogin",
        ///     "password": "userPassword"
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
        [HttpPost("userId")]
        public async Task<ActionResult<Guid>> GetId([FromBody] UserRequestBodyDto request)
        {
            var query = new UserIdQuery
            {
                Login = request.Login ?? throw new InvalidRequestException(nameof(request.Login)),
                Password = request.Password ?? throw new InvalidRequestException(nameof(request.Password))
            };
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        /// <summary>User exsistance check</summary>
        /// <param name="request">User request DTO</param>
        /// <returns>Return true/false (bool)</returns>
        /// <remarks>
        /// Sample request:
        /// POST /users/userExist
        /// {
        ///     "login": "userLogin"
        /// }
        /// </remarks>
        /// <response code = "200">Success</response>
        /// <response code = "400">If request does not contain required parameter</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("userExist")]
        public async Task<ActionResult<bool>> UserExist([FromBody] UserRequestBodyDto request)
        {
            var query = new UserExistQuery
            {
                Login = request.Login ?? throw new InvalidRequestException(nameof(request.Login))
            };
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        /// <summary>Creates user with login and password</summary>
        /// <param name="request">User request DTO</param>
        /// <returns>Return user id (Guid)</returns>
        /// <remarks>
        /// Sample request:
        /// POST /users/create
        /// {
        ///     "login": "userLogin",
        ///     "password": "userPassword"
        /// }
        /// </remarks>
        /// <response code = "200">Success</response>
        /// <response code = "409">If user with this login exist</response>
        /// <response code = "400">If request does not contain required parameter</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("create")]
        public async Task<ActionResult<Guid>> Create([FromBody] UserRequestBodyDto request)
        {
            var query = new CreateCommand
            {
                Login = request.Login ?? throw new InvalidRequestException(nameof(request.Login)),
                Password = request.Password ?? throw new InvalidRequestException(nameof(request.Password))
            };
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        /// <summary>Update user login and password</summary>
        /// <param name="request">User request DTO</param>
        /// <returns>Return NoContent</returns>
        /// <remarks>
        /// Sample request:
        /// POST /users/update
        /// {
        ///     "login": "userLogin",
        ///     "password": "userPassword",
        ///     "newLogin": "newUserLogin", //is not required
        ///     "newPassword": "newUserPassword" //is not required
        /// }
        /// </remarks>
        /// <response code = "204">Success</response>
        /// <response code = "404">If user not found</response>
        /// <response code = "403">If user password is wrong</response>
        /// <response code = "400">If request does not contain required parameter</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("update")]
        public async Task<ActionResult> Update([FromBody] UserRequestBodyDto request)
        {
            if (request.NewLogin != null)
            {
                var query = new ChangeLoginCommand
                {
                    OldLogin = request.Login ?? throw new InvalidRequestException(nameof(request.Login)),
                    NewLogin = request.NewLogin,
                    Password = request.Password ?? throw new InvalidRequestException(nameof(request.Password))
                };
                await Mediator.Send(query);
            }
            if (request.NewPassword != null)
            {
                var query = new ChangePasswordCommand
                {
                    Login = request.NewLogin ??
                    request.Login ?? throw new InvalidRequestException(nameof(request.Login)),
                    NewPassword = request.NewPassword,
                    OldPassword = request.Password ?? throw new InvalidRequestException(nameof(request.Password))
                };
                await Mediator.Send(query);
            }

            return NoContent();
        }

        /// <summary>Delete user with login and password</summary>
        /// <param name="request">User request DTO</param>
        /// <returns>Return NoContent</returns>
        /// <remarks>
        /// Sample request:
        /// POST /users/delete
        /// {
        ///     "login": "userLogin",
        ///     "password": "userPassword"
        /// }
        /// </remarks>
        /// <response code = "204">Success</response>
        /// <response code = "404">If user not found</response>
        /// <response code = "403">If user password is wrong</response>
        /// <response code = "400">If request does not contain required parameter</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("delete")]
        public async Task<ActionResult> Delete([FromBody] UserRequestBodyDto request)
        {
            var query = new DeleteCommand
            {
                Login = request.Login ?? throw new InvalidRequestException(nameof(request.Login)),
                Password = request.Password ?? throw new InvalidRequestException(nameof(request.Password))
            };
            await Mediator.Send(query);
            return NoContent();
        }
    }
}

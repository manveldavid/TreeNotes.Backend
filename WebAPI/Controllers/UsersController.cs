using Application.Actions.User.Commands.Delete;
using Application.Actions.User.Commands.ChangeLogin;
using Application.Actions.User.Commands.ChangePassword;
using Application.Actions.User.Commands.Create;
using Application.Actions.User.Queries.UserExist;
using Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Application.Actions.User.Queries.UserFromPassCode;
using Application.Common.Workers;

namespace WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class UsersController:TreeNotesControllerBase
    {
        /// <summary>Get user id</summary>
        /// <param name="token">JWT {sub: passcode}</param>
        /// <returns>Return user id (Guid)</returns>
        /// <response code = "200">Success</response>
        /// <response code = "404">If user not found</response>
        /// <response code = "401">If user token is wrong</response>
        /// <response code = "400">If request does not contain required parameter</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("id")]
        [Authorize]
        public async Task<ActionResult<Guid>> GetId([FromQuery] string token)
        {
            var jwt = new JwtSecurityToken(token);
            var claim = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
            if(claim == null) { return BadRequest(); }
            var passcode = claim.Value ?? string.Empty;
            var query = new UserFromPassCodeQuery { PassCode = passcode };
            var user = await Mediator.Send(query);
            if(user == null) { return NotFound(); }
            return Ok(user.Id);
        }

        /// <summary>Get user passcode from login and password</summary>
        /// <param name="request">User request DTO</param>
        /// <returns>Return user id (Guid)</returns>
        /// <remarks>
        /// Sample request:
        /// PUT /users/passcode
        /// {
        ///     "login": "userLogin",
        ///     "password": "userPassword"
        /// }
        /// </remarks>
        /// <response code = "200">Success</response>
        /// <response code = "400">If request does not contain required parameter</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("passcode")]
        public async Task<ActionResult<Guid>> GetPassCode([FromBody] UserRequestBodyDto request)
        {
            var login = request.Login ?? throw new InvalidRequestException(nameof(request.Login));
            var password = request.Password ?? throw new InvalidRequestException(nameof(request.Password));
            var passcode = TreeNoteUserWorker.Encoder.CodeFromLoginPassword(login, password);
            return Ok(passcode);
        }

        /// <summary>User exsistance check</summary>
        /// <param name="login">User login</param>
        /// <returns>Return true/false (bool)</returns>
        /// <remarks>
        /// Sample request:
        /// GET /users/exist?login
        /// </remarks>
        /// <response code = "200">Success</response>
        /// <response code = "400">If request does not contain required parameter</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("exist")]
        public async Task<ActionResult<bool>> UserExist([FromQuery] string login)
        {
            var query = new UserExistQuery
            {
                Login = login ?? throw new InvalidRequestException(nameof(login))
            };
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        /// <summary>Creates user with login and password</summary>
        /// <param name="request">User request DTO</param>
        /// <returns>Return user id (Guid)</returns>
        /// <remarks>
        /// Sample request:
        /// POST /
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
        [HttpPost("")]
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
        /// PUT /users
        /// {
        ///     "Login": "UserLogin", 
        ///     "Password": "UserPassword"
        ///     "newLogin": "newUserLogin", //is not required
        ///     "newPassword": "newUserPassword" //is not required
        /// }
        /// </remarks>
        /// <response code = "204">Success</response>
        /// <response code = "404">If user not found</response>
        /// <response code = "401">If user token is wrong</response>
        /// <response code = "400">If request does not contain required parameter</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("")]
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
        /// <param name="token">JWT {sub: passcode}</param>
        /// <returns>Return NoContent</returns>
        /// <remarks>
        /// Sample request:
        /// DELETE /users?token
        /// </remarks>
        /// <response code = "204">Success</response>
        /// <response code = "404">If user not found</response>
        /// <response code = "401">If user token is wrong</response>
        /// <response code = "400">If request does not contain required parameter</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("")]
        [Authorize]
        public async Task<ActionResult> Delete([FromQuery] string token)
        {
            var jwt = new JwtSecurityToken(token);
            var claim = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
            if (claim == null) { return BadRequest(); }
            var passcode = claim.Value ?? string.Empty;

            var query = new DeleteCommand
            {
                Passcode = passcode ?? throw new InvalidRequestException(nameof(passcode))
            };
            await Mediator.Send(query);
            return NoContent();
        }
    }
}

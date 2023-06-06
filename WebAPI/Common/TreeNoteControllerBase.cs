using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Common
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TreeNotesControllerBase : ControllerBase
    {
        private IMediator _mediator;
        protected IMediator Mediator =>
            _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
    }
}

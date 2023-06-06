using FluentValidation;
using MediatR;

namespace Application.Common.Behaviors
{
    public class ValidatorBehavior<Request, Responce> :
        IPipelineBehavior<Request, Responce> where Request : IRequest<Responce>
    {
        private readonly IEnumerable<IValidator<Request>> _validators;
        public ValidatorBehavior(IEnumerable<IValidator<Request>> validators)
        {
            _validators = validators;
        }
        public Task<Responce> Handle(Request request, RequestHandlerDelegate<Responce> next, CancellationToken cancellationToken)
        {
            var validatorContext = new ValidationContext<Request>(request);
            var failures = _validators
                .Select(validation => validation.Validate(validatorContext))
                .SelectMany(result => result.Errors).Where(fail => fail != null).ToList();
            if (failures.Count > 0)
            {
                throw new ValidationException(failures);
            }
            return next();
        }
    }
}

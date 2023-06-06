namespace Application.Common.Exceptions
{
    public class InvalidRequestException:Exception
    {
        public InvalidRequestException(string name) :
            base($"Parameter {name} is required")
        { }
    }
}

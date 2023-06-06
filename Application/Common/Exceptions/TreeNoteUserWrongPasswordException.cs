namespace Application.Common.Exceptions
{
    public class TreeNoteUserWrongPasswordException : Exception
    {
        public TreeNoteUserWrongPasswordException(string name, object key) :
            base($"Entity {name} ({key}) password is wrong")
        { }
    }
}

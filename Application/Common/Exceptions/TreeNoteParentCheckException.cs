namespace Application.Common.Exceptions
{
    public class TreeNoteParentCheckException:Exception
    {
        public TreeNoteParentCheckException(string name, object key) :
            base($"Entity {name} ({key}) is block, it's parent checked") { }
    }
}

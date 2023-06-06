namespace Application.Common.Exceptions
{
    public class EntityDeniedPermissionException:Exception
    {
        public EntityDeniedPermissionException(string name, object key) :
            base($"Entity {name} ({key}) permission denied")
        { }
    }
}

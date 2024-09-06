namespace Code.Scripts.ControlObjectsSystem.EventArgs
{
    public class ControllableObjectsChangedEventArgs
    {
        public ControllableObjectsChangedEventArgs(
            ChangeType controllableObjectsChangeType,
            ControllableObject affectedObject
            )
        {
            ControllableObjectsChangeType = controllableObjectsChangeType;
            AffectedObjects = new ControllableObject[]{affectedObject};
        }
        
        public enum ChangeType
        {
            Added,
            Removed
        }

        public ChangeType ControllableObjectsChangeType { get; }

        public ControllableObject[] AffectedObjects { get; }
    }
}
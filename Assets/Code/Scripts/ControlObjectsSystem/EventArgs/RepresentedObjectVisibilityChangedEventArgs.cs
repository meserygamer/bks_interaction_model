namespace Code.Scripts.ControlObjectsSystem.EventArgs
{
    public class RepresentedObjectVisibilityChangedEventArgs
    {
        public RepresentedObjectVisibilityChangedEventArgs(bool newVisibilityValue)
        {
            NewVisibilityValue = newVisibilityValue;
        }
        
        public bool NewVisibilityValue { get; }
    }
}
namespace Code.Scripts.ControlObjectsSystem.EventArgs
{
    public class SelectionChangedEventArgs
    {
        public SelectionChangedEventArgs(bool newSelectionValue)
        {
            NewSelectionValue = newSelectionValue;
        }
        
        public bool NewSelectionValue { get; }
    }
}
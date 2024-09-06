using Code.Scripts.ControlObjectsSystem;
using Code.Scripts.ControlObjectsSystem.EventArgs;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class RepresentativeForControllableObjectUI : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Toggle _selectToggle;
        [SerializeField] private Toggle _representedObjectVisibilityToggle;
        [SerializeField] private TextMeshProUGUI _objectNameText;

        [CanBeNull] private ControllableObject _representedControllableObject = null;
        private string _representedName = string.Empty;

        #endregion

        #region Delegates

        public delegate void SelectionChangedEventHandler(object sender, SelectionChangedEventArgs e);
        public delegate void RepresentedObjectVisibilityChangedEventHandler(
            object sender,
            RepresentedObjectVisibilityChangedEventArgs e);

        #endregion

        #region Events

        public event SelectionChangedEventHandler SelectionChanged;
        public event RepresentedObjectVisibilityChangedEventHandler RepresentedObjectVisibilityChanged;

        #endregion

        #region MonoBehavior

        private void OnEnable()
        {
            _selectToggle.onValueChanged.AddListener(SelectToggleValueChangedHandler);
            _representedObjectVisibilityToggle.onValueChanged.AddListener(
                RepresentedObjectVisibilityToggleChangedHandler);
        }
        private void OnDisable()
        {
            _selectToggle.onValueChanged.RemoveListener(SelectToggleValueChangedHandler);
            _representedObjectVisibilityToggle.onValueChanged.RemoveListener(
                RepresentedObjectVisibilityToggleChangedHandler);
        }

        #endregion

        #region Properties

        public ControllableObject RepresentedControllableObject
        {
            get => _representedControllableObject;
            set
            {
                _representedControllableObject = value;
                RepresentedName = value.ObjectName;
            }
        }
        
        private string RepresentedName
        {
            get => _representedName;
            set
            {
                _representedName = value;
                _objectNameText.text = value;
            }
        }

        #endregion

        #region Methods

        public void SetRepresentedObjectVisibilityWithoutNotify(bool newValue)
            => _representedObjectVisibilityToggle.SetIsOnWithoutNotify(newValue);

        public void SetSelectionWithoutNotify(bool newValue)
            => _selectToggle.SetIsOnWithoutNotify(newValue);

        private void SelectToggleValueChangedHandler(bool newValue)
        {
            SelectionChangedEventArgs eventArgs = new SelectionChangedEventArgs(newValue);
            SelectionChanged?.Invoke(this, eventArgs);
        }
        private void RepresentedObjectVisibilityToggleChangedHandler(bool newValue)
        {
            RepresentedObjectVisibilityChangedEventArgs eventArgs =
                new RepresentedObjectVisibilityChangedEventArgs(newValue);
            RepresentedObjectVisibilityChanged?.Invoke(this, eventArgs);
        }

        #endregion
    }
}
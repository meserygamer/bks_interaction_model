using System;
using System.Collections.Generic;
using System.Linq;
using Code.Scripts.ControlObjectsSystem;
using Code.Scripts.ControlObjectsSystem.EventArgs;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts.UI
{
    public class ObjectsInteractorUI : MonoBehaviour
    {
        #region Fields
        
        [SerializeField] private ControllableObjectInteractor _interactor;
        [SerializeField] private GameObject _representativeForControllableObjectPrefab;
        [SerializeField] private Transform _spawnPointForPrefab;
        [SerializeField] private Toggle _controllableObjectsVisibilitySwitcher;
        [SerializeField] private Toggle _controllableObjectsAllSelectionSwitcher;
        [SerializeField] private TransparencyButton[] _transparencyButtons; 
        
        private readonly Dictionary<ControllableObject, RepresentativeForControllableObjectUI> _representatives 
            = new();

        private readonly HashSet<RepresentativeForControllableObjectUI> _selectedRepresentatives = new();
        
        private Dictionary<ControllableObjectsChangedEventArgs.ChangeType, Action<ControllableObject>> _controllableObjectsChangedHandlers;

        #endregion

        #region MonoBehaviour

        private void Awake()
        {
            _controllableObjectsChangedHandlers =
                new Dictionary<ControllableObjectsChangedEventArgs.ChangeType, Action<ControllableObject>>
                {
                    { ControllableObjectsChangedEventArgs.ChangeType.Added, AddControllableObjectRepresentative },
                    { ControllableObjectsChangedEventArgs.ChangeType.Removed, RemoveControllableObjectRepresentative }
                };
        }

        private void OnEnable()
        {
            _interactor.ControllableObjectsChanged += ControllableObjectsChangedHandler;
            UpdateControllableObjectsFromInteractor();
            
            _controllableObjectsVisibilitySwitcher.onValueChanged.AddListener(
                ControllableObjectsVisibilitySwitcherValueChangedHandler);
            
            _controllableObjectsAllSelectionSwitcher.onValueChanged.AddListener(
                ControllableObjectsAllSelectionSwitcherValueChangedHandler);

            foreach (var transparencyButton in _transparencyButtons) 
                transparencyButton.clicked.AddListener(TransparencyButtonClickedHandler);
        }

        private void OnDisable()
        {
            _interactor.ControllableObjectsChanged -= ControllableObjectsChangedHandler;
            
            _controllableObjectsVisibilitySwitcher.onValueChanged.RemoveListener(
                ControllableObjectsVisibilitySwitcherValueChangedHandler);
            
            _controllableObjectsAllSelectionSwitcher.onValueChanged.RemoveListener(
                ControllableObjectsAllSelectionSwitcherValueChangedHandler);
            
            foreach (var transparencyButton in _transparencyButtons) 
                transparencyButton.clicked.RemoveListener(TransparencyButtonClickedHandler);
        }

        #endregion

        #region Methods

        private void ControllableObjectsChangedHandler(object sender, ControllableObjectsChangedEventArgs e)
        {
            Action<ControllableObject> action;
            if (!_controllableObjectsChangedHandlers.TryGetValue(e.ControllableObjectsChangeType, out action))
                throw new NotSupportedException($"{e.ControllableObjectsChangeType} is not supported");

            foreach (var controllableObject in e.AffectedObjects) 
                action.Invoke(controllableObject);
        }

        private void ControllableObjectRepresentativeSelectionChangedHandler(object sender, SelectionChangedEventArgs e)
        {
            RepresentativeForControllableObjectUI representative = (RepresentativeForControllableObjectUI)sender;
            bool isNowSelect = e.NewSelectionValue;
            if (isNowSelect)
                _selectedRepresentatives.Add(representative);
            else
                _selectedRepresentatives.Remove(representative);
        }

        private void ControllableObjectRepresentativeVisibilityChangedHandler(
            object sender,
            RepresentedObjectVisibilityChangedEventArgs e)
        {
            RepresentativeForControllableObjectUI representative = (RepresentativeForControllableObjectUI)sender;
            ControllableObject controllableObject = representative.RepresentedControllableObject;
            _interactor.SetControllableObjectVisibility(controllableObject, e.NewVisibilityValue);
        }

        private void ControllableObjectsVisibilitySwitcherValueChangedHandler(bool newValue)
        {
            _interactor.SetAllControllableObjectsVisibility(newValue);
            
            List<RepresentativeForControllableObjectUI> fixedList = _representatives.Values.ToList();
            foreach (var representative in fixedList) 
                representative.SetRepresentedObjectVisibilityWithoutNotify(newValue);
        }

        private void ControllableObjectsAllSelectionSwitcherValueChangedHandler(bool newValue)
        {
            List<RepresentativeForControllableObjectUI> fixedList = _representatives.Values.ToList();
            foreach (var representative in fixedList)
            {
                if (newValue)
                    _selectedRepresentatives.Add(representative);
                else
                    _selectedRepresentatives.Remove(representative);
                
                representative.SetSelectionWithoutNotify(newValue);
            }
        }

        private void TransparencyButtonClickedHandler(float transparencyValue)
        {
            List<RepresentativeForControllableObjectUI> fixedList = _selectedRepresentatives.ToList();
            foreach (var representative in fixedList)
            {
                ControllableObject controllableObject = representative.RepresentedControllableObject;
                _interactor.SetControllableObjectTransparency(controllableObject, transparencyValue);
            }
        }

        private void UpdateControllableObjectsFromInteractor()
        {
            HashSet<ControllableObject> controllableObjectsFromInteracor 
                = new HashSet<ControllableObject>(_interactor.ControllableObjects);
            
            foreach (var controllableObject in _representatives.Keys)
                if(!controllableObjectsFromInteracor.Contains(controllableObject))
                    RemoveControllableObjectRepresentative(controllableObject);

            foreach (var controllableObject in controllableObjectsFromInteracor)
                if(!_representatives.ContainsKey(controllableObject))
                    AddControllableObjectRepresentative(controllableObject);
        }

        private void AddControllableObjectRepresentative(ControllableObject controllableObject)
        {
            GameObject representativeObject 
                = Instantiate(_representativeForControllableObjectPrefab, _spawnPointForPrefab).GameObject();
            RepresentativeForControllableObjectUI representativeComponent
                = representativeObject.GetComponent<RepresentativeForControllableObjectUI>();
            representativeComponent.RepresentedControllableObject = controllableObject;
            representativeComponent.SelectionChanged += ControllableObjectRepresentativeSelectionChangedHandler;
            representativeComponent.RepresentedObjectVisibilityChanged +=
                ControllableObjectRepresentativeVisibilityChangedHandler;

            if (!_representatives.TryAdd(controllableObject, representativeComponent))
                throw new SystemException("Failed to add controllableObject in representatives dictionary");
        }
        private void RemoveControllableObjectRepresentative(ControllableObject controllableObject)
        {
            RepresentativeForControllableObjectUI controllableObjectRepresentative;
            if (!_representatives.Remove(controllableObject, out controllableObjectRepresentative))
                throw new SystemException("controllableObject not in representatives dictionary");
            
            Destroy(controllableObjectRepresentative);
        }

        #endregion
    }
}

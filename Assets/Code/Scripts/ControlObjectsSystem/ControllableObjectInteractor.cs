using System;
using System.Collections.Generic;
using System.Linq;
using Code.Scripts.ControlObjectsSystem.EventArgs;
using UnityEngine;

namespace Code.Scripts.ControlObjectsSystem
{
    public class ControllableObjectInteractor : MonoBehaviour
    {
        private readonly HashSet<ControllableObject> _controllableObjects = new();

        public delegate void ControlableObjectsChangedEventHandler(object sender, ControllableObjectsChangedEventArgs e);
        public event ControlableObjectsChangedEventHandler ControllableObjectsChanged;

        public ControllableObject[] ControllableObjects
            => _controllableObjects.ToArray();

        public bool AddInInteractor(ControllableObject controllableObject)
        {
            bool isAdd = _controllableObjects.Add(controllableObject);
            if (isAdd)
            {
                ControllableObjectsChangedEventArgs eventArgs = new ControllableObjectsChangedEventArgs(
                    ControllableObjectsChangedEventArgs.ChangeType.Added,
                    controllableObject
                    );
                ControllableObjectsChanged?.Invoke(this, eventArgs);
            }
            
            return isAdd;
        }

        public bool RemovedFromInteractor(ControllableObject controllableObject)
        {
            bool isRemoved = _controllableObjects.Remove(controllableObject);;
            if (isRemoved)
            {
                ControllableObjectsChangedEventArgs eventArgs = new ControllableObjectsChangedEventArgs(
                    ControllableObjectsChangedEventArgs.ChangeType.Removed,
                    controllableObject
                );
                ControllableObjectsChanged?.Invoke(this, eventArgs);
            }

            return isRemoved;
        }

        public void SetControllableObjectVisibility(ControllableObject controllableObject, bool newValue)
        {
            if (!_controllableObjects.Contains(controllableObject))
                throw new ArgumentException("Controllable object is not under control by this interactor");
            
            controllableObject.SetVisibility(newValue);
        }

        public void SetAllControllableObjectsVisibility(bool newValue)
        {
            ControllableObject[] fixedList = _controllableObjects.ToArray();
            foreach (var controllableObject in fixedList)
                controllableObject.SetVisibility(newValue);
        }

        public void SetControllableObjectTransparency(ControllableObject controllableObject, float newTransparencyLevel)
            => controllableObject.SetTransparency(newTransparencyLevel);
    }
}
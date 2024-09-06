using System;
using UnityEngine;

namespace Code.Scripts.ControlObjectsSystem
{
    [RequireComponent(typeof(MeshRenderer))]
    public class ControllableObject : MonoBehaviour
    {
        [SerializeField] private ControllableObjectInteractor _interactor;
        [SerializeField] private string _objectName;

        private Renderer _renderComponent;

        #region Monobehavior

        private void Awake()
        {
            _renderComponent = GetComponent<Renderer>();
        }

        private void OnEnable()
        {
            _interactor.AddInInteractor(this);
        }

        private void OnDisable()
        {
            _interactor.RemovedFromInteractor(this);
        }

        #endregion
        
        public string ObjectName => _objectName;

        public void SetVisibility(bool newValue)
            => _renderComponent.enabled = newValue;

        public void SetTransparency(float newValue)
        {
            if (newValue < 0f && newValue > 1f)
                throw new ArgumentException("New transparency value must be between 0 and 1");
            
            Material[] newMaterials = _renderComponent.materials;
            foreach (var newMaterial in newMaterials)
            {
                Color newMaterialColor = newMaterial.color;
                newMaterialColor.a = newValue;
                newMaterial.color = newMaterialColor;
            }

            _renderComponent.materials = newMaterials;
        }
    }
}



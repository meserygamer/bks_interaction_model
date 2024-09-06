using System;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class ObjectInteractorUIPanel : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Toggle _minMaxPanelStateToggle;
        
        [SerializeField] private float _minStateTargetXMinAnchor = -0.3f;
        [SerializeField] private float _minStateTargetXMaxAnchor = 0f;
        
        [SerializeField] private float _maxStateTargetXMinAnchor = 0f;
        [SerializeField] private float _maxStateTargetXMaxAnchor = 0.3f;

        private RectTransform _rectTransform;

        #endregion

        #region MonoBehaviour

        private void Awake()
        {
            _rectTransform = gameObject.GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            _minMaxPanelStateToggle.onValueChanged.AddListener(MinMaxPanelStateToggleValueChangedHandler);
        }

        private void OnDisable()
        {
            _minMaxPanelStateToggle.onValueChanged.RemoveListener(MinMaxPanelStateToggleValueChangedHandler);
        }

        #endregion

        #region Methods

        private void MinMaxPanelStateToggleValueChangedHandler(bool newValue)
        {
            if(newValue)
                MaximizeUIPanel();
            else
                MinimizeUIPanel();
        }

        private void MaximizeUIPanel()
        {
            SetNewXAnchors(_maxStateTargetXMinAnchor, _maxStateTargetXMaxAnchor);
        }

        private void MinimizeUIPanel()
        {
            SetNewXAnchors(_minStateTargetXMinAnchor, _minStateTargetXMaxAnchor);
        }

        private void SetNewXAnchors(float anchorMinX, float anchorMaxX)
        {
            Vector2 anchorMin = _rectTransform.anchorMin;
            Vector2 anchorMax = _rectTransform.anchorMax;
            anchorMin.x = anchorMinX;
            anchorMax.x = anchorMaxX;
            _rectTransform.anchorMin = anchorMin;
            _rectTransform.anchorMax = anchorMax;
        }

        #endregion
    }
}
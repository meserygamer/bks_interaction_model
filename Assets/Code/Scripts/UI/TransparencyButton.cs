using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Code.Scripts.UI
{
    [RequireComponent(typeof(Button))]
    public class TransparencyButton : MonoBehaviour
    {
        public UnityEvent<float> clicked;
        
        [Range(0, 1)]
        [SerializeField] private float _transparencyValue;
        
        private Button _button;
        
        private void Awake()
        {
            _button = gameObject.GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(ButtonClickedHandler);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(ButtonClickedHandler);
        }

        private void ButtonClickedHandler()
            => clicked.Invoke(_transparencyValue);

    }
}
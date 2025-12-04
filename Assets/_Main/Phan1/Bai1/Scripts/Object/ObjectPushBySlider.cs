using _Main.Phan1.Bai1.Scripts.UI.WarningUI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Main.Phan1.Bai1.Scripts
{
    public class ObjectPushBySlider : MonoBehaviour
    {
        public Transform piston;
        public Vector3 localDirection = Vector3.forward;
        public float distance = 0.1f;
        public Slider slider;
        public float maxSpeed = 1f;

        public string warningText;
        
        public float reachPercent = .98f;
        public UnityEvent onReachValue;
        
        private float lastValue;
        private Vector3 startLocalPos;
        private float currentSpeed;

        void Start()
        {
            if (piston == null) piston = transform;
            startLocalPos = piston.localPosition;

            if (slider)
            {
                slider.onValueChanged.AddListener(OnSliderChanged);
                lastValue = slider.value;
            }
        }

        void OnSliderChanged(float value)
        {
            if (value / (slider.maxValue - slider.minValue) > reachPercent)
            {
                onReachValue?.Invoke();
                enabled = false;
                return;
            }
            
            float deltaValue = value - lastValue;
            currentSpeed = Mathf.Abs(deltaValue * distance / Time.deltaTime);

            if (currentSpeed > maxSpeed)
            {
                WarningUI.Instance?.Show(warningText);
                Debug.LogWarning($"Đẩy quá nhanh: {currentSpeed:F3} m/s (giới hạn {maxSpeed})");
            }

            piston.localPosition = startLocalPos + localDirection.normalized * (value * distance);
            lastValue = value;
        }
    }
}
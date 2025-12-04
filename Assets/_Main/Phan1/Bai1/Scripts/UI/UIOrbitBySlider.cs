using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Main.Phan1.Bai1.Scripts.UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;

    public class UIOrbitBySlider : MonoBehaviour
    {
        public RectTransform target;
        public RectTransform movingObj;
        public float radius = 100f;
        public Slider slider;
        public TMP_Text valueText;
        public float maxAngle = 180f;
        
        private void Start()
        {
            if (slider)
                slider.onValueChanged.AddListener(OnSliderChanged);

            OnSliderChanged(0);
        }

        void OnSliderChanged(float value)
        {
            if (target == null || movingObj == null) return;

            float angle = Mathf.Round(value * maxAngle * 2f) * 0.5f;

            float rad = angle * Mathf.Deg2Rad;
            Vector2 offset = new Vector2(-Mathf.Cos(rad), Mathf.Sin(rad)) * radius;

            if (valueText)
            {
                valueText.text = angle.ToString("F1");
            }

            movingObj.anchoredPosition = target.anchoredPosition + offset;
        }
    }
}
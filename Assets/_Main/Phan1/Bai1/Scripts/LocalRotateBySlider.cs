namespace _Main.Phan1.Bai1.Scripts
{
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;

    public class LocalRotateBySlider : MonoBehaviour
    {
        public enum Axis { X, Y, Z }

        public Transform target;
        public Slider slider;
        public Axis axis = Axis.Y;
        public float maxAngle = 180f;

        private void Start()
        {
            if (slider)
                slider.onValueChanged.AddListener(OnSliderChanged);
        }

        void OnSliderChanged(float value)
        {
            if (target == null) return;

            float angle = Mathf.Round(value * maxAngle * 2f) * 0.5f;

            Vector3 rot = target.localEulerAngles;
            switch (axis)
            {
                case Axis.X: rot.x = angle; break;
                case Axis.Y: rot.y = angle; break;
                case Axis.Z: rot.z = angle; break;
            }

            target.localEulerAngles = rot;
        }
    }

}
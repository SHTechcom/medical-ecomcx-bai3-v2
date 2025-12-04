using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Main.Common.UI
{
    public class SoundSettingGroupUI : MonoBehaviour
    {
        [Header("Sliders")] [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider effectSlider;
        [SerializeField] private Slider uiSlider;
        [SerializeField] private Slider voiceSlider;
        [SerializeField] private Slider ambientSlider;

        [Header("Percents")] [SerializeField] private TMP_Text musicPercent;
        [SerializeField] private TMP_Text effectPercent;
        [SerializeField] private TMP_Text uiPercent;
        [SerializeField] private TMP_Text voicePercent;
        [SerializeField] private TMP_Text ambientPercent;

        private void Awake()
        {
            musicSlider.onValueChanged.AddListener(OnMusicSliderChange);
            effectSlider.onValueChanged.AddListener(OnEffectSliderChange);
            uiSlider.onValueChanged.AddListener(OnUISliderChange);
            voiceSlider.onValueChanged.AddListener(OnVoiceSliderChange);
            ambientSlider.onValueChanged.AddListener(OnAmbientSliderChange);
        }

        private void Start()
        {
            SetSliderValue(musicSlider, SoundType.MUSIC, musicPercent);
            SetSliderValue(effectSlider, SoundType.EFFECT, effectPercent);
            SetSliderValue(uiSlider, SoundType.UI, uiPercent);
            SetSliderValue(voiceSlider, SoundType.VOICE, voicePercent);
            SetSliderValue(ambientSlider, SoundType.AMBIENT, ambientPercent);
        }

        private void SetSliderValue(Slider slider, SoundType type, TMP_Text percentText)
        {
            float value = SoundSettingData.Get(type);
            slider.value = Mathf.Clamp(value, slider.minValue, slider.maxValue);
            UpdatePercentText(slider, percentText, value);
        }

        private static void UpdatePercentText(Slider slider, TMP_Text text, float value)
        {
            if (text == null) return;
            float percent = Mathf.InverseLerp(slider.minValue, slider.maxValue, value);
            text.text = Mathf.RoundToInt(percent * 100f).ToString();
        }

        private void OnMusicSliderChange(float value)
        {
            SoundSettingData.Set(SoundType.MUSIC, value);
            SoundManager.Instance?.SetVolume(value, SoundType.MUSIC);
            UpdatePercentText(musicSlider, musicPercent, value);
        }

        private void OnEffectSliderChange(float value)
        {
            SoundSettingData.Set(SoundType.EFFECT, value);
            SoundManager.Instance?.SetVolume(value, SoundType.EFFECT);
            UpdatePercentText(effectSlider, effectPercent, value);
        }

        private void OnUISliderChange(float value)
        {
            SoundSettingData.Set(SoundType.UI, value);
            SoundManager.Instance?.SetVolume(value, SoundType.UI);
            UpdatePercentText(uiSlider, uiPercent, value);
        }

        private void OnVoiceSliderChange(float value)
        {
            SoundSettingData.Set(SoundType.VOICE, value);
            SoundManager.Instance?.SetVolume(value, SoundType.VOICE);
            UpdatePercentText(voiceSlider, voicePercent, value);
        }

        private void OnAmbientSliderChange(float value)
        {
            SoundSettingData.Set(SoundType.AMBIENT, value);
            SoundManager.Instance?.SetVolume(value, SoundType.AMBIENT);
            UpdatePercentText(ambientSlider, ambientPercent, value);
        }
    }
}
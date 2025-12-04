using System;
using UnityEngine;
using UnityEngine.UI;

public class UISetting : BaseView
{
    public Button closeButton;
    public Button soundButton;
    public Button resetSoundButton;
    public Button langButton;
    public Button voiceButton;
    public Slider volumeSlider;
    public Image voiceIcon;

    public Sprite[] langSprites;
    public Sprite[] soundSprites;
    public Sprite[] voiceSprites;

    private void Start()
    {
        closeButton.onClick.AddListener(() =>
        {
            Hide();
        });
    }

    public void SetIconSoundButton(bool isMute)
    {
        soundButton.image.sprite = soundSprites[isMute ? 1 : 0];
    }

    public void OnClickedSound(Action action)
    {
        soundButton.onClick.RemoveAllListeners();
        soundButton.onClick.AddListener(() => { action?.Invoke(); });
    }

    public void OnClickedVoice(Action action)
    {
        voiceButton.onClick.RemoveAllListeners();
        voiceButton.onClick.AddListener(() => { action?.Invoke(); });
    }

    public void OnClickedResetSound(Action action)
    {
        resetSoundButton.onClick.RemoveAllListeners();
        resetSoundButton.onClick.AddListener(() => { action?.Invoke(); });
    }

    public void OnClickedLang(Action action)
    {
        langButton.onClick.RemoveAllListeners();
        langButton.onClick.AddListener(() => { action?.Invoke(); });
    }

    public void SetIconSound(Sprite icon)
    {
        soundButton.image.sprite = icon;
    }

    public void SetIconSound(bool isMute)
    {
        SetIconSound(soundSprites[isMute ? 1 : 0]);
    }

    public void SetIconLang(Sprite icon)
    {
        langButton.image.sprite = icon;
    }

    public void SetIconLang(int id)
    {
        SetIconLang(langSprites[id]);
    }

    public void SetIconVoice(bool isMale)
    {
        voiceIcon.sprite = voiceSprites[isMale ? 0 : 1];
    }

    public void OnChangeVolume(Action<float> action)
    {
        volumeSlider.onValueChanged.RemoveAllListeners();
        volumeSlider.onValueChanged.AddListener((value) => action?.Invoke(value));
    }
}

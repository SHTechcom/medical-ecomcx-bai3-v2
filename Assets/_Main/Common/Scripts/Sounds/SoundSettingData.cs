using UnityEngine;
using System;

public static class SoundSettingData
{
    private const string Prefix = "SOUND_SETTING_";

    static SoundSettingData()
    {
        LoadAll();
    }

    private static void LoadAll()
    {
        foreach (SoundType type in Enum.GetValues(typeof(SoundType)))
        {
            if (!PlayerPrefs.HasKey(Prefix + type))
                PlayerPrefs.SetFloat(Prefix + type, 1f);
        }

        PlayerPrefs.Save();
    }

    public static void Set(SoundType type, float value)
    {
        value = Mathf.Clamp01(value);
        PlayerPrefs.SetFloat(Prefix + type, value);
        PlayerPrefs.Save();
    }

    public static float Get(SoundType type)
    {
        if (!PlayerPrefs.HasKey(Prefix + type)) return 1f;
        return PlayerPrefs.GetFloat(Prefix + type);
    }

    public static float Music => Get(SoundType.MUSIC);
    public static float Effect => Get(SoundType.EFFECT);
    public static float UI => Get(SoundType.UI);
    public static float Voice => Get(SoundType.VOICE);
    public static float Ambient => Get(SoundType.AMBIENT);
}
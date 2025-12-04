using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundData", menuName = "Project/Sound/SoundData")]
public class SoundData : ScriptableObject
{
    [field: ListDrawerSettings(ListElementLabelName = "key")]
    public List<SoundEntry> entries;

    private Dictionary<string, SoundEntry> _soundMap;

    public void Init()
    {
        _soundMap = new Dictionary<string, SoundEntry>();
        foreach (var entry in entries) _soundMap[entry.key] = entry;
    }

    public SoundEntry? Get(string key)
    {
        if (_soundMap == null) Init();
        if (string.IsNullOrEmpty(key)) return null;
        return _soundMap.TryGetValue(key, out var entry) ? entry : null;
    }
}

[System.Serializable]
public struct SoundEntry
{
    public string key;
    public SoundType type;
    public AudioClip clip;
}

public enum AudioPlayType
{
    OneShot,
    Loop,
}

public enum SoundType
{
    MUSIC,
    EFFECT,
    AMBIENT,
    VOICE,
    UI
}
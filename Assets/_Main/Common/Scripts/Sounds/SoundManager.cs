using System;
using System.Collections.Generic;
using Frank;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class SoundManager : SingletonPersistent<SoundManager>
{
    [SerializeField] private SoundData soundData;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private AudioMixerGroup mixerMusicGroup;
    [SerializeField] private AudioMixerGroup mixerFXGroup;
    [SerializeField] private AudioMixerGroup mixerUIGroup;
    [SerializeField] private AudioMixerGroup mixerVoiceGroup;
    [SerializeField] private AudioMixerGroup mixerAmbientGroup;

    private readonly Dictionary<SoundType, List<AudioSource>> _sourcePools = new();

    private void Start()
    {
        soundData?.Init();
        SetVolume(SoundSettingData.Get(SoundType.MUSIC), SoundType.MUSIC);
        SetVolume(SoundSettingData.Get(SoundType.EFFECT), SoundType.EFFECT);
        SetVolume(SoundSettingData.Get(SoundType.UI), SoundType.UI);
        SetVolume(SoundSettingData.Get(SoundType.VOICE), SoundType.VOICE);
        SetVolume(SoundSettingData.Get(SoundType.AMBIENT), SoundType.AMBIENT);
    }

    public void SetVolume(float value, SoundType type)
    {
        switch (type)
        {
            case SoundType.MUSIC:
                mixer.SetFloat("MusicVolume", Mathf.Lerp(-80f, 0f, value));
                break;
            case SoundType.EFFECT:
                mixer.SetFloat("EffectVolume", Mathf.Lerp(-80f, 0f, value));
                break;
            case SoundType.AMBIENT:
                mixer.SetFloat("AmbientVolume", Mathf.Lerp(-80f, 0f, value));
                break;
            case SoundType.VOICE:
                mixer.SetFloat("VoiceVolume", Mathf.Lerp(-80f, 0f, value));
                break;
            case SoundType.UI:
                mixer.SetFloat("UIVolume", Mathf.Lerp(-80f, 0f, value));
                break;
        }
    }

    public void Play(string key, AudioPlayType playType = AudioPlayType.OneShot)
    {
        var entry = soundData.Get(key);
        if (entry == null) return;

        var e = entry.Value;
        var source = GetSource(e.type, GetMixerGroup(e.type));
        switch (playType)
        {
            case AudioPlayType.Loop:
                source.loop = true;
                source.clip = e.clip;
                source.Play();
                break;
            case AudioPlayType.OneShot:
                source.loop = false;
                source.PlayOneShot(e.clip);
                break;
        }
    }

    public void Play(AudioClip fx, AudioPlayType audioPlayType = AudioPlayType.OneShot, SoundType type = SoundType.EFFECT)
    {
        var source = GetSource(type, GetMixerGroup(type));
        switch (audioPlayType)
        {
            case AudioPlayType.Loop:
                source.loop = true;
                source.clip = fx;
                source.Play();
                break;
            case AudioPlayType.OneShot:
                source.loop = false;
                source.PlayOneShot(fx);
                break;
        }
    }

    public void PlayOneShot(AudioClip clip, SoundType type = SoundType.EFFECT)
    {
        if (clip == null) return;
        var source = GetSource(type, GetMixerGroup(type));
        source.loop = false;
        source.PlayOneShot(clip);
    }

    public void Stop(string key)
    {
        var entry = soundData.Get(key);
        if (entry == null) return;

        var e = entry.Value;
        var source = GetSourceContains(e.clip);
        if (source == null) return;

        source.Stop();
        source.loop = false;
        source.clip = null;
    }

    public void Stop(AudioClip clip)
    {
        var source = GetSourceContains(clip);
        if (source == null) return;

        source.Stop();
        source.loop = false;
        source.clip = null;
    }

    #region QUICK MUSIC METHOD

    public void PlayMusic(string key, AudioPlayType audioPlayType)
    {
        var entry = soundData.Get(key);
        if (entry == null) return;

        var e = entry.Value;
        if (musicSource.clip == e.clip && musicSource.isPlaying) return;

        musicSource.outputAudioMixerGroup = mixerMusicGroup;
        musicSource.loop = audioPlayType == AudioPlayType.Loop;
        musicSource.clip = e.clip;
        musicSource.Play();
    }

    public void PlayMusic(AudioClip music, AudioPlayType audioPlayType)
    {
        if (music == null) return;
        if (musicSource.clip == music && musicSource.isPlaying) return;

        musicSource.outputAudioMixerGroup = mixerMusicGroup;
        musicSource.loop = audioPlayType == AudioPlayType.Loop;
        musicSource.clip = music;
        musicSource.Play();
    }

    public void PlayRandomMusic(AudioPlayType audioPlayType = AudioPlayType.Loop)
    {
        var musics = soundData.entries.FindAll(e => e.type == SoundType.MUSIC);
        if (musics.Count == 0) return;

        var randomIndex = Random.Range(0, musics.Count);
        var music = musics[randomIndex].clip;

        PlayMusic(music, audioPlayType);
    }

    public void StopMusic()
    {
        musicSource?.Stop();
    }

    public void PauseMusic()
    {
        musicSource?.Pause();
    }

    public void ResumeMusic()
    {
        if (musicSource.clip == null)
        {
            PlayRandomMusic();
        }
        else if (!musicSource.isPlaying)
        {
            musicSource.Play();
        }
        else
        {
            musicSource.UnPause();
        }
    }

    #endregion

    private AudioSource GetSource(SoundType type, AudioMixerGroup group)
    {
        if (!_sourcePools.ContainsKey(type)) _sourcePools[type] = new List<AudioSource>();

        foreach (var src in _sourcePools[type])
        {
            if (!src.isPlaying) return src;
        }

        var newSrc = gameObject.AddComponent<AudioSource>();
        newSrc.outputAudioMixerGroup = group;
        _sourcePools[type].Add(newSrc);
        return newSrc;
    }

    private AudioSource GetSourceContains(AudioClip clip)
    {
        foreach (var (_, group) in _sourcePools)
        {
            foreach (var src in group)
            {
                if (src.clip == clip) return src;
            }
        }

        return null;
    }

    private AudioMixerGroup GetMixerGroup(SoundType type)
    {
        return type switch
        {
            SoundType.MUSIC => mixerMusicGroup,
            SoundType.EFFECT => mixerFXGroup,
            SoundType.UI => mixerUIGroup,
            SoundType.VOICE => mixerVoiceGroup,
            SoundType.AMBIENT => mixerAmbientGroup,
            _ => null
        };
    }
}
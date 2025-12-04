using Sirenix.OdinInspector;
using UnityEngine;

namespace _Main.Common.Scripts.Sounds
{
    public class SoundExample : MonoBehaviour
    {
        [Button]
        public void Play(string key, AudioPlayType audioPlayType = AudioPlayType.OneShot)
        {
            SoundManager.Instance?.Play(key, audioPlayType);
        }

        [Button]
        public void Play(AudioClip clip, AudioPlayType audioPlayType, SoundType type)
        {
            SoundManager.Instance?.Play(clip, audioPlayType, type);
        }
        
        [Button]
        public void PlayOneShot(AudioClip clip, SoundType type)
        {
            SoundManager.Instance?.PlayOneShot(clip, type);
        }

        [Button]
        public void PlayMusic(AudioClip clip, AudioPlayType audioPlayType)
        {
            SoundManager.Instance?.PlayMusic(clip, audioPlayType);
        }
        
        [Button]
        public void PlayRandomMusic()
        {
            SoundManager.Instance?.PlayRandomMusic();
        }

        [Button]
        public void PauseMusic()
        {
            SoundManager.Instance?.PauseMusic();
        }

        [Button]
        public void ResumeMusic()
        {
            SoundManager.Instance?.ResumeMusic();
        }
        
        [Button]
        public void StopMusic()
        {
            SoundManager.Instance?.StopMusic();
        }
    }
}
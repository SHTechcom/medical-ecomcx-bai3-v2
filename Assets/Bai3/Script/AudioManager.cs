using UnityEngine;
using System.Collections.Generic;
using Frank;

namespace UDA.Audio
{
    public class AudioManager : Singleton<AudioManager>
    {
        public List<AudioSource> audioSources = new List<AudioSource>();
        float vollum;
        private bool isMute;
        private UISetting UISetting => GameViewManager.Instance.GetView<UISetting>();

        [ContextMenu("Refresh Audio Source List")]
        public void RefreshList()
        {
            audioSources.Clear();

            AudioSource[] foundSources = FindObjectsOfType<AudioSource>(true);

            audioSources.AddRange(foundSources);
        }

        private void Start()
        {
            //RefreshList();
            vollum = 0.5f;

            UISetting.volumeSlider.value = vollum;
            UISetting.OnChangeVolume(SetVolumeAll);
            UISetting.OnClickedSound(() =>
            {
                isMute = !isMute;
                TurnSound(isMute);
                UISetting.SetIconSound(isMute);
            });
            UISetting.OnClickedResetSound(OnReset);
        }

        public void SetVolumeAll(float volume)
        {
            volume = Mathf.Clamp01(volume);

            foreach (var src in audioSources)
            {
                if (src != null)
                    src.volume = volume;
            }
        }

        public void TurnSound(bool isMute)
        {
            foreach (var src in audioSources)
            {
                if (src != null)
                    src.mute = isMute;
            }
        }

        public void OnReset()
        {
            isMute = false;
            vollum = 0.5f;
            SetVolumeAll(vollum);
            TurnSound(isMute);
            UISetting.volumeSlider.value = vollum;
            UISetting.SetIconSound(isMute);
        }
    }
}

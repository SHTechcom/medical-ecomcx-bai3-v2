using UnityEngine;

public class VoiceItem : MonoBehaviour
{
    private AudioSource audioSource;

    public AudioSource AudioSource
    {
        get
        {
            if(audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
            }
            return audioSource;
        }
    }

    public AudioClip maleClip;
    public AudioClip femaleClip;

    private void OnEnable()
    {
        AudioSource.Play();
    }

    public void Change(bool isMale)
    {
        AudioSource.clip = isMale ? maleClip : femaleClip;
    }
}

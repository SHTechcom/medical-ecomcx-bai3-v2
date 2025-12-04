using UnityEngine;

namespace _Main.Phan1.Bai1.Scripts
{
    public class AnimationCaller : MonoBehaviour
    {
        public Animation anim;
        private float speed;
        private AnimationClip clip;
        public bool isPaused = false;
        private float savedSpeed = 1f; // lưu speed để resume

        public void PlayAnimation(AnimationClip animationClip)
        {
            clip = animationClip;
            if (anim != null && !string.IsNullOrEmpty(animationClip.name))
            {
                anim.Play(animationClip.name);
            }
        }

        public void Play()
        {
            if (anim == null || clip == null) return;

            if (!isPaused)
            {
                savedSpeed = anim[clip.name].speed;
                anim[clip.name].speed = 0f;
                isPaused = true;
            }
            else
            {
                anim[clip.name].speed = savedSpeed;
                isPaused = false;
            }
        }

        public void Pause()
        {
            if (anim == null || clip == null) return;

            if (!isPaused)
            {
                savedSpeed = anim[clip.name].speed;
                anim[clip.name].speed = 0f;
                isPaused = true;
            }
        }

        public void Resume()
        {
            if (anim == null || clip == null) return;

            if (isPaused)
            {
                anim[clip.name].speed = savedSpeed;
                isPaused = false;
            }
        }

        public void IncreaseSpeed()
        {
            speed += 0.1f;
            if (speed > 5f) speed = 5f;

            if (anim.isPlaying)
                anim[clip.name].speed = speed;
        }

        public void DecreaseSpeed()
        {
            speed -= 0.1f;
            if (speed < 0.1f) speed = 0.1f;

            if (anim.isPlaying)
                anim[clip.name].speed = speed;
        }
    }
}
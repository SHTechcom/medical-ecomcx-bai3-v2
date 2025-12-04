using System;
using UnityEngine;
using System.Collections;

namespace _Main.Phan1.Bai1.Scripts
{
    public class OutlineTween : MonoBehaviour
    {
        public Outline outline;
        public float blinkSpeed = 2f;
        public float fadeSpeed = 3f;
        public bool hideOnStart;
        
        Coroutine blinkRoutine;
        Coroutine fadeRoutine;

        private void Start()
        {
            if (hideOnStart) outline.enabled = false;
        }

        private void Reset()
        {
            outline = GetComponent<Outline>();
        }

        public void Blink()
        {
            outline.enabled = true;
            StopBlink();
            blinkRoutine = StartCoroutine(BlinkRoutine());
        }

        public void BlinkLimited(int count)
        {
            StopBlink();
            blinkRoutine = StartCoroutine(BlinkLimitedRoutine(count));
        }

        public void FadeToColor(Color color)
        {
            outline.enabled = true;
            if (fadeRoutine != null) StopCoroutine(fadeRoutine);
            fadeRoutine = StartCoroutine(FadeColorRoutine(color));
        }

        IEnumerator BlinkRoutine()
        {
            float t = 0f;
            Color baseColor = outline.OutlineColor;
            while (true)
            {
                t += Time.deltaTime * blinkSpeed;
                float alpha = Mathf.PingPong(t, 1f);
                outline.OutlineWidth = Mathf.Lerp(0f, 8f, alpha);
                outline.OutlineColor = Color.Lerp(baseColor * 0.5f, baseColor, alpha);
                yield return null;
            }
        }

        IEnumerator BlinkLimitedRoutine(int count)
        {
            float t = 0f;
            Color baseColor = outline.OutlineColor;
            int done = 0;
            while (done < count)
            {
                while (t < 1f)
                {
                    t += Time.deltaTime * blinkSpeed;
                    float alpha = Mathf.Sin(t * Mathf.PI);
                    outline.OutlineWidth = Mathf.Lerp(0f, 8f, alpha);
                    outline.OutlineColor = Color.Lerp(baseColor * 0.5f, baseColor, alpha);
                    yield return null;
                }

                t = 0f;
                done++;
            }

            outline.OutlineWidth = 2f;
            outline.OutlineColor = baseColor;
            blinkRoutine = null;
        }

        IEnumerator FadeColorRoutine(Color targetColor)
        {
            Color start = outline.OutlineColor;
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime * fadeSpeed;
                outline.OutlineColor = Color.Lerp(start, targetColor, t);
                yield return null;
            }

            outline.OutlineColor = targetColor;
        }

        public void StopBlink()
        {
            if (blinkRoutine != null)
            {
                StopCoroutine(blinkRoutine);
                blinkRoutine = null;
                outline.OutlineWidth = 2f;
            }
        }
    }
}
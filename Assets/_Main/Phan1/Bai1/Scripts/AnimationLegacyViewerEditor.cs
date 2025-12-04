using Sirenix.OdinInspector;
using UnityEngine;

namespace _Main.Phan1.Bai1.Scripts
{
    [ExecuteInEditMode]
    public class AnimationLegacyViewerEditor : MonoBehaviour
    {
        public Animation animationComp;
        public AnimationClip clip;
        [Range(0, 1)] public float time;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (animationComp == null || clip == null) return;

            if (!animationComp.GetClip(clip.name)) return;

            clip.SampleAnimation(gameObject, time * clip.length);
            UnityEditor.SceneView.RepaintAll();
        }

        [Button]
        public void Remove()
        {
            UnityEditor.PrefabUtility.RevertObjectOverride(gameObject, UnityEditor.InteractionMode.AutomatedAction);
            DestroyImmediate(this);
        }
#endif
    }
}
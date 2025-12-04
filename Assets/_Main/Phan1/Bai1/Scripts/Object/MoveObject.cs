using System;
using DG.Tweening;
using UnityEngine;

namespace _Main.Phan1.Bai1.Scripts
{
    public class MoveObject : MonoBehaviour
    {
        [SerializeField] private float moveDuration = 1f;
        [SerializeField] private Ease moveEase = Ease.OutQuad;

        private Transform _firstParent;
        private Vector3 _startPos;
        private Tween _moveTween;

        private void Awake()
        {
            _startPos = transform.position;
            _firstParent = transform.parent;
        }

        private void OnDisable()
        {
            _moveTween?.Kill();
        }

        public void MoveTo(Transform to)
        {
            _moveTween?.Kill();
            transform.DOMove(to.position, moveDuration).SetEase(moveEase);
        }

        public void MoveToAndToChild(Transform to)
        {
            _moveTween?.Kill();
            transform.SetParent(to, true);
            transform.DOLocalMove(Vector3.zero, moveDuration).SetEase(moveEase);
        }

        public void ResetStartPos()
        {
            _startPos = transform.position;
            _firstParent = transform.parent;
        }

        public void ReturnFirstPos()
        {
            _moveTween?.Kill();
            if (_firstParent != null) transform.parent.SetParent(_firstParent, true);
            transform.DOMove(_startPos, moveDuration).SetEase(Ease.Linear);
        }
    }
}
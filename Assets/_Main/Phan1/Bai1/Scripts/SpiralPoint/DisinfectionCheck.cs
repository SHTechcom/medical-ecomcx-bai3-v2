using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

namespace _Main.Phan1.Bai1.Scripts.SpiralPoint
{
    public class DisinfectionCheck : MonoBehaviour
    {
        public Camera cam;
        public List<DisinfectionPoint> points;
        public UnityEvent onComplete;

        private int currentIndex = 0;
        private bool dragging = false;

        private void Start()
        {
            if (points.Count == 0)
            {
                gameObject.SetActive(false);
                return;
            }

            ResetAllPoints();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                dragging = true;
                ResetAllPoints();
                ResetProgress();
            }

            if (Input.GetMouseButtonUp(0))
            {
                dragging = false;
                ResetAllPoints();
                ResetProgress();
            }

            if (!dragging || points.Count == 0) return;

            if (Input.GetMouseButton(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    var currentPoint = points[currentIndex];
                    if (hit.collider == currentPoint.col)
                    {
                        currentPoint.Check();
                        currentPoint.SetHighlight(true);
                        currentIndex++;

                        if (currentIndex >= points.Count)
                        {
                            onComplete?.Invoke();
                            ResetAllPoints();
                            gameObject.SetActive(false);
                        }
                        else
                        {
                            points[currentIndex].SetHighlight(true);
                        }
                    }
                    else
                    {
                        //TODO: Warning
                    }
                }
            }
        }

        private void ResetAllPoints()
        {
            currentIndex = 0;
            foreach (var p in points) p.ResetPoint();
            points[0].SetHighlight(true);
        }

        private void ResetProgress()
        {
            currentIndex = 0;
        }

#if UNITY_EDITOR
        [Button]
        public void GetPoints()
        {
            points = GetComponentsInChildren<DisinfectionPoint>().ToList();
        }

        private void OnDrawGizmos()
        {
            if (points == null || points.Count == 0) return;

            // for (int i = 0; i < points.Count; i++)
            // {
            //     Gizmos.color = i < currentIndex ? Color.green : Color.red;
            //     if (points[i].col != null)
            //         Gizmos.DrawSphere(points[i].col.transform.position, points[i].col.bounds.extents.magnitude);
            //     else
            //         Gizmos.DrawSphere(points[i].transform.position, 0.005f);
            // }
        }
#endif
    }
}
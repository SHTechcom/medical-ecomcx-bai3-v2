using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Main.Common.Scripts.Avatar
{
    public class ChooseEquipmentDragUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private static readonly int ColorPropertyID = Shader.PropertyToID("_Color");
        private static readonly int BaseColorPropertyID = Shader.PropertyToID("_BaseColor");

        [SerializeField] private Camera viewerCam;
        [SerializeField] private RectTransform rawImageRect;
        [SerializeField] private Canvas canvas;
        [SerializeField] private Collider trayCollider;
        [SerializeField] private Collider avatarCollider;
        [SerializeField] private Transform returnTransform;

        [SerializeField] private LayerMask dragLayer = -1;
        [SerializeField] private Color validDropColor = Color.green;
        [SerializeField] private Color invalidDropColor = Color.red;

        private GameObject _dragObj;
        private GameObject _dragPreviewObj;

        public void SetDropArea(Collider col)
        {
            _dropAreaCollider = col;
        }
        private Collider _previewCollider;
        private Collider _dropAreaCollider;

        private Renderer[] _cachedRenderers;
        private Material[] _cachedMaterials;
        private MaterialPropertyBlock _mpb;

        private readonly Collider[] _overlapResults = new Collider[4];
        public bool IsDragging { get; private set; }

        private void Start()
        {
            _mpb = new MaterialPropertyBlock();
        }

        private void OnDisable()
        {
            if (IsDragging) Clean();
        }

        public void OnBeginDrag(PointerEventData e)
        {
            if (!HitInViewer(e.position, out var hit)) return;
            if (hit.collider == _dropAreaCollider) return;

            _dragObj = hit.collider.gameObject;

            CreateDragPreview();

            IsDragging = true;
        }

        public void OnDrag(PointerEventData e)
        {
            if (!IsDragging || _dragObj == null) return;

            if (!GetViewerRayFromScreen(e.position, out var ray)) return;

            Vector3 targetPos = GetRayPlaneIntersection(ray, Vector3.zero, Vector3.forward);

            if (_dragPreviewObj != null)
            {
                _dragPreviewObj.transform.position = targetPos;
                CheckValidDrop();
            }
            else
            {
                _dragObj.transform.position = targetPos;
            }
        }

        public void OnEndDrag(PointerEventData e)
        {
            if (!IsDragging) return;

            bool rayHit = GetViewerRayFromScreen(e.position, out var ray);

            if (_dragObj != null)
            {
                if (rayHit)
                {
                    Vector3 targetPos = GetRayPlaneIntersection(ray, Vector3.zero, Vector3.forward);
                    if (_dragPreviewObj != null) _dragPreviewObj.transform.position = targetPos;

                    if (CanDrop(_previewCollider, _dropAreaCollider))
                    {
                        OnSuccessfulDrop(_dragObj, targetPos);
                    }
                    else
                    {
                        OnFailedDrop(_dragObj);
                    }
                }
            }

            Clean();
        }

        public void SetupList(EquipmentType type)
        {
            Clean();
            IsDragging = false;
            gameObject.SetActive(true);

            switch (type)
            {
                case EquipmentType.Cloth:
                    //avatarCollider.gameObject.SetActive(true);
                    trayCollider.gameObject.SetActive(false);
                    break;
                case EquipmentType.ToolAndMedicine:
                    _dropAreaCollider = trayCollider;
                    avatarCollider.gameObject.SetActive(false);
                    trayCollider.gameObject.SetActive(true);
                    break;
            }
        }

        protected virtual void OnSuccessfulDrop(GameObject dragObj, Vector3 targetPos)
        {
            var eq = GetEquipment(dragObj);
            if (eq == null) return;

            dragObj.transform.position = targetPos;

            AvatarEquipmentControl.Instance.Equip(eq);
        }

        protected virtual void OnFailedDrop(GameObject dragObj)
        {
            var eq = GetEquipment(dragObj);
            if (eq == null) return;

            if (AvatarEquipmentControl.Instance.CurrentObjectEquipment == null)
            {
                AvatarEquipmentControl.Instance.UnEquip(eq);
                dragObj.transform.position = returnTransform.position;
            }
            else if (AvatarEquipmentControl.Instance.CurrentObjectEquipment == eq)
            {
                AvatarEquipmentControl.Instance.UnEquip(eq);
                dragObj.transform.position = returnTransform.position;
            }
            else if (AvatarEquipmentControl.Instance.CurrentObjectEquipment != eq)
            {
                AvatarEquipmentControl.Instance.DropOut(eq);
            }
        }

        private void CreateDragPreview()
        {
            _dragPreviewObj = Instantiate(_dragObj);
            _dragPreviewObj.transform.position = _dragObj.transform.position;
            _dragPreviewObj.name = _dragObj.name + "_Preview";
            _dragPreviewObj.layer = LayerMask.NameToLayer("Ignore Raycast");

            _previewCollider = _dragPreviewObj.GetComponent<Collider>();

            _cachedRenderers = _dragPreviewObj.GetComponentsInChildren<Renderer>(true);

            int matCount = 0;
            foreach (var rend in _cachedRenderers)
            {
                matCount += rend.sharedMaterials.Length;
            }

            if (_cachedMaterials == null || _cachedMaterials.Length < matCount)
            {
                _cachedMaterials = new Material[matCount];
            }

            int idx = 0;
            foreach (var rend in _cachedRenderers)
            {
                var mats = rend.sharedMaterials;
                foreach (var mat in mats)
                {
                    _cachedMaterials[idx] = mat;
                    idx++;
                }
            }

            _dragObj.SetActive(false);
        }

        private void CheckValidDrop()
        {
            if (_previewCollider == null || _dropAreaCollider == null) return;

            if (_dragPreviewObj != null && _cachedRenderers != null)
            {
                Color targetColor = CanDrop(_previewCollider, _dropAreaCollider) ? validDropColor : invalidDropColor;
                targetColor.a = 0.5f;

                foreach (var rend in _cachedRenderers)
                {
                    if (rend == null) continue;

                    rend.GetPropertyBlock(_mpb);

                    if (rend.sharedMaterial.HasProperty(BaseColorPropertyID))
                    {
                        _mpb.SetColor(BaseColorPropertyID, targetColor);
                    }
                    else if (rend.sharedMaterial.HasProperty(ColorPropertyID))
                    {
                        _mpb.SetColor(ColorPropertyID, targetColor);
                    }

                    rend.SetPropertyBlock(_mpb);
                }
            }
        }

        public void SetCollider(Collider col)
        {
            _dropAreaCollider = col;
        }

        private bool CanDrop(Collider obj, Collider tray)
        {
            // if (!IsInsideXY(obj, tray)) return false;
            //
            // int hitCount = Physics.OverlapBoxNonAlloc(
            //     obj.bounds.center,
            //     obj.bounds.extents,
            //     _overlapResults,
            //     obj.transform.rotation,
            //     dragLayer
            // );
            //
            // for (int i = 0; i < hitCount; i++)
            // {
            //     var hit = _overlapResults[i];
            //     if (hit != obj && hit != tray)
            //     {
            //         if (IsOverlapXY(obj, hit)) return false;
            //     }
            // }
            //
            // return true;
            Vector3 dir;
            float dist;

            return Physics.ComputePenetration(
                obj, obj.transform.position, obj.transform.rotation,
                tray, tray.transform.position, tray.transform.rotation,
                out dir, out dist
            );
        }

        private void Clean()
        {
            if (_dragPreviewObj != null && _cachedRenderers != null)
            {
                foreach (var rend in _cachedRenderers)
                {
                    if (rend == null) continue;
                    rend.SetPropertyBlock(null);
                }
            }

            if (_dragPreviewObj != null) Destroy(_dragPreviewObj);
            _dragPreviewObj = null;
            _previewCollider = null;

            if (_dragObj != null) _dragObj.SetActive(true);

            _dragObj = null;
            IsDragging = false;
            _cachedRenderers = null;
        }

        private bool HitInViewer(Vector2 screenPos, out RaycastHit hit)
        {
            hit = default;

            if (!RectTransformUtility.RectangleContainsScreenPoint(rawImageRect, screenPos, canvas.worldCamera))
                return false;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rawImageRect, screenPos, canvas.worldCamera, out var local);

            Vector2 size = rawImageRect.rect.size;

            var uv = new Vector2(
                (local.x + size.x * 0.5f) / size.x,
                (local.y + size.y * 0.5f) / size.y
            );

            var ray = viewerCam.ViewportPointToRay(new Vector3(uv.x, uv.y, 0));
            return Physics.Raycast(ray, out hit, Mathf.Infinity, dragLayer);
        }

        private bool GetViewerRayFromScreen(Vector2 screenPos, out Ray ray)
        {
            ray = default;

            if (!RectTransformUtility.RectangleContainsScreenPoint(rawImageRect, screenPos, canvas.worldCamera))
                return false;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rawImageRect, screenPos, canvas.worldCamera, out var local);

            Vector2 size = rawImageRect.rect.size;

            var uv = new Vector2(
                (local.x + size.x * 0.5f) / size.x,
                (local.y + size.y * 0.5f) / size.y
            );

            ray = viewerCam.ViewportPointToRay(new Vector3(uv.x, uv.y, 0));
            return true;
        }

        private AvatarEquipment GetEquipment(GameObject obj)
        {
            var d = obj.GetComponent<ChooseEquipmentObjectDrag>();
            return d != null ? d.Equipment : null;
        }

        private static bool IsInsideXY(Collider a, Collider b)
        {
            Bounds aB = a.bounds;
            Bounds bB = b.bounds;

            bool insideX = aB.min.x >= bB.min.x && aB.max.x <= bB.max.x;
            bool insideY = aB.min.y >= bB.min.y && aB.max.y <= bB.max.y;

            return insideX && insideY;
        }

        private static bool IsOverlapXY(Collider a, Collider b)
        {
            Bounds aB = a.bounds;
            Bounds bB = b.bounds;

            bool overlapX = aB.min.x < bB.max.x && aB.max.x > bB.min.x;
            bool overlapY = aB.min.y < bB.max.y && aB.max.y > bB.min.y;

            return overlapX && overlapY;
        }

        private static Vector3 GetRayPlaneIntersection(Ray ray, Vector3 planePoint, Vector3 planeNormal)
        {
            Plane plane = new Plane(planeNormal, planePoint);
            if (plane.Raycast(ray, out float enter))
            {
                return ray.GetPoint(enter);
            }

            return ray.GetPoint(Mathf.Infinity);
        }
    }
}
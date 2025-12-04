using DG.Tweening;
using UnityEngine;

public class ZoomingAndRotate : MonoBehaviour
{
    private Outline _outline;
    bool isTargeted = false;

    Vector3 originalPos;
    Quaternion originalRot;
    Vector3 originalScale;

    Renderer _renderer;   // để lấy center của mesh

    public bool IsTargeted => isTargeted;

    private void Start()
    {
        originalPos = transform.position;
        originalRot = transform.rotation;
        originalScale = transform.localScale;

        // Lấy renderer (nếu model ở child thì dùng GetComponentInChildren)
        _renderer = GetComponentInChildren<Renderer>();
        if (_renderer == null)
        {
            Debug.LogWarning($"[{name}] Không tìm thấy Renderer để tính center, RotateAround sẽ không hoạt động đúng.");
        }
    }

    private void Update()
    {
        if (!isTargeted) return;

        // Nếu không có renderer thì khỏi xoay center
        if (_renderer != null && Input.GetMouseButton(0))
        {
            float rotX = Input.GetAxis("Mouse X") * 5f; // kéo ngang
            float rotY = Input.GetAxis("Mouse Y") * 5f; // kéo dọc

            // Tâm thật của mesh trong world space
            Vector3 center = _renderer.bounds.center;

            // 🎯 XOAY QUANH CENTER – full 3D (yaw + pitch)
            // yaw (trái/phải) quanh trục Y world
            transform.RotateAround(center, Vector3.up, -rotX);

            // pitch (lên/xuống) quanh trục X local (right)
            transform.RotateAround(center, transform.right, rotY);
        }

        // 🔍 ZOOM bằng con lăn chuột (scale)
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            float zoomSpeed = 1f;

            Vector3 scale = transform.localScale;
            scale += Vector3.one * scroll * zoomSpeed;

            scale.x = Mathf.Clamp(scale.x, scale.x * 0.2f, scale.x * 5);
            scale.y = Mathf.Clamp(scale.y, scale.y * 0.2f, scale.y *5);
            scale.z = Mathf.Clamp(scale.z, scale.z * 0.2f, scale.z *5);

            transform.localScale = scale;
        }
    }

    public void OnChosingTarget()
    {
        isTargeted = true;
        Debug.Log("Chose target");

        if (!TryGetComponent(out _outline))
            _outline = gameObject.AddComponent<Outline>();

        _outline.OutlineColor = Color.red;
        _outline.OutlineWidth = 5f;
    }

    public void OutChosing()
    {
        isTargeted = false;

        if (_outline != null)
        {
            Destroy(_outline);
            _outline = null;
        }

        // Trả về trạng thái ban đầu
        transform.DOMove(originalPos, 1f);
        transform.DORotateQuaternion(originalRot, 1f);
        transform.DOScale(originalScale, 1f);
    }
}

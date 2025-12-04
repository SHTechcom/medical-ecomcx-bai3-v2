using DG.Tweening;
using UnityEngine;

public class DragSnapOnCollision : MonoBehaviour
{
    public Transform snapTarget;          // vị trí snap
    public float snapSpeed = 15f;         // tốc độ snap mượt
    public bool snapped = false;          // đã snap hay chưa

    private bool isDragging = false;
    private Vector3 offset;
    public Camera cam;
    public GameObject binhPhong, thisCanvas;
    void OnMouseDown()
    {
        if (snapped) return; // nếu đã snap thì không cho kéo nữa

        isDragging = true;

        float z = cam.WorldToScreenPoint(transform.position).z;
        Vector3 mouse = Input.mousePosition;
        mouse.z = z;

        offset = transform.position - cam.ScreenToWorldPoint(mouse);
    }

    void OnMouseDrag()
    {
        if (!isDragging || snapped) return;

        float z = cam.WorldToScreenPoint(transform.position).z;
        Vector3 mouse = Input.mousePosition;
        mouse.z = z;

        Vector3 worldPos = cam.ScreenToWorldPoint(mouse) + offset;

        // Giữ nguyên giá trị Y (không cho di chuyển)
        worldPos.y = transform.position.y;

        transform.position = worldPos;
    }

    void OnMouseUp()
    {
        isDragging = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Nếu collider mà object va chạm trùng với vùng snap
        if (other.CompareTag("SnapZone") && !snapped)
        {
            snapped = true;

            // Snap mượt bằng coroutine
            StopAllCoroutines();
            StartCoroutine(SnapRoutine());
        }
    }

    System.Collections.IEnumerator SnapRoutine()
    {
        while (Vector3.Distance(transform.position, snapTarget.position) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, snapTarget.position, Time.deltaTime * snapSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, snapTarget.rotation, Time.deltaTime * snapSpeed);
            yield return null;
        }

        transform.position = snapTarget.position;
        transform.rotation = snapTarget.rotation;
        binhPhong.SetActive(true);
        gameObject.SetActive(false);
        thisCanvas.SetActive(true);
        // 👇 tại đây bạn có thể gọi event khác như: mở cửa, bật animation, hoàn thành puzzle...
    }

    public void BinhPhongAnim()
    {
        gameObject.transform.DOMove(snapTarget.position, 3) .OnComplete(() =>
        {
           thisCanvas.SetActive(true);
        });;
        gameObject.transform.DORotate(snapTarget.transform.rotation.eulerAngles, 1);

    } 
         
}

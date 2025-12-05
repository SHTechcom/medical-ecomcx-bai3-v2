using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class InputZoomingManager : MonoBehaviour
{
    Vector3 lastPos;
    Quaternion lastRot;

    public GameObject canvas;
    public Camera _cam;
    [SerializeField] Transform _cameraTrans;
    [SerializeField] Button backButton;
    [SerializeField] Button freeBtn;
    [SerializeField] Button fixxedBtn;
    //[SerializeField] Transform transformMoveToCamera;
    [SerializeField] Transform transformMoveToPrefabs;
    public CameraController cameraController;
    ZoomingAndRotate zoomingTarget;

    [SerializeField] float zoomingDuration = 1f;

    private void Start()
    {
        cameraController.type = CameraType.Lock;
        freeBtn.onClick.AddListener(() =>
        {
            StartFreeMode();
        });
        fixxedBtn.onClick.AddListener(() =>
        {
            StopMode();
        });
        backButton.onClick.AddListener(OnClickBackButton);
    }

    private void Update()
    {
        if (cameraController.type == CameraType.Lock) return;
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Hit: " + hit.transform.name);

                if (hit.transform.TryGetComponent<ZoomingAndRotate>(out var a))
                {
                    backButton.gameObject.SetActive(true);
                    // Nếu đã có target đang được chọn rồi thì không cho chọn thêm nữa
                    if (zoomingTarget != null && zoomingTarget.IsTargeted)
                        return;

                    // Lưu lại vị trí/rotation camera trước khi zoom
                    lastPos = _cameraTrans.position;
                    lastRot = _cameraTrans.rotation;

                    // Zoom camera tới vị trí mong muốn
                    //_cameraTrans.DOMove(transformMoveToCamera.position, zoomingDuration);
                    //_cameraTrans.DORotateQuaternion(transformMoveToCamera.rotation, zoomingDuration);

                    // Đánh dấu target
                    zoomingTarget = a;
                    a.OnChosingTarget();

                    // DI CHUYỂN + XOAY OBJECT tới transformMoveToPrefabs
                    a.transform.DOMove(transformMoveToPrefabs.position, zoomingDuration);
                    a.transform.DORotateQuaternion(transformMoveToPrefabs.rotation, zoomingDuration);
                }
            }
        }
    }

    private List<Canvas> openedCanvasCached = new List<Canvas>();

    public void Show()
    {
        canvas.SetActive(true);
    }

    public void Hide()
    {
        canvas.SetActive(false);
        OnClickBackButton();
        StopMode();
    }

    private void OnClickBackButton()
    {
        // BACK LẦN 1: nếu vẫn đang có target → trả target về chỗ cũ
        if (zoomingTarget != null && zoomingTarget.IsTargeted)
        {
            zoomingTarget.OutChosing();   // trong này tự move/rotate về vị trí ban đầu
            zoomingTarget = null;
            backButton.gameObject.SetActive(false);
            return;
        }
    }

    public void StartFreeMode()
    {
        cameraController.type = CameraType.Free;
        var canvases = FindObjectsOfType<Canvas>(true);
        foreach (var c in canvases)
        {
            if (c.gameObject.activeSelf && c.gameObject.GetInstanceID() != canvas.gameObject.GetInstanceID())
            {
                openedCanvasCached.Add(c);
            }
            c.gameObject.SetActive(false);
        }
        canvas.gameObject.SetActive(true);
        _cameraTrans.gameObject.SetActive(true);
        freeBtn.gameObject.SetActive(false);
        fixxedBtn.gameObject.SetActive(true);
    }

    public void StopMode()
    {
        cameraController.type = CameraType.Lock;
        openedCanvasCached.ForEach(i => i.gameObject.SetActive(true));
        openedCanvasCached.Clear();
        _cameraTrans.gameObject.SetActive(false);
        freeBtn.gameObject.SetActive(true);
        fixxedBtn.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);
        if (zoomingTarget != null && zoomingTarget.IsTargeted)
        {
            zoomingTarget.OutChosing();   // trong này tự move/rotate về vị trí ban đầu
            zoomingTarget = null;
            backButton.gameObject.SetActive(false);
            return;
        }
    }
}

using System.Collections;
using UnityEngine;
using System.Runtime.InteropServices;

public class ScreenshotManager : MonoBehaviour
{
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")] private static extern void ShowWebGLScreenshot(string dataUrl);
#endif

    [SerializeField] private Camera targetCamera; // Cho phép gán camera cụ thể, nếu để trống thì tự lấy Camera.main
    [SerializeField] private AudioClip shutterSound;
    [SerializeField] private AudioSource shutterSoundSource;

    private Texture2D screenShot;

    private void Start()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;
    }

    public void GetScreenShot()
    {
        if (targetCamera == null)
        {
            Debug.LogError("Không tìm thấy Camera để chụp ảnh!");
            return;
        }

        Debug.Log("Đang chụp ảnh...");

        // Xóa texture cũ để tránh rò rỉ bộ nhớ
        if (screenShot != null)
        {
            Destroy(screenShot);
        }

        // Tạo RenderTexture để chứa hình tạm thời
        screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGBA32, false);
        //RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);

        //targetCamera.targetTexture = renderTexture;
        //targetCamera.Render();

        //RenderTexture.active = renderTexture;
        //screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        //screenShot.Apply();

        screenShot = ScreenCapture.CaptureScreenshotAsTexture();

        // Dọn bộ nhớ
        targetCamera.targetTexture = null;
        RenderTexture.active = null;
        //Destroy(renderTexture);

        // Phát âm thanh chụp
        if (shutterSoundSource != null && shutterSound != null)
            shutterSoundSource.PlayOneShot(shutterSound);

#if UNITY_EDITOR
        Debug.Log("Ảnh chụp chỉ được hiển thị trên bản WebGL (ở Editor chỉ lưu local).");
#elif UNITY_WEBGL && !UNITY_EDITOR
            byte[] textureBytes = screenShot.EncodeToJPG();
            string dataUrlStr = "data:image/jpeg;base64," + System.Convert.ToBase64String(textureBytes);
            ShowWebGLScreenshot(dataUrlStr);
#endif
    }
}
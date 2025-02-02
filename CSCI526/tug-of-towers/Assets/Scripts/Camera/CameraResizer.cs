using UnityEngine;

public class CameraResizer : MonoBehaviour
{
    private Camera _camera;
    [SerializeField] private float targetWidth = 20f;  // The desired width in world units that you want to fit
    [SerializeField] private float targetAspectRatio = 16f / 9f; // The default aspect ratio (change as needed)

    private void Start()
    {
        _camera = GetComponent<Camera>();
        AdjustCameraSize();
    }

    private void Update()
    {
        // Continuously check if the aspect ratio has changed and adjust the camera size accordingly
        AdjustCameraSize();
    }

    private void AdjustCameraSize()
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;

        // Adjust the orthographic size to maintain the targetWidth, taking into account the aspect ratio
        float scaleFactor = screenAspect / targetAspectRatio;
        _camera.orthographicSize = targetWidth / 2f / scaleFactor;
    }
}

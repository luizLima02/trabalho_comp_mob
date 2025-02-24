using UnityEngine;

public class CameraShaker : MonoBehaviour
{

    [Header("Camera Shaker Config")]
    private Vector3 cameraInitialPosition;
    public float shakerMagnitude = 0.05f;
    public float shakeTime = 0.5f;
    public Camera mainCamera;

    public void ShakeIt()
    {
        cameraInitialPosition = mainCamera.transform.position;
        InvokeRepeating("StartCameraShaking", 0f, 0.005f);
        Invoke("StopCameraShaking", shakeTime);
    }
    
    void StartCameraShaking()
    {
        float cameraShakingOffsetX = Random.value * shakerMagnitude * 2 - shakerMagnitude;
        float cameraShakingOffsetY = Random.value * shakerMagnitude * 2 - shakerMagnitude;
        Vector3 cameraIntermediatePosition = mainCamera.transform.position;

        cameraIntermediatePosition.x += cameraShakingOffsetX;
        cameraIntermediatePosition.y += cameraShakingOffsetY;
        mainCamera.transform.position = cameraIntermediatePosition;
    }

    void StopCameraShaking()
    {
        CancelInvoke("StartCameraShaking");
        mainCamera.transform.position = cameraInitialPosition;
    }
}

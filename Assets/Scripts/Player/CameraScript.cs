using UnityEngine;
using DG.Tweening;

public class CameraScript : MonoBehaviour
{
    [Header("Fov")]

    [SerializeField] private float fovTime = 0.25f;
    private Camera cam;

    private void Awake()
    {
        cam ??= GetComponent<Camera>();
    }

    private void DoFov(float endValue)
        => cam.DOFieldOfView(endValue, fovTime);

    private void DoFov(float endValue, float time)
        => cam.DOFieldOfView(endValue, time);
}

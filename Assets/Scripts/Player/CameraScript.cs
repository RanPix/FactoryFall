using UnityEngine;
using DG.Tweening;

public class CameraScript : MonoBehaviour
{
    [Header("Fov")]

    [SerializeField] private float fovTime = 0.25f;

    private void DoFov(float endValue)
        => GetComponent<Camera>().DOFieldOfView(endValue, fovTime);

    private void DoFov(float endValue, float time)
        => GetComponent<Camera>().DOFieldOfView(endValue, time);
}

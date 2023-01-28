using UnityEngine;

public class NicknameLookAtCamera : MonoBehaviour
{
    [SerializeField] private Transform localPlayer;
    private Camera cam;

    private void Start()
    {
        localPlayer = GameObject.FindGameObjectWithTag("LocalPlayer")?.GetComponent<Transform>();
        cam = Camera.main;
    }

    private void LateUpdate()
    {
        if (localPlayer)
        {
            transform.LookAt(cam.transform);
        }
        else
        {
            cam = Camera.main;
            localPlayer = GameObject.FindGameObjectWithTag("LocalPlayer")?.GetComponent<Transform>();
        }

    }
}

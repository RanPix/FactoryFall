using UnityEngine;

public class UpdatingNicknameTransform : MonoBehaviour
{
    [SerializeField] private Transform localPlayer;

    void Start()
    {
            localPlayer = GameObject.FindGameObjectWithTag("LocalPlayer")?.GetComponent<Transform>();
    }

    void LateUpdate()
    {
        if (localPlayer)
        {
            transform.LookAt(localPlayer);
        }
        else
            localPlayer = GameObject.FindGameObjectWithTag("LocalPlayer")?.GetComponent<Transform>();
    }
}

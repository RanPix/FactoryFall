using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [HideInInspector] public Transform cameraPosition;

    private void LateUpdate()
        => transform.position = cameraPosition.position;
}

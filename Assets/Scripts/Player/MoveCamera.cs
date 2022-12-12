using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform cameraPosition;

    private void LateUpdate()
        => transform.position = cameraPosition.position;
}

using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform cameraPosition;

    private void LateUpdate()
    {
        if (cameraPosition != null)
            transform.position = cameraPosition.position;
    }
}

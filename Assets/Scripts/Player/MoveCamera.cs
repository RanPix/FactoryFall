using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveCamera : MonoBehaviour
{
    public Transform cameraPosition;

    private void LateUpdate()
    {
        if (!transform || !cameraPosition) 
            return;
        print("Camera "+SceneManager.GetActiveScene().name);
        transform.position = cameraPosition.position;
    }
}

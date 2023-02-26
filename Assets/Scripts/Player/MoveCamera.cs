using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveCamera : MonoBehaviour
{
    public Transform cameraPosition;

    private void LateUpdate()
    {
        transform.position = cameraPosition.position;
        print(SceneManager.GetActiveScene().name);
    }
}

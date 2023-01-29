using UnityEngine;

public class MiniMapCameraMove : MonoBehaviour
{
    private Transform player;

    private bool firstSetup = true;


    public void Setup(Transform player)
    {
        if (!firstSetup)
            return;

        this.player = player;

        firstSetup = false;
    }

    private void LateUpdate()
    {
        this.transform.SetPositionAndRotation(new Vector3(player.position.x, 291.6f, player.position.z), new Quaternion(90, 0, player.rotation.z, 90));
    }
}

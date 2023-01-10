using UnityEngine;

public class MiniMapCameraMove : MonoBehaviour
{
    public Transform player;
    [field: SerializeField]public LayerMask maskWithEnemyPlayer { get; private set; }
    [field: SerializeField]public LayerMask maskWithOutEnemyPlayer{ get; private set; }
    void Start()
    {
        GetComponent<Camera>().cullingMask = maskWithEnemyPlayer;
    }

    void LateUpdate()
    {
        this.transform.SetPositionAndRotation(new Vector3(player.position.x, 291.6f, player.position.z), new Quaternion(90, 0, player.rotation.z, 90));

    }
}

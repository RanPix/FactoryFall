using UnityEngine;
using UnityEngine.UI;

public class PlayerMark : MonoBehaviour
{

    public Transform rotationReference;
    public Transform player;

    public GameObject localMark;
    public GameObject enemyMark;
    public GameObject friendlyMark;

    public bool isLocal;
    private GameObject activeMark;
    void LateUpdate()
    {
        if(player)
            this.transform.SetPositionAndRotation(new Vector3(player.position.x, 287f, player.position.z), new Quaternion(0, rotationReference.rotation.y, 0, rotationReference.rotation.w));

    }
}

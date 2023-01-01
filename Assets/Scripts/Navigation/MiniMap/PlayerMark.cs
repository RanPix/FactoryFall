using UnityEngine;

public class PlayerMark : MonoBehaviour
{
    public Transform rotationReference;
    public Transform player;

    private Canvas canvas;
    public bool isLocal;

    private GameObject activeMark;
    // Start is called before the first frame update
    void Start()
    {
        canvas = transform.GetComponentInChildren<Canvas>();
        canvas.worldCamera = transform.GetComponentInParent<Camera>();
        if (isLocal)
        {
            activeMark = canvas.transform.GetChild(0).gameObject;
            activeMark.SetActive(true);
            gameObject.layer = LayerMask.NameToLayer("LocalPlayerMark");
            gameObject.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("LocalPlayerMark");
        }
        else
        {
            activeMark = canvas.transform.GetChild(1).gameObject;
            activeMark.SetActive(true);
            gameObject.layer = LayerMask.NameToLayer("EnemyPlayerMark");
            gameObject.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("EnemyPlayerMark");

        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
            this.transform.SetPositionAndRotation(new Vector3(player.position.x, 40, player.position.z), new Quaternion(0, rotationReference.rotation.y, 0, rotationReference.rotation.w));

    }
}

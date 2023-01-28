using UnityEngine;
using UnityEngine.UI;

public class PlayerMark : MonoBehaviour
{

    public Transform rotationReference;
    public Transform player;

    public GameObject localMark;
    public GameObject enemyMark;
    public GameObject friendlyMark;

    private Canvas canvas;
    public bool isLocal;




    private GameObject activeMark;
    // Start is called before the first frame update
    void Start()
    {
        /*canvas = transform.GetComponentInChildren<Canvas>();
        canvas.worldCamera = transform.GetComponentInParent<Camera>();
        activeMark = canvas.transform.GetChild(0).gameObject;
        if (isLocal)
        {
            gameObject.layer = LayerMask.NameToLayer("LocalPlayerMark");
            activeMark.layer = LayerMask.NameToLayer("LocalPlayerMark");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("EnemyPlayerMark");
            activeMark.layer = LayerMask.NameToLayer("EnemyPlayerMark");
            activeMark.GetComponent<RawImage>().color = enemyColor;

        }*/
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(player)
            this.transform.SetPositionAndRotation(new Vector3(player.position.x, 287f, player.position.z), new Quaternion(0, rotationReference.rotation.y, 0, rotationReference.rotation.w));

    }
}

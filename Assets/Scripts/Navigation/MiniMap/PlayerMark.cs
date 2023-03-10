using UnityEngine;

public class PlayerMark : MonoBehaviour
{
    private Transform player;
    private Transform rotationReference;

    [SerializeField] private GameObject localMark;
    [SerializeField] private GameObject blueTeam;
    [SerializeField] private GameObject redTeam;

    //private bool isLocal;
    private bool firstSetup = true;


    public void Setup(Team team, bool isLocal, Transform targetPlayer, Transform targetRotationReference)
    {
        if (!firstSetup)
            return;

        player = targetPlayer;
        rotationReference = targetRotationReference;


        if (isLocal)
        {
            localMark.SetActive(true);
            firstSetup = false;
            return;
        }

        if (team == Team.Blue)
            blueTeam.SetActive(true);
        else if (team == Team.Red)
            redTeam.SetActive(true);

        firstSetup = false;
    }

    void LateUpdate()
    {
        if (player)
            this.transform.SetPositionAndRotation(new Vector3(player.position.x, 287f, player.position.z), new Quaternion(0, rotationReference.rotation.y, 0, rotationReference.rotation.w));
    }
}

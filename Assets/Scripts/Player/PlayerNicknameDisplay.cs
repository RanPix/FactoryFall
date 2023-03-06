using System.Collections;
using Player;
using UnityEngine;
using TMPro;
using Mirror;

public class PlayerNicknameDisplay : MonoBehaviour
{
    [SerializeField] private Transform localPlayer;

    private Camera cam;

    [SerializeField] private Material normalFontMaterial;
    [SerializeField] private Material ROTFontMaterial; // render on top

    [SerializeField] private TMP_Text text;
    [SerializeField] private Team thisTeam;
    [SerializeField] private bool isInSameTeam;

    private bool nicknameSettedUp;

    private IEnumerator WaitForLocalPlayer(Team team)
    {
        yield return new WaitUntil(() => NetworkClient.localPlayer == null);
        ULTRASetup(team);
    }

    public void Setup(string name, Team team)
    {
        text = GetComponentInChildren<TMP_Text>();

        thisTeam = team;
        text.color = TeamToColor.GetTeamColor(team);
        text.text = name;

        if (NetworkClient.localPlayer)
        {
            ULTRASetup(team);
        }
        else
        {
            StartCoroutine(WaitForLocalPlayer(team));
        }
    }

    private void ULTRASetup(Team team)
    {
        localPlayer = NetworkClient.localPlayer?.GetComponent<Transform>();
        print("local player = " + NetworkClient.localPlayer);
        Team localPlayerTeam = localPlayer.GetComponent<GamePlayer>().team;

        isInSameTeam = localPlayerTeam == team; //?
        //true:
        //false;
        GetComponentInChildren<TMP_Text>().fontSharedMaterial =
            isInSameTeam ?
                ROTFontMaterial :
                normalFontMaterial;

        nicknameSettedUp = true;

        cam = Camera.main;

    }


    private void LateUpdate()
    {
        if (localPlayer)
        {
            transform.LookAt(cam.transform);

            //print($"team {thisTeam}");

            if (isInSameTeam)
            {
                //print("insane");

                float magnitudeFromCamera = Mathf.Clamp((cam.transform.position - transform.position).magnitude * 0.09f, 2f, 1000f);
                transform.localScale = new Vector3(magnitudeFromCamera, magnitudeFromCamera, magnitudeFromCamera);
            }

            if (nicknameSettedUp == false)
            {
                Team localPlayerTeam = localPlayer.GetComponent<GamePlayer>().team;

                GetComponentInChildren<TMP_Text>().fontSharedMaterial =
                    localPlayerTeam == thisTeam ?
                    ROTFontMaterial :
                    normalFontMaterial;

                nicknameSettedUp = true;
            }
        }
        else
        {
            cam = Camera.main;
            localPlayer = NetworkClient.localPlayer?.GetComponent<Transform>();
        }

    }
}

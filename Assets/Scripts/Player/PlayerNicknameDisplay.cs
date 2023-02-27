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

    private bool nicknameSettedUp;

    public void Setup(string name, Team team)
    {
        text = GetComponentInChildren<TMP_Text>();

        thisTeam = team;
        text.color = TeamToColor.GetTeamColor(team);
        text.text = name;


        localPlayer = NetworkClient.localPlayer?.GetComponent<Transform>();

        if (localPlayer)
        {
            Team localPlayerTeam = localPlayer.GetComponent<GamePlayer>().team;

            GetComponentInChildren<TMP_Text>().fontSharedMaterial =
                localPlayerTeam == team ?
                ROTFontMaterial :
                normalFontMaterial;

            nicknameSettedUp = true;
        }

        cam = Camera.main;
    }


    private void LateUpdate()
    {
        if (localPlayer)
        {
            transform.LookAt(cam.transform);

            float magnitudeFromCamera = (cam.transform.position - transform.position).magnitude * 0.09f;
            transform.localScale = new Vector3(magnitudeFromCamera, magnitudeFromCamera, magnitudeFromCamera);

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
